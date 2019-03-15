using System.Collections.Generic;
using System.Data;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;

namespace Corkscrew.SDK.odm
{

    /// <summary>
    /// Object/data manager for CSContentType
    /// </summary>
    internal class OdmMIMEType : OdmBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public OdmMIMEType() : base() { }

        /// <summary>
        /// Saves the given instance of CSMIMEType. 
        /// Check value of return to determine success.
        /// </summary>
        /// <param name="MIMEType">MIME type to save</param>
        /// <returns>True if save was successful. Typical reason for failure is if file extension is already mapped to another MIME type.</returns>
        public bool Save(CSMIMEType MIMEType)
        {
            return base.CommitChanges
            (
                "MIMETypeSave",
                new Dictionary<string, object>()
                {
                    { "@FileExtension", MIMEType.FileExtension },
                    { "@MimeType", ( (! string.IsNullOrEmpty(MIMEType.KnownMimeType)) ? MIMEType.KnownMimeType : CSMIMEType.DEFAULT_MIME_TYPE ) }
                }
            );
        }

        /// <summary>
        /// Get all registered MIME Types
        /// </summary>
        /// <returns>List[CSMIMEType] or empty list.</returns>
        public List<CSMIMEType> GetAll()
        {
            List<CSMIMEType> types = new List<CSMIMEType>();

            DataSet ds = base.GetData
                (
                    "MIMETypesGetAll"
                );

            if (base.HasData(ds))
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    types.Add(Populate(row));
                }
            }

            return types;
        }

        /// <summary>
        /// Gets registered MIME types by partial mimetype name. 
        /// This is useful for auto-complete ajax methods
        /// </summary>
        /// <param name="partialMimeName">Partial value of MIME Type name. Do not add the "%" at the end, this is done automatically at database end.</param>
        /// <returns>List[CSMIMEType] or empty list</returns>
        public List<CSMIMEType> GetByPartialMimeTypeName(string partialMimeName)
        {
            List<CSMIMEType> types = new List<CSMIMEType>();

            DataSet ds = base.GetData
                (
                    "MIMETypeMatchByPartialName",
                    new Dictionary<string, object>()
                    {
                        { "@PartialMimeType", partialMimeName }
                    }
                );

            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    types.Add(Populate(row));
                }
            }

            return types;
        }

        /// <summary>
        /// Gets a CSMIMEType map for the given file extension
        /// </summary>
        /// <param name="extension">File extension to get map for</param>
        /// <returns>CSMIMEType if found. Else NULL.</returns>
        public CSMIMEType GetByExtension(string extension)
        {
            DataSet ds = base.GetData
                (
                    "MIMETypeGetForExtension",
                    new Dictionary<string, object>()
                    {
                        { "@FileExtension", extension }
                    }
                );

            if (base.HasData(ds))
            {
                return Populate(ds.Tables[0].Rows[0]);
            }

            return null;
        }

        /// <summary>
        /// Deletes a CSMIMEType map with the given file extension
        /// </summary>
        /// <param name="extension">File extension to delete</param>
        public bool DeleteByExtension(string extension)
        {
            return base.CommitChanges
            (
                "MIMETypeDeleteByExtension",
                new Dictionary<string, object>()
                {
                    { "@FileExtension", extension }
                }
            );
        }

        private CSMIMEType Populate(DataRow row)
        {
            return new CSMIMEType
            (
                Utility.SafeString(row["FileExtension"]),
                Utility.SafeString(row["KnownMimeType"])
            );
        }

    }
}
