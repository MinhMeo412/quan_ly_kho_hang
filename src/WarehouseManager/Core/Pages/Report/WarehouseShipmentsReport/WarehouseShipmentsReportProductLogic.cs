using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class WarehouseShipmentsReportProductLogic
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

        public static Dictionary<int, string> GetProductList()
        {
            List<Product> products = GetProducts();
            Dictionary<int, string> productDictionary = new Dictionary<int, string>();

            foreach (var product in products)
            {
                productDictionary[product.ProductID] = product.ProductName;
            }

            return productDictionary;
        }

        public static DateTime GetDefaultStartDate()
        {
            return new DateTime(1, 1, 1);
        }

        public static DateTime GetDefaultEndDate()
        {
            return DateTime.Now;
        }

        public static DataTable GetProductExportData(string id, DateTime startDate, DateTime endDate, bool includeStockTransfers)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add($"Warehouse Shipments Report: Inbound", typeof(string));

            dataTable.Rows.Add($"Warehouse: {id}");
            dataTable.Rows.Add($"Date: {startDate} -> {endDate}, include stock transfers: {includeStockTransfers}");

            return dataTable;
        }

        public static DataTable GetProductFileInformation(string id, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add($"Warehouse Shipments Report: Inbound", typeof(string));

            dataTable.Rows.Add($"Warehouse: {id}");
            dataTable.Rows.Add($"Date: {startDate} -> {endDate}, include stock transfers:");

            return dataTable;
        }

        public static int GetProductID(string productName, Dictionary<int, string> productDictionary)
        {
            foreach (var kvp in productDictionary)
            {
                if (kvp.Value.Equals(productName, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Key;
                }
            }

            return -1;
        }

        public static int GetProductIndex(string productIDString, Dictionary<int, string> productDictionary)
        {
            int index = 0;
            int productID = int.TryParse(productIDString, out int parsedID) ? parsedID : -1;

            foreach (var kvp in productDictionary)
            {
                if (kvp.Key == productID)
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        private static List<Product> GetProducts()
        {
            List<Product> products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            return products;
        }
    }
}
