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
    }
}