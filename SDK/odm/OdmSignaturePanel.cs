using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.odm
{
    /// <summary>
    /// ODM for Signature Panel
    /// </summary>
    internal class OdmSignaturePanel : OdmBase
    {
        public OdmSignaturePanel() : base() { }

        public bool Save(CSSignaturePanel panel)
        {
            return base.CommitChanges
            (
                "SaveSignaturePanel",
                new Dictionary<string, object>()
                {
                    { "@Id", panel.Id },
                    { "@Type", (int)panel.PanelType },
                    { "@TimeLimited", panel.IsTimelimited },
                    { "@Deadline", (panel.IsTimelimited ? panel.Deadline : DateTime.MinValue) },
                    { "@State", panel.State },
                    { "@ResponsesInProgress", panel._sentForResponses },
                    { "@ModifiedBy", panel.ModifiedBy.Id },
                    { "@Modified", panel.Modified }
                }
            );
        }

        public CSSignaturePanel Get(Guid id)
        {
            DataSet ds = base.GetData
            (
                "GetSignaturePanelById",
                new Dictionary<string, object>()
                {
                    { "@Id", id }
                }
            );

            if (base.HasData(ds))
            {
                return PopulateSignaturePanel(ds.Tables[0].Rows[0]);
            }

            return null;
        }

        public bool Save(CSSignatureItem item)
        {
            return base.CommitChanges
            (
                "SaveSignaturePanelItem",
                new Dictionary<string, object>()
                {
                    { "@Id", item.Id },
                    { "@PanelId", item.Panel.Id },
                    { "@RespondentId", item.Respondent.Id },
                    { "@IsDecider", item.ResponsesIsFinalDecision },
                    { "@IsTieBreaker", item.UseResponseAsTieBreaker },
                    { "@IsMandatory", item.IsMandatoryMember },
                    { "@Response", (int)item.State },
                    { "@Comment", item.Comment },
                    { "@SentToResponder", item._sentToResponder },
                    { "@ModifiedBy", item.ModifiedBy.Id },
                    { "@Modified", item.Modified }
                }
            );
        }

        public List<CSSignatureItem> GetSignaturesForPanel(CSSignaturePanel panel)
        {
            List<CSSignatureItem> list = new List<CSSignatureItem>();

            DataSet ds = base.GetData
            (
                "GetSignaturePanelItemByPanelId",
                new Dictionary<string, object>()
                {
                    { "@PanelId", panel.Id }
                }
            );

            if (base.HasData(ds))
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateSignaturePanelItem(panel, row));
                }
            }

            return list;
        }

        public bool Delete(CSSignatureItem item)
        {
            return base.CommitChanges
            (
                "DeleteSignaturePanelItemById",
                new Dictionary<string, object>()
                {
                    { "@Id", item.Id }
                }
            );
        }

        private CSSignatureItem PopulateSignaturePanelItem(CSSignaturePanel panel, DataRow row)
        {
            return new CSSignatureItem()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                Panel = panel,
                Respondent = CSUser.GetById(Utility.SafeConvertToGuid(row["RespondentId"])),
                IsMandatoryMember = Utility.SafeConvertToBool(row["IsMandatory"]),
                ResponsesIsFinalDecision = Utility.SafeConvertToBool(row["IsFinalDecider"]),
                UseResponseAsTieBreaker = Utility.SafeConvertToBool(row["IsTieBreaker"]),
                State = (SignatureItemStateEnum)Utility.SafeConvertToInt(row["Response"]),
                Comment = Utility.SafeString(row["Comment"], null),
                RespondedOn = Utility.SafeConvertToDateTime(row["RespondedOn"]),
                _sentToResponder = Utility.SafeConvertToBool(row["SentToResponder"]),
                Created = Utility.SafeConvertToDateTime(row["Created"]),
                _createdById = Utility.SafeConvertToGuid(row["CreatedBy"]),
                Modified = Utility.SafeConvertToDateTime(row["Modified"]),
                _modifiedById = Utility.SafeConvertToGuid(row["ModifiedBy"])
            };
        }

        private CSSignaturePanel PopulateSignaturePanel(DataRow row)
        {
            return new CSSignaturePanel()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                PanelType = (SignaturePanelTypeEnum)Utility.SafeConvertToInt(row["PanelType"]),
                IsTimelimited = Utility.SafeConvertToBool(row["IsTimeLimited"]),
                Deadline = Utility.SafeConvertToDateTime(row["Deadline"]),
                _sentForResponses = Utility.SafeConvertToBool(row["ResponsesInProgress"]),
                Created = Utility.SafeConvertToDateTime(row["Created"]),
                _createdById = Utility.SafeConvertToGuid(row["CreatedBy"]),
                Modified = Utility.SafeConvertToDateTime(row["Modified"]),
                _modifiedById = Utility.SafeConvertToGuid(row["ModifiedBy"])
            };
        }

    }
}
