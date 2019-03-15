using Corkscrew.SDK.objects;
using Corkscrew.SDK.providers.database;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.odm
{
    internal class OdmTabularData : OdmBase
    {

        private CSSite _site = null;
        private OdmUsers _odmUsers = null;

        public OdmTabularData(CSSite site) : base()
        {
            _site = site;
            _odmUsers = new OdmUsers();
        }

        public bool SaveDefinition(CSTable table)
        {
            return base.CommitChanges(
                "CreateTableDef",
                new Dictionary<string, object>()
                {
                    { "Id", table.Id.ToString("d") },
                    { "SiteId", table.Site.Id.ToString("d") },
                    { "Name", table.Name },
                    { "UniqueName", table.InternalName },
                    { "FriendlyName", table.FriendlyName },
                    { "Description", table.Description },
                    { "Modified", table.Modified },
                    { "ModifiedBy", table.ModifiedBy.Id.ToString("d") }
                }
            );
        }

        public bool SaveDefinition(CSTableColumnDefinition column)
        {
            return base.CommitChanges(
                "CreateColDef",
                new Dictionary<string, object>()
                {
                    { "Id", column.Id.ToString("d") },
                    { "SiteId", column.Site.Id.ToString("d") },
                    { "Name", column.Name },
                    { "Type", CSTabularDataColumnDataTypeConverter.GetDatabaseTypeName(column.DataType) },
                    { "AllowNull", column.Nullable },
                    { "MaxLength", column.MaximumLength },
                    { "Created", DateTime.Now },
                    { "CreatedBy", _site.AuthenticatedUser.Id.ToString("d") }
                }
            );
        }

        public bool Drop(CSTable table)
        {
            return base.CommitChanges
            (
                "DropTableDef",
                new Dictionary<string, object>()
                {
                    { "Id", table.Id.ToString("d") }
                }
            );
        }

        public bool Drop(CSTableColumnDefinition column)
        {
            return base.CommitChanges
            (
                "DropColDef",
                new Dictionary<string, object>()
                {
                    { "Id", column.Id.ToString("d") }
                }
            );
        }

        public List<CSTable> GetAllTables()
        {
            List<CSTable> list = new List<CSTable>();

            DataSet ds = base.GetData
            (
                "GetTableDefsAll",
                new Dictionary<string, object>()
                {
                    { "SiteId", _site.Id.ToString("d") }
                }
            );

            if (HasData(ds))
            {
                OdmUsers _odmUser = new OdmUsers();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateTable(row));
                }
            }

            return list;
        }

        public List<CSTableColumnDefinition> GetAllColumns()
        {
            List<CSTableColumnDefinition> list = new List<CSTableColumnDefinition>();

            DataSet ds = base.GetData
            (
                "GetColDefsAll"
            );

            if (HasData(ds))
            {
                OdmUsers _odmUser = new OdmUsers();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateColumnDefinition(row));
                }
            }

            return list;
        }

        public List<CSTableColumn> GetColumnsAddedToTable(CSTable table)
        {
            List<CSTableColumn> list = new List<CSTableColumn>();

            DataSet ds = base.GetData
            (
                "GetColumnsAddedToTable",
                new Dictionary<string, object>()
                {
                    { "TableId", table.Id.ToString("d") }
                }
            );

            if (HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateTableColumn(table, row));
                }
            }

            return list;
        }

        public bool DropColumnFromTable(CSTableColumn tableColumn)
        {
            return base.CommitChanges(
                "RemoveColumnFromTableDef",
                new Dictionary<string, object>()
                {
                    { "TableDefId", tableColumn.Table.Id.ToString("d") },
                    { "ColDefId", tableColumn.Column.Id.ToString("d") },
                    { "LocalName", tableColumn.LocalName }
                }
            );
        }

        public bool AddColumnToTable(CSTableColumn column)
        {
            return base.CommitChanges
            (
                "AddColumnToTableDef",
                new Dictionary<string, object>()
                {
                    { "Id", column.Id.ToString("d") },
                    { "ColDefId", column.Column.Id.ToString("d") },
                    { "TableDefId", column.Table.Id.ToString("d") },
                    { "LocalName", column.LocalName },
                    { "Created", column.Created },
                    { "CreatedBy", column.CreatedBy.Id.ToString("d") }
                }
            );
        }

        public CSTableColumnDefinition GetColumnDefinition(Guid id)
        {
            DataSet ds = base.GetData
            (
                "GetColDefById",
                new Dictionary<string, object>()
                {
                    { "Id", id.ToString("d") }
                }
            );

            if (!HasData(ds))
            {
                return null;
            }

            return PopulateColumnDefinition(ds.Tables[0].Rows[0]);
        }

        public bool InsertTableRow(CSTableRow row)
        {
            bool result = false;
            string rowSaveQuery = string.Empty, columnsList = string.Empty, valuesList = string.Empty;

            rowSaveQuery = "INSERT INTO [" + row.Table.InternalName + "] (";

            columnsList = "[Corkscrew_RowId]";
            valuesList = "'" + row.Id.ToString("d") + "'";

            foreach (CSTableColumn col in row.Columns)
            {
                if (!string.IsNullOrEmpty(columnsList))
                {
                    columnsList += ", ";
                }

                if (!string.IsNullOrEmpty(valuesList))
                {
                    valuesList += ", ";
                }

                columnsList += "[" + col.LocalName + "]";

                object value = row[col];

                if (value == null)
                {
                    if (!col.Column.Nullable)
                    {
                        row.HasError = true;
                        row.RowError = "Column " + col.LocalName + " is defined as Non-Nullable, but does not have a value.";

                        break;
                    }

                    valuesList += "NULL";
                }
                else
                {
                    switch (col.Column.DataType)
                    {
                        case CSTableColumnDataTypeEnum.Binary:
                            valuesList += "0x" + BitConverter.ToString((byte[])value).Replace("-", "").ToLower();
                            break;

                        case CSTableColumnDataTypeEnum.Boolean:
                            switch ((bool)value)
                            {
                                case true:
                                    valuesList += "1";
                                    break;

                                case false:
                                    valuesList += "0";
                                    break;
                            }
                            break;

                        case CSTableColumnDataTypeEnum.DateTime:
                            valuesList += "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                            break;

                        case CSTableColumnDataTypeEnum.FloatingPoint:
                            valuesList += ((double)value).ToString().Trim();
                            break;

                        case CSTableColumnDataTypeEnum.Integer:
                            valuesList += ((long)value).ToString().Trim();
                            break;

                        case CSTableColumnDataTypeEnum.String:
                        case CSTableColumnDataTypeEnum.Text:
                            valuesList += "'" + ((string)value) + "'";
                            break;

                        case CSTableColumnDataTypeEnum.Guid:
                            valuesList += "'" + ((Guid)value).ToString("d") + "'";
                            break;
                    }
                }
            }

            if (row.HasError)
            {
                return false;
            }

            rowSaveQuery += columnsList + ") VALUES (" + valuesList + ");";

            DatabaseActionResult dbResult = base.DataProvider.ExecuteNonQueryStatement(rowSaveQuery);
            if (!dbResult.Error)
            {
                result = true;
            }

            return result;

            // rowid != -1. We did not insert
            return false;
        }

        public bool UpdateTableRow(CSTableRow row)
        {
            bool result = false;
            string rowSaveQuery = string.Empty, valuesList = string.Empty;

            rowSaveQuery = "UPDATE [" + row.Table.InternalName + "] SET ";

            foreach (CSTableColumn col in row.Columns)
            {
                if (!string.IsNullOrEmpty(valuesList))
                {
                    valuesList += ", ";
                }

                valuesList += "[" + col.LocalName + "]=";

                object value = row[col];

                if (value == null)
                {
                    if (!col.Column.Nullable)
                    {
                        row.HasError = true;
                        row.RowError = "Column " + col.LocalName + " is defined as Non-Nullable, but does not have a value.";

                        break;
                    }

                    valuesList += "NULL";
                }
                else
                {
                    switch (col.Column.DataType)
                    {
                        case CSTableColumnDataTypeEnum.Binary:
                            valuesList += "0x" + BitConverter.ToString((byte[])value).Replace("-", "").ToLower();
                            break;

                        case CSTableColumnDataTypeEnum.Boolean:
                            switch ((bool)value)
                            {
                                case true:
                                    valuesList += "1";
                                    break;

                                case false:
                                    valuesList += "0";
                                    break;
                            }
                            break;

                        case CSTableColumnDataTypeEnum.DateTime:
                            valuesList += "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                            break;

                        case CSTableColumnDataTypeEnum.FloatingPoint:
                            valuesList += ((double)value).ToString().Trim();
                            break;

                        case CSTableColumnDataTypeEnum.Integer:
                            valuesList += ((long)value).ToString().Trim();
                            break;

                        case CSTableColumnDataTypeEnum.String:
                        case CSTableColumnDataTypeEnum.Text:
                            valuesList += "'" + ((string)value) + "'";
                            break;

                        case CSTableColumnDataTypeEnum.Guid:
                            valuesList += "'" + ((Guid)value).ToString("d") + "'";
                            break;
                    }
                }
            }

            if (row.HasError)
            {
                return false;
            }

            rowSaveQuery += valuesList + " WHERE ([Corkscrew_RowId]='" + row.Id.ToString("d") + "');";

            DatabaseActionResult dbResult = base.DataProvider.ExecuteNonQueryStatement(rowSaveQuery);
            if (!dbResult.Error)
            {
                result = true;
            }

            return result;

            // rowId was not > 0. We did not update
            return false;
        }

        public bool DeleteTableRow(CSTableRow row)
        {
            DatabaseActionResult result = base.DataProvider.ExecuteNonQueryStatement(
                string.Format("DELETE FROM [{0}]  WHERE ([Corkscrew_RowId]='{1}');", row.Table.Name, row.Id.ToString("d"))
            );

            return (!result.Error);
        }

        public List<CSTableRow> GetTableRows(CSTable table)
        {
            List<CSTableRow> list = new List<CSTableRow>();

            DataSet ds = base.GetData
            (
                "GetTableData",
                new Dictionary<string, object>()
                {
                    { "Id", table.Id.ToString("d") }
                }
            );

            if (HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateTableRow(table, row));
                }
            }

            return list;
        }

        private CSTable PopulateTable(DataRow row)
        {
            Guid tableSiteId = Utility.SafeConvertToGuid(row["SiteId"]);
            if (tableSiteId != _site.Id)
            {
                throw new Exception("Returned table does not belong to context site");
            }

            return new CSTable(_site)
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                Name = Utility.SafeString(row["Name"]),
                InternalName = Utility.SafeString(row["UniqueName"]),
                FriendlyName = Utility.SafeString(row["FriendlyName"]),
                Description = Utility.SafeString(row["Description"]),
                Created = Utility.SafeConvertToDateTime(row["Created"]),
                CreatedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["CreatedBy"])),
                Modified = Utility.SafeConvertToDateTime(row["Modified"]),
                ModifiedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["ModifiedBy"]))
            };
        }

        private CSTableColumnDefinition PopulateColumnDefinition(DataRow row)
        {
            Guid columnSiteId = Utility.SafeConvertToGuid(row["SiteId"]);
            if (columnSiteId != _site.Id)
            {
                throw new Exception("Returned column definition does not belong to context site");
            }

            return new CSTableColumnDefinition(_site)
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                Name = Utility.SafeString(row["Name"]),
                DataType = CSTabularDataColumnDataTypeConverter.GetCSTypeName(Utility.SafeString(row["Type"])),
                Nullable = Utility.SafeConvertToBool(row["Nullable"]),
                MaximumLength = Utility.SafeConvertToInt(row["MaxLength"]),
                Created = Utility.SafeConvertToDateTime(row["Created"]),
                CreatedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["CreatedBy"]))
            };
        }

        private CSTableColumn PopulateTableColumn(CSTable table, DataRow row)
        {

            Guid columnTableId = Utility.SafeConvertToGuid(row["TableId"]);
            if (columnTableId != table.Id)
            {
                throw new Exception("Returned column does not belong to context table");
            }

            return new CSTableColumn(table, GetColumnDefinition(Utility.SafeConvertToGuid(row["ColDefId"])))
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                LocalName = Utility.SafeString(row["LocalName"]),
                Created = Utility.SafeConvertToDateTime(row["Created"]),
                CreatedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["CreatedBy"]))
            };
        }

        private CSTableRow PopulateTableRow(CSTable table, DataRow row)
        {
            CSTableRow tableRow = new CSTableRow(table)
            {
                Id = Utility.SafeConvertToGuid(row["Corkscrew_RowId"])
            };

            foreach (CSTableColumn column in table.Columns)
            {
                // the this[] accessor has checks for permissions/readonly state. But that will also cause 
                // failure during Odm load. This method bypasses that check by talking to _storage directly. 
                tableRow.SetRowColumnValueInternal(column, row[column.LocalName]);
            }

            tableRow.RowState = CSTableItemStateEnum.UnchangedOrCommitted;

            return tableRow;
        }


    }
}
