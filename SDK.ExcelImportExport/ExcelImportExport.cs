using System;
using System.Data;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace Corkscrew.SDK.tools
{

    /// <summary>
    /// Performs Excel file export
    /// </summary>
    public sealed class ExcelImportExport
    {

        /// <summary>
        /// Exports the data from the given dataset to an Excel file at the given filepath.
        /// </summary>
        /// <param name="data">Dataset containing the data to be exported. All the tables in this dataset will be exported.</param>
        /// <param name="excelFilePath">Path to the file to be created (the Excel file)</param>
        /// <exception cref="Exception">General exception thrown</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static void ExportToExcel(DataSet data, string excelFilePath)
        {
            try
            {
                // step 1: create the file
                using (SpreadsheetDocument excelFile = SpreadsheetDocument.Create(excelFilePath, SpreadsheetDocumentType.Workbook))
                {  

                    // step 2: add a new workbook in the file
                    WorkbookPart wbPart = excelFile.AddWorkbookPart();
                    wbPart.Workbook = new Workbook();
                    wbPart.Workbook.Save();

                    wbPart.Workbook.Sheets = new Sheets();

                    foreach (DataTable tab in data.Tables)
                    {

                        // step 3: add a new worksheet for this table
                        SheetData sd = new SheetData();
                        WorksheetPart wsPart = wbPart.AddNewPart<WorksheetPart>();
                        wsPart.Worksheet = new Worksheet(sd);
                        wsPart.Worksheet.Save();


                        // step 4: find out the id of the worksheet we just added
                        string relId = wbPart.GetIdOfPart(wsPart);
                        uint shId = 1;

                        if (wbPart.Workbook.Sheets.Elements<Sheet>().Count() > 0)
                        {
                            shId = wbPart.Workbook.Sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }

                        // step 5: create a new sheet in that worksheet
                        
                        // we don't like empty table names, so if we dont have one in the dataset, create as "Table N"
                        string shName = Utility.SafeString(tab.TableName, "Table " + shId.ToString());

                        Sheet thisSheet = new Sheet()
                        {
                            Id = relId,                 // internal sheet id
                            SheetId = shId,             // numeric sheet number
                            Name = shName               // name of sheet == table name
                        };

                        wbPart.Workbook.Sheets.Append(thisSheet);
                        wbPart.Workbook.Save();


                        // step 6: write column headers
                        Row hr = new Row();
                        foreach (DataColumn dc in tab.Columns)
                        {
                            Cell sc = new Cell();
                            sc.DataType = CellValues.InlineString;
                            InlineString value = new InlineString();
                            Text valueText = new Text();
                            valueText.Text = dc.ColumnName.ToString();

                            value.AppendChild(valueText);   // append value text as child node to inline string
                            sc.AppendChild(value);          // append inline string as child node to cell
                            hr.Append(sc);                  // append cell as child node to row

                        }
                        sd.Append(hr);


                        // step 7: write data
                        foreach (DataRow dr in tab.Rows)
                        {
                            Row sr = new Row();
                            foreach (DataColumn dc in tab.Columns)
                            {
                                Cell sc = new Cell();

                                sc.DataType = CellValues.InlineString;
                                InlineString value = new InlineString();
                                Text valueText = new Text();
                                if (dr[dc.ColumnName] != null)
                                {
                                    valueText.Text = dr[dc.ColumnName].ToString();
                                }
                                else
                                {
                                    valueText.Text = "";    // this is necessary to maintain cell integrity
                                }

                                value.AppendChild(valueText);   // append value text as child node to inline string
                                sc.AppendChild(value);          // append inline string as child node to cell
                                sr.Append(sc);                  // append cell as child node to row
                            }
                            sd.Append(sr);
                        }
                    }

                    // step 8: commit changes
                    excelFile.WorkbookPart.Workbook.Save();
                    excelFile.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Reads in data from the given excel file and sheet
        /// </summary>
        /// <param name="excelFilePath">Path to Excel file</param>
        /// <param name="sheetName">Name of the sheet to read. If NULL, all the sheets will be processed.</param>
        /// <returns>Dataset containing data from the Excel sheet</returns>
        /// <exception cref="Exception">General exception thrown</exception>
        public static DataSet ImportFromExcel(string excelFilePath, string sheetName = null)
        {
            try
            {
                DataSet excelDataset = new DataSet();
                Dictionary<string, string> Headers = new Dictionary<string, string>();
                Stream fileStream = null;
                SpreadsheetDocument excelSpreadSheetDoc = null;

                try
                {
                    fileStream = File.Open(excelFilePath, FileMode.Open);
                    excelSpreadSheetDoc = SpreadsheetDocument.Open(fileStream, false);

                    IEnumerable<Sheet> sheets = null;
                    if (string.IsNullOrEmpty(sheetName))
                    {
                        sheets = excelSpreadSheetDoc.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName);
                    }
                    else
                    {
                        sheets = excelSpreadSheetDoc.WorkbookPart.Workbook.Descendants<Sheet>();
                    }

                    string labelName = null;

                    foreach (Sheet processingSheet in sheets)
                    {
                        DataTable sheetTable = new DataTable(processingSheet.Name.Value);

                        WorksheetPart worksheetPart = (WorksheetPart)excelSpreadSheetDoc.WorkbookPart.GetPartById(sheets.First().Id);
                        Worksheet worksheet = worksheetPart.Worksheet;
                        Row HeaderRow = worksheet.Descendants<Row>().First<Row>();

                        SharedStringTablePart shareStringPart = excelSpreadSheetDoc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        SharedStringItem[] items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                        foreach (Cell Headercell in HeaderRow)
                        {
                            if (Headercell.DataType != null && Headercell.DataType.Value == CellValues.SharedString)
                            {
                                SharedStringItem sharedsi = items.ElementAt(int.Parse(Headercell.CellValue.Text));
                                labelName = sharedsi.InnerText;
                                string columnReferenceName = GetColumnName(Headercell.CellReference.Value);

                                if (!string.IsNullOrEmpty(labelName))
                                {
                                    labelName = labelName.Trim().Replace(" ", "");

                                    if (!Headers.ContainsKey(columnReferenceName))
                                    {
                                        Headers.Add(columnReferenceName, labelName);
                                        sheetTable.Columns.Add(labelName);
                                    }
                                }
                            }
                        }

                        foreach (Row row in worksheet.Descendants<Row>())
                        {
                            if (row.RowIndex == 1)
                            {
                                continue;
                            }

                            DataRow sheetTableRow = sheetTable.NewRow();
                            foreach (Cell cell in row)
                            {
                                labelName = GetColumnName(cell.CellReference.Value);

                                if (Headers.ContainsKey(labelName) && (cell.CellValue != null))
                                {
                                    if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                                    {
                                        SharedStringItem item = items.ElementAt(int.Parse(cell.CellValue.Text));
                                        sheetTableRow[labelName] = item.InnerText;
                                    }
                                    else
                                    {
                                        sheetTableRow[labelName] = cell.CellValue.Text;
                                    }

                                    sheetTable.Rows.Add(sheetTableRow);
                                }
                            }
                        }

                        excelDataset.Tables.Add(sheetTable);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (excelSpreadSheetDoc != null)
                    {
                        excelSpreadSheetDoc.Close();
                    }
                }

                return excelDataset;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Private helper methods

        // gets the row index of the given cell (A1 -> 1)
        private static uint GetRowIndex(string cellName)
        {
            // Create a regular expression to match the row index portion the cell name.
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }

        // gets the column name of the given cell (A1 -> A)
        private static string GetColumnName(string cellName)
        {
            // Create a regular expression to match the column name portion of the cell name.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }

        #endregion

    }
}
