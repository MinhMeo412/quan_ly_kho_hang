using OfficeOpenXml;
using System.Data;

namespace WarehouseManager.Core.Utility
{
    public static class Excel
    {
        public static void Export(string filePath, DataTable table1, string sheetName1, DataTable table2, string sheetName2)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet1 = package.Workbook.Worksheets.Add(sheetName1);
                worksheet1.Cells["A1"].LoadFromDataTable(table1, true);

                var worksheet2 = package.Workbook.Worksheets.Add(sheetName2);
                worksheet2.Cells["A1"].LoadFromDataTable(table2, true);

                package.SaveAs(new FileInfo(filePath));
            }
        }

        public static string GetExcelFileName(string fileName)
        {
            if (!fileName.EndsWith(".xlsx"))
            {
                fileName = $"{fileName}.xlsx";
            }
            return fileName;
        }

        public static DataTable ImportFromExcel(string filePath)
        {
            // Ensure ExcelPackage uses non-commercial license
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new DataTable
            DataTable dataTable = new DataTable();

            // Load the Excel file into a memory stream
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Get the first worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Add columns to the DataTable
                foreach (var header in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                {
                    dataTable.Columns.Add(header.Text);
                }

                // Add rows to the DataTable
                for (int rowNum = 2; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                {
                    var row = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                    DataRow newRow = dataTable.NewRow();
                    foreach (var cell in row)
                    {
                        newRow[cell.Start.Column - 1] = cell.Text;
                    }
                    dataTable.Rows.Add(newRow);
                }
            }

            return dataTable;
        }

    }
}