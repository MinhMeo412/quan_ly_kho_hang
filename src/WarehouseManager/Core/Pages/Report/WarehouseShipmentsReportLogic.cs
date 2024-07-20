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
            List<Warehouse> warehouses = GetWarehouses();
            return warehouses.Select(w => w.WarehouseName).ToList();
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


        private static List<Warehouse> GetWarehouses()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            return warehouses;
        }

        private static List<Product> GetProducts()
        {
            List<Product> products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            return products;
        }
    }
}