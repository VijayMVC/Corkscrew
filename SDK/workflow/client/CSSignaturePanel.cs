using Corkscrew.SDK.objects;
using Corkscrew.SDK.odm;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Corkscrew.SDK.workflow
{

    #region Enums

    /// <summary>
    /// Type of signature panel
    /// </summary>
    public enum SignaturePanelTypeEnum
    {
        /// <summary>
        /// At least one person must respond. Whatever the first response is, 
        /// that will be taken
        /// </summary>
        AtleastOneResponse = 0,

        /// <summary>
        /// At least one person must approve. First "Approval" will terminate 
        /// signature panel
        /// </summary>
        AtleastOneApproves,

        /// <summary>
        /// If at least one person rejects, signature panel rejects
        /// </summary>
        EvenOneRejects,

        /// <summary>
        /// Votes must be unanimous. If all approve, signature panel is approved. If 
        /// all reject, signature panel is rejected. If there are a mix of approvals and 
        /// rejections, state is set to Deadlocked.
        /// </summary>
        Unanimous,

        /// <summary>
        /// The decision of the majority of polled users will apply. 
        /// </summary>
        Majority

    }

    /// <summary>
    /// Status of a single signature item
    /// </summary>
    public enum SignatureItemStateEnum
    {
        /// <summary>
        /// User has not responded
        /// </summary>
        NoResponse = 0,

        /// <summary>
        /// Approved
        /// </summary>
        Approved,

        /// <summary>
        /// Rejected
        /// </summary>
        Rejected,

        /// <summary>
        /// User is unable to either approve or reject
        /// </summary>
        OnTheFence,

        /// <summary>
        /// This is set when the Signature Panel terminates externally, 
        /// indicates signature collection was aborted before this user responded.
        /// </summary>
        Expired
    }

    /// <summary>
    /// Aggregate state of the signature panel
    /// </summary>
    public enum SignaturePanelStateEnum
    {
        /// <summary>
        /// Not yet started
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// Response collection is on-going, until all the responses have been received (or signature panel terminates)
        /// </summary>
        SentForResponses,

        /// <summary>
        /// Approved
        /// </summary>
        Approved,

        /// <summary>
        /// Rejected
        /// </summary>
        Rejected,

        /// <summary>
        /// Vote is neither approved nor rejected. This status will apply only 
        /// after the signature panel is terminated. Until then, even if the conditions match, the status 
        /// will remain SentForResponses.
        /// </summary>
        Deadlocked, 

        /// <summary>
        /// The deadline for the panel expired. If this state is set, no member registered a vote within the 
        /// set deadline.
        /// </summary>
        DeadlineExpired
    }

    #endregion

    /// <summary>
    /// Signals completion of a signature panel
    /// </summary>
    /// <param name="sender">The signature panel instance that was completed</param>
    public delegate void SignaturePanelCompleted(CSSignaturePanel sender);

    /// <summary>
    /// A signature panel. Created for approval workflows, this keeps track of how many contacts have responded and their responses.
    /// </summary>
    public class CSSignaturePanel
    {
        // accessed from ODM
        internal CSUser _currentCredential = null;

        /// <summary>
        /// Event signaled when the signature panel terminates
        /// </summary>
        public event SignaturePanelCompleted Completed;

        #region Properties

        /// <summary>
        /// Id of the panel
        /// </summary>
        public Guid Id
        {
            get;
            internal set;
        } = Guid.Empty;

        /// <summary>
        /// Panel members
        /// </summary>
        public IReadOnlyList<CSSignatureItem> Members
        {
            get
            {
                if (_members == null)
                {
                    _members = (new OdmSignaturePanel()).GetSignaturesForPanel(this);
                }

                return _members.AsReadOnly();
            }
        }
        private List<CSSignatureItem> _members = null;

        /// <summary>
        /// Type of signature panel. Default is a majority panel
        /// </summary>
        public SignaturePanelTypeEnum PanelType
        {
            get;
            internal set;
        } = SignaturePanelTypeEnum.Majority;

        /// <summary>
        /// Whether the signature panel is time-limited.
        /// </summary>
        public bool IsTimelimited
        {
            get;
            internal set;
        }

        /// <summary>
        /// If the panel is time-limited, the end date
        /// </summary>
        public DateTime Deadline
        {
            get;
            internal set;
        }

        #region Auditing
        /// <summary>
        /// Gets the date/time of creation
        /// </summary>
        public DateTime Created
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the user who created. Lazy-loaded.  
        /// </summary>
        public CSUser CreatedBy
        {
            get
            {
                if ((_createdBy == null) && (!_createdById.Equals(Guid.Empty)))
                {
                    _createdBy = CSUser.GetById(_createdById);
                }

                return _createdBy;
            }
            internal set
            {
                _createdBy = value;
                _createdById = ((value == null) ? Guid.Empty : _createdBy.Id);
            }
        }
        private CSUser _createdBy = null;
        internal Guid _createdById = Guid.Empty;


        /// <summary>
        /// Gets the date/time of last modification
        /// </summary>
        public DateTime Modified
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the user who last modified. Lazy-loaded.  
        /// </summary>
        public CSUser ModifiedBy
        {
            get
            {
                if ((_modifiedBy == null) && (!_modifiedById.Equals(Guid.Empty)))
                {
                    _modifiedBy = CSUser.GetById(_modifiedById);
                }

                return _modifiedBy;
            }
            internal set
            {
                _modifiedBy = value;
                _modifiedById = ((value == null) ? Guid.Empty : value.Id);
            }
        }
        private CSUser _modifiedBy = null;
        internal Guid _modifiedById = Guid.Empty;

        #endregion

        /// <summary>
        /// Returns if the deadline for the panel has expired. 
        /// Always returns FALSE for a no-deadline panel.
        /// </summary>
        public bool IsDeadlineExpired
        {
            get
            {
                if (!IsTimelimited)
                {
                    return false;
                }

                return (Deadline < DateTime.Now);
            }
        }


        /// <summary>
        /// Gets the current state of the signature panel (result)
        /// </summary>
        public SignaturePanelStateEnum State
        {
            get
            {
                SignaturePanelStateEnum result =
                    (
                        _sentForResponses 
                        ? SignaturePanelStateEnum.SentForResponses 
                        : SignaturePanelStateEnum.NotStarted
                    );

                int approvals = Approvals, rejections = Rejections;
                if ((approvals == 0) && (rejections == 0))
                {
                    return result;
                }
                
                switch (PanelType)
                {
                    case SignaturePanelTypeEnum.AtleastOneResponse:
                    case SignaturePanelTypeEnum.Majority:
                        if (approvals > rejections)
                        {
                            result = SignaturePanelStateEnum.Approved;
                        }
                        else if (approvals < rejections)
                        {
                            result = SignaturePanelStateEnum.Rejected;
                        }
                        else if (approvals == rejections)
                        {
                            result = SignaturePanelStateEnum.Deadlocked;
                        }
                        break;

                    case SignaturePanelTypeEnum.Unanimous:
                        if ((approvals > 0) && (rejections == 0))
                        {
                            result = SignaturePanelStateEnum.Approved;
                        }
                        else if ((rejections > 0) && (approvals == 0))
                        {
                            result = SignaturePanelStateEnum.Rejected;
                        }
                        else
                        {
                            result = SignaturePanelStateEnum.Deadlocked;
                        }
                        break;

                    case SignaturePanelTypeEnum.AtleastOneApproves:
                        if (approvals >= 1)
                        {
                            result = SignaturePanelStateEnum.Approved;
                        }
                        break;

                    case SignaturePanelTypeEnum.EvenOneRejects:
                        if (rejections >= 1)
                        {
                            result = SignaturePanelStateEnum.Rejected;
                        }
                        break;
                }

                if (IsDeadlineExpired && (result != SignaturePanelStateEnum.Approved) && (result != SignaturePanelStateEnum.Rejected) && (result != SignaturePanelStateEnum.Deadlocked))
                {
                    result = SignaturePanelStateEnum.DeadlineExpired;
                }

                return result;
            }
        }
        internal bool _sentForResponses = false; // flag set only if Start() was ever called sucessfully

        /// <summary>
        /// Gets number of approvals
        /// </summary>
        public int Approvals
        {
            get
            {
                int count = 0;

                foreach (CSSignatureItem member in Members)
                {
                    if (member.State == SignatureItemStateEnum.Approved)
                    {
                        if (member.ResponsesIsFinalDecision)
                        {
                            count = Members.Count;
                            break;
                        }

                        count++;

                        if (member.UseResponseAsTieBreaker)
                        {
                            count++;
                        }
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Gets number of rejections
        /// </summary>
        public int Rejections
        {
            get
            {
                int count = 0;

                foreach (CSSignatureItem member in Members)
                {
                    if (member.State == SignatureItemStateEnum.Rejected)
                    {
                        if (member.ResponsesIsFinalDecision)
                        {
                            count = Members.Count;
                            break;
                        }

                        count++;

                        if (member.UseResponseAsTieBreaker)
                        {
                            count++;
                        }
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Gets progress of signature collection, is a percentage value
        /// </summary>
        public int Progress
        {
            get
            {
                int voted = 0, total = 0;

                foreach (CSSignatureItem member in Members)
                {
                    total++;

                    if (member.State != SignatureItemStateEnum.NoResponse)
                    {
                        voted++;
                    }
                }

                return (int)(voted / (float)total * 100.0);
            }
        }

        #endregion

        #region Constructors

        // for ODM use
        internal CSSignaturePanel()
        {

        }

        /// <summary>
        /// Create a new signature panel
        /// </summary>
        /// <param name="type">Type of panel</param>
        /// <param name="createdBy">User creating the panel</param>
        public CSSignaturePanel(SignaturePanelTypeEnum type, CSUser createdBy)
            : this(type, createdBy, DateTime.MinValue)
        {

        }

        /// <summary>
        /// Create a new time-limited signature panel
        /// </summary>
        /// <param name="type">Type of panel</param>
        /// <param name="createdBy">User creating the panel</param>
        /// <param name="expireAfter">Timespan value after for this panel to expire</param>
        public CSSignaturePanel(SignaturePanelTypeEnum type, CSUser createdBy, TimeSpan expireAfter)
            : this(type, createdBy, DateTime.Now.Add(expireAfter))
        {

        }

        /// <summary>
        /// Create a new time-limited signature panel
        /// </summary>
        /// <param name="type">Type of panel</param>
        /// <param name="createdBy">User creating the panel</param>
        /// <param name="deadline">The deadline for this panel to expire. Set to DateTime.MinValue or some past value to disable deadline feature.</param>
        public CSSignaturePanel(SignaturePanelTypeEnum type, CSUser createdBy, DateTime deadline)
        {
            DateTime now = DateTime.Now;

            if ((createdBy == null) || (!createdBy.Login()))
            {
                throw new UnauthorizedAccessException("Credentials are not valid.");
            }

            Id = Guid.NewGuid();
            PanelType = type;
            IsTimelimited = false;
            Deadline = DateTime.MinValue;

            if (deadline > now)
            {
                IsTimelimited = true;
                Deadline = deadline;
            }

            Created = now;
            CreatedBy = createdBy;
            Modified = now;
            ModifiedBy = createdBy;

            _currentCredential = createdBy;

            Save();
        }

        /// <summary>
        /// Returns a signature panel given its Guid
        /// </summary>
        /// <param name="id">Guid of signature panel to return</param>
        /// <param name="credential">User credential to open with</param>
        /// <returns>The Signature Panel matching the Guid</returns>
        public static CSSignaturePanel Get(Guid id, CSUser credential)
        {
            if ((credential == null) || (! credential.Login()))
            {
                throw new UnauthorizedAccessException("Credentials are not valid.");
            }

            CSSignaturePanel panel = (new OdmSignaturePanel()).Get(id);
            panel._currentCredential = credential;

            return panel;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Creates a new signature panel member
        /// </summary>
        /// <param name="respondent">The user who is the respondent</param>
        /// <param name="mandatory">If set, user is marked as a mandatory (must respond to terminate panel)</param>
        /// <param name="isDecisionMaker">If set, user is marked as the decision maker (vote overrides and panel terminates)</param>
        /// <param name="isTieBreaker">If set, in case of deadlocks, the value of this vote doubles</param>
        /// <returns>The created panel member</returns>
        public CSSignatureItem AddPanelMember(CSUser respondent, bool mandatory = true, bool isDecisionMaker = false, bool isTieBreaker = false)
        {
            if (! IsValidStateToManageMember())
            {
                throw new InvalidOperationException("Panel is already completed. Cannot add a new panel member.");
            }

            if (respondent == null)
            {
                throw new ArgumentNullException("respondent");
            }

            // check for duplicates
            if (Members.Where(m => m.Respondent.Id.Equals(respondent.Id)).Count() > 0)
            {
                throw new InvalidOperationException("Panel member already exists.");
            }

            CSSignatureItem member = new CSSignatureItem(this, respondent, mandatory, isDecisionMaker, isTieBreaker);
            if (member != null)
            {
                _members = null;    // next access will retrieve the list again
            }

            return member;
        }

        /// <summary>
        /// Removes the given panel member. 
        /// This will only succeed if the panel member has not yet responded.
        /// </summary>
        /// <param name="respondent">User to remove</param>
        public void RemovePanelMember(CSUser respondent)
        {
            if (!IsValidStateToManageMember())
            {
                throw new InvalidOperationException("Panel is already completed. Cannot remove a panel member.");
            }

            foreach (CSSignatureItem member in Members)
            {
                if (((member.Respondent.Id.Equals(respondent.Id))) && (member.State == SignatureItemStateEnum.NoResponse))
                {
                    member.Delete();
                }
            }
            _members = null;    // retrieved on next access
        }

        /// <summary>
        /// Registers the member's response. If the response completes the signature panel, the signature panel is terminated.
        /// </summary>
        /// <param name="respondent">User responding</param>
        /// <param name="response">Response state</param>
        /// <param name="comment">Optional comment</param>
        public void RegisterResponse(CSUser respondent, SignatureItemStateEnum response, string comment)
        {
            if (State == SignaturePanelStateEnum.NotStarted)
            {
                throw new InvalidOperationException("Signature panel is not yet started. Call Start() first.");
            }

            if (!IsValidStateForResponse())
            {
                throw new InvalidOperationException("Signature panel is not in a valid state for a response.");
            }

            if (respondent == null)
            {
                throw new ArgumentNullException("respondent");
            }

            foreach(CSSignatureItem member in _members)
            {
                if ((member.Respondent.Id.Equals(respondent.Id)) && ((member.State == SignatureItemStateEnum.NoResponse) || (member.State == SignatureItemStateEnum.OnTheFence) || (member.State == SignatureItemStateEnum.Expired)))
                {
                    member.RegisterResponse(response, comment);
                    break;
                }
            }

            if (State != SignaturePanelStateEnum.SentForResponses)
            {
                // above response ended the panel
                Terminate();
            }
        }

        /// <summary>
        /// Send for responses
        /// </summary>
        public void Start()
        {
            if (State != SignaturePanelStateEnum.NotStarted)
            {
                throw new InvalidOperationException("Signature panel has already been started.");
            }

            foreach(CSSignatureItem member in _members)
            {
                member.SendForResponse();
            }

            _sentForResponses = true;
            Save();
        }

        /// <summary>
        /// Terminates the signature panel
        /// </summary>
        public void Terminate()
        {
            _sentForResponses = false;      // if this remains true, State will keep returning SentForResponses.
            Save();

            Completed?.Invoke(this);
        }

        /// <summary>
        /// Saves the panel metadata
        /// </summary>
        private void Save()
        {
            (new OdmSignaturePanel()).Save(this);
        }

        /// <summary>
        /// Returns if responses can be registered.
        /// </summary>
        /// <returns></returns>
        private bool IsValidStateForResponse()
        {
            if (State == SignaturePanelStateEnum.SentForResponses)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns if members can be added or removed.
        /// </summary>
        /// <returns></returns>
        private bool IsValidStateToManageMember()
        {
            switch (State)
            {
                case SignaturePanelStateEnum.NotStarted:
                case SignaturePanelStateEnum.SentForResponses:
                case SignaturePanelStateEnum.Deadlocked:
                    return true;
            }

            return false;
        }

        #endregion

    }

    /// <summary>
    /// A single item in a Corkscrew Signature Panel
    /// </summary>
    public class CSSignatureItem
    {

        internal bool _sentToResponder = false;

        #region Properties

        /// <summary>
        /// Reference to the signature panel parent
        /// </summary>
        internal CSSignaturePanel Panel
        {
            get;
            set;
        }

        /// <summary>
        /// Id of this item
        /// </summary>
        public Guid Id
        {
            get;
            internal set;
        } = Guid.Empty;

        /// <summary>
        /// Polled user who must respond
        /// </summary>
        public CSUser Respondent
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// If set, this member must mandatorily respond for the panel to complete
        /// </summary>
        public bool IsMandatoryMember
        {
            get;
            internal set;
        } = true;

        /// <summary>
        /// If set, whatever this member responds (if they do) becomes the decision
        /// </summary>
        public bool ResponsesIsFinalDecision
        {
            get;
            internal set;
        } = false;

        /// <summary>
        /// If set, in case of a deadlocked signature panel, this response doubles in value.
        /// </summary>
        public bool UseResponseAsTieBreaker
        {
            get;
            internal set;
        } = false;

        /// <summary>
        /// Current state of the panel item
        /// </summary>
        public SignatureItemStateEnum State
        {
            get;
            internal set;
        } = SignatureItemStateEnum.NoResponse;

        /// <summary>
        /// Any comments entered by the user while acting on the item
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set { _comment = value.SafeString(1024, true, true, nameof(Comment), null); }
        }
        private string _comment = null;

        /// <summary>
        /// Date/time when response was received
        /// </summary>
        public DateTime RespondedOn
        {
            get;
            internal set;
        } = DateTime.MinValue;

        #region Auditing
        /// <summary>
        /// Gets the date/time of creation
        /// </summary>
        public DateTime Created
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the user who created. Lazy-loaded.  
        /// </summary>
        public CSUser CreatedBy
        {
            get
            {
                if ((_createdBy == null) && (!_createdById.Equals(Guid.Empty)))
                {
                    _createdBy = CSUser.GetById(_createdById);
                }

                return _createdBy;
            }
            internal set
            {
                _createdBy = value;
                _createdById = ((value == null) ? Guid.Empty : _createdBy.Id);
            }
        }
        private CSUser _createdBy = null;
        internal Guid _createdById = Guid.Empty;


        /// <summary>
        /// Gets the date/time of last modification
        /// </summary>
        public DateTime Modified
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the user who last modified. Lazy-loaded.  
        /// </summary>
        public CSUser ModifiedBy
        {
            get
            {
                if ((_modifiedBy == null) && (!_modifiedById.Equals(Guid.Empty)))
                {
                    _modifiedBy = CSUser.GetById(_modifiedById);
                }

                return _modifiedBy;
            }
            internal set
            {
                _modifiedBy = value;
                _modifiedById = ((value == null) ? Guid.Empty : value.Id);
            }
        }
        private CSUser _modifiedBy = null;
        internal Guid _modifiedById = Guid.Empty;

        #endregion

        /// <summary>
        /// Returns the numeric value of the vote
        /// </summary>
        public int VoteValue
        {
            get
            {
                int value = 0;

                if (RespondedOn > DateTime.MinValue)
                {
                    /*
                        +1 for approved
                        -1 for rejected
                        0 for non commitals
                    */

                    value = ((State == SignatureItemStateEnum.Approved) ? 1 : ((State == SignatureItemStateEnum.Rejected) ? -1 : 0));

                    if (value != 0)
                    {
                        if (ResponsesIsFinalDecision)
                        {
                            value = ((value > 0) ? int.MaxValue : int.MinValue);
                        }
                        else if (UseResponseAsTieBreaker)
                        {
                            value = value * 2;
                        }
                    }
                }

                return value;
            }
        }

        #endregion

        #region Constructors

        // for use by ODM
        internal CSSignatureItem()
        {

        }

        /// <summary>
        /// Creates a new signature panel member
        /// </summary>
        /// <param name="panel">Signature panel</param>
        /// <param name="respondent">The user who is the respondent</param>
        /// <param name="mandatory">If set, user is marked as a mandatory (must respond to terminate panel)</param>
        /// <param name="isDecisionMaker">If set, user is marked as the decision maker (vote overrides and panel terminates)</param>
        /// <param name="isTieBreaker">If set, in case of deadlocks, the value of this vote doubles</param>
        internal CSSignatureItem(CSSignaturePanel panel, CSUser respondent, bool mandatory = true, bool isDecisionMaker = false, bool isTieBreaker = false)
        {
            if ((! mandatory) && (isTieBreaker))
            {
                throw new ArgumentException("Cannot set respondant as tie-breaker if response is not mandatory.");
            }

            Id = Guid.NewGuid();
            Panel = panel;
            Respondent = respondent;
            IsMandatoryMember = mandatory;
            ResponsesIsFinalDecision = isDecisionMaker;
            UseResponseAsTieBreaker = isTieBreaker;

            Save();
        }

        #endregion

        #region Instance Methods
        
        /// <summary>
        /// Register the member's response.
        /// </summary>
        /// <param name="response">The response to register</param>
        /// <param name="comment">Optional comment</param>
        internal void RegisterResponse(SignatureItemStateEnum response, string comment)
        {
            if ((State != SignatureItemStateEnum.NoResponse) && (State != SignatureItemStateEnum.OnTheFence) && (State != SignatureItemStateEnum.Expired))
            {
                throw new InvalidOperationException("Cannot register response. Panelist has already responded.");
            }

            State = response;
            Comment = comment;
            RespondedOn = DateTime.Now;

            Save();
        }


        /// <summary>
        /// Internal function to mark panelist as aborted. Works only if no response has been registered,
        /// will not throw an exception otherwise.
        /// </summary>
        /// <param name="comment">Optional comment</param>
        internal void MarkAborted(string comment)
        {
            if (State == SignatureItemStateEnum.NoResponse)
            {
                State = SignatureItemStateEnum.Expired;
                Comment = comment;
                RespondedOn = DateTime.Now;

                Save();
            }
        }

        /// <summary>
        /// Delete this panel member. Works only if no response has been registered.
        /// </summary>
        /// <returns>Sucess of deletion</returns>
        internal bool Delete()
        {
            if ((State == SignatureItemStateEnum.NoResponse) || (State == SignatureItemStateEnum.Expired))
            {
                return (new OdmSignaturePanel()).Delete(this);
            }

            return false;
        }

        /// <summary>
        /// Send email to the response member for a response
        /// </summary>
        internal void SendForResponse()
        {
            if (! _sentToResponder)
            {
                if (CSMailItem.QueueMail(Panel._currentCredential, "", "", "", "") == null)
                {
                    throw new Exception("Unable to send mail to response member for action.");
                }

                _sentToResponder = true;
                Save();
            }
        }

        /// <summary>
        /// Saves the item metadata
        /// </summary>
        private void Save()
        {
            (new OdmSignaturePanel()).Save(this);
        }

        #endregion

    }

}
