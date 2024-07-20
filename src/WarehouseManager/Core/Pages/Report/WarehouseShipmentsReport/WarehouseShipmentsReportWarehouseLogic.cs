using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class WarehouseShipmentsReportWarehouseLogic
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
            List<Warehouse> warehouses = GetWarehouses();
            return warehouses.Select(w => w.WarehouseName).ToList();
        }

        public static DateTime GetDefaultStartDate()
        {
            return new DateTime(1, 1, 1);
        }

        public static DateTime GetDefaultEndDate()
        {
            return DateTime.Now;
        }

        public static DataTable GetWarehouseExportData(string option, string warehouseName, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            return dataTable;
        }

        public static DataTable GetWarehouseFileInformation(string option, string warehouseName, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            return dataTable;
        }

        private static List<Warehouse> GetWarehouses()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            return warehouses;
        }

    }
}
