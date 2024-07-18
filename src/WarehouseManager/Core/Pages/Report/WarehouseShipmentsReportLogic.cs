using System.Data;
using System.Dynamic;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class WarehouseShipmentsReportLogic
    {
        public static List<string> GetReportOptions()
        {
            List<string> options = new List<string>{
                "Inbound",
                "Outbound",
                "Product"
            };
            return options;
        }

        public static List<string> GetWarehouseList()
        {
            return new List<string>();
        }

        public static List<string> GetProductList()
        {
            return new List<string>();
        }

        public static DateTime GetDefaultStartDate()
        {
            return new DateTime(1, 1, 1);
        }

        public static DateTime GetDefaultEndDate()
        {
            return DateTime.Now;
        }

        public static DataTable GetProductExportData(string id, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            return dataTable;
        }

        public static DataTable GetWarehouseExportData(string id, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            return dataTable;
        }

        public static DataTable GetWarehouseFileInformation(string id, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            return dataTable;
        }

        public static DataTable GetProductFileInformation(string id, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            return dataTable;
        }
    }
}