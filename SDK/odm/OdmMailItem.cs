using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.odm
{
    internal class OdmMailItem : OdmBase
    {
        public OdmMailItem() : base() { }

        public bool QueueMail(CSMailItem mail, CSUser queueUser)
        {
            return base.CommitChanges
            (
                "QueueMail"
                , new Dictionary<string, object>()
                {
                    { "@Id", mail.Id },
                    { "@From", mail.From },
                    { "@Recipient", mail.Recipient },
                    { "@InternalCC", mail.InternalCopyTo },
                    { "@Subject", mail.Subject },
                    { "@ContentHtml", mail.ContentHtml },
                    { "@WasSent", mail.WasSent },
                    { "@CreatingUserId", queueUser.Id }
                }
            );
        }

        public bool UpdateMailTry(CSMailItem mail)
        {
            return base.CommitChanges
            (
                "UpdateMailTry",
                new Dictionary<string, object>()
                {
                    { "@MailId", mail.Id },
                    { "@AttemptResult", mail.WasSent }
                }
            );
        }

        public CSMailItem GetById(Guid id)
        {
            DataSet ds = base.GetData
            (
                "GetQueuedMailItemById",
                new Dictionary<string, object>()
                {
                    { "@MailId", id }
                }
            );

            if (!base.HasData(ds))
            {
                return null;
            }

            return PopulateQueueItem(ds.Tables[0].Rows[0]);
        }

        public List<CSMailItem> GetUnsentMail(int count = 25)
        {
            List<CSMailItem> results = new List<CSMailItem>();

            DataSet ds = base.GetData
            (
                "GetQueuedMail", 
                new Dictionary<string, object>()
                {
                    { "@Count", count }
                }
            );

            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    results.Add(PopulateQueueItem(row));
                }
            }

            return results;
        }

        private CSMailItem PopulateQueueItem(DataRow row)
        {
            CSMailItem item = new CSMailItem()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                From = Utility.SafeString(row["From"]),
                Recipient = Utility.SafeString(row["Recipient"]),
                Subject = Utility.SafeString(row["Subject"]),
                ContentHtml = Utility.SafeString(row["ContentHtml"]),
                WasSent = Utility.SafeConvertToBool(row["WasSent"])
            };

            return item;
        }

    }
}
