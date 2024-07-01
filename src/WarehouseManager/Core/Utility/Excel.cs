using OfficeOpenXml;
using System.Data;

namespace WarehouseManager.Core.Utility
{
    public static class Excel
    {
        public static void Export(DataTable table, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
                worksheet.Cells["A1"].LoadFromDataTable(table, true);
                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
}