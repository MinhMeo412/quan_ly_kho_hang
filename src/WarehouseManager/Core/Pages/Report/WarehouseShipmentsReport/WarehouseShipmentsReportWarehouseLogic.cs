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

        public static DataTable GetWarehouseExportData(string option, string warehouseName, DateTime startDate, DateTime endDate, bool includeStockTransfers)
        {
            DataTable dataTable;

            if (option == "Inbound")
            {
                dataTable = GetInboundWarehouseExportData(warehouseName, startDate, endDate, includeStockTransfers);
            }
            else
            {
                dataTable = GetOutboundWarehouseExportData(warehouseName, startDate, endDate, includeStockTransfers);
            }

            return dataTable;
        }

        public static DataTable GetWarehouseFileInformation(string option, string warehouseName, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add($"Warehouse Shipments Report: {option}", typeof(string));

            dataTable.Rows.Add($"Warehouse: {warehouseName}");
            dataTable.Rows.Add($"Date: {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}");


            return dataTable;
        }

        private static List<Warehouse> GetWarehouses()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            return warehouses;
        }

        private static DataTable GetInboundWarehouseExportData(string warehouseName, DateTime startDate, DateTime endDate, bool includeStockTransfers)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add($"Warehouse Shipments Report: Inbound", typeof(string));

            dataTable.Rows.Add($"Warehouse: {warehouseName}");
            dataTable.Rows.Add($"Date: {startDate} -> {endDate}");

            return dataTable;
        }

        private static DataTable GetOutboundWarehouseExportData(string warehouseName, DateTime startDate, DateTime endDate, bool includeStockTransfers)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add($"Warehouse Shipments Report: Outbound", typeof(string));

            dataTable.Rows.Add($"Warehouse: {warehouseName}");
            dataTable.Rows.Add($"Date: {startDate} -> {endDate}");

            return dataTable;
        }


    }
}
