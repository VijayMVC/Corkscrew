using System.Collections.Generic;

namespace Corkscrew.SDK.ActiveDirectory.odm
{
    internal class OdmActiveDirectoryUser : OdmBase
    {

        public OdmActiveDirectoryUser() : base() { }

        /// <summary>
        /// Saves the user
        /// </summary>
        /// <param name="user">CSUser to save</param>
        public void Save(CSActiveDirectoryUser user)
        {
            base.CommitChanges
            (
                "UserSave",
                new Dictionary<string, object>()
                {
                    { "@Id", user.Id },
                    { "@Username", user.Username },
                    { "@SecretHash", string.Empty },
                    { "@DisplayName", user.DisplayName },
                    { "@EmailAddress", user.EmailAddress },
                    { "@IsWinADUser", true }                     // this is ALWAYS true from this Odm.
                }
            );
        }

        /// <summary>
        /// Saves the user
        /// </summary>
        /// <param name="group">CSUser to save</param>
        public void Save(CSActiveDirectoryUserGroup group)
        {
            base.CommitChanges
            (
                "UserSave",
                new Dictionary<string, object>()
                {
                    { "@Id", group.Id },
                    { "@Username", group.Username },
                    { "@DisplayName", group.DisplayName },
                    { "@EmailAddress", group.EmailAddress },
                    { "@IsWinADGroup", true }                     // this is ALWAYS true from this Odm.
                }
            );
        }

    }
}
