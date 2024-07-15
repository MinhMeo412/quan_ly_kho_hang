using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class WarehouseStockReportLogic
    {
        public static Dictionary<string, int?> GetWarehousesNames()
        {
            Dictionary<string, int?> warehouseNameID = new Dictionary<string, int?>();
            warehouseNameID["All warehouses"] = null;

            List<Warehouse> warehouses = GetWarehouseList();
            foreach (Warehouse warehouse in warehouses)
            {
                warehouseNameID[warehouse.WarehouseName] = warehouse.WarehouseID;
            }

            return warehouseNameID;
        }

        public static DataTable GetWarehouseStock(KeyValuePair<string, int?> warehouseNameID)
        {
            List<Warehouse> warehouses = GetSelectedWarehouses(warehouseNameID.Value);
            List<WarehouseStock> warehouseStocks = GetSelectedWarehousesStock(warehouses);
            List<ProductVariant> productVariants = GetRelevantProductVariants(warehouseStocks);
            List<Product> products = GetRelevantProducts(productVariants);

            List<(string, string, string, Dictionary<int, int>)> warehouseStockProductVariants = new List<(string, string, string, Dictionary<int, int>)>();
            foreach (ProductVariant productVariant in productVariants)
            {
                string id = $"P{productVariant.ProductID}-V{productVariant.ProductVariantID}";
                string productName = $"{GetProductName(productVariant.ProductID, products)} {productVariant.ProductVariantColor} {productVariant.ProductVariantSize}";
                string category = GetProductCategory(productVariant.ProductID, products);

                Dictionary<int, int> warehouseVariantQuantity = warehouses.Select(w => w.WarehouseID).ToList()
                    .ToDictionary(
                        selectedWarehouseID => selectedWarehouseID,
                        selectedWarehouseID => warehouseStocks
                            .Where(ws => ws.WarehouseID == selectedWarehouseID && ws.ProductVariantID == productVariant.ProductVariantID)
                            .Select(ws => ws.WarehouseStockQuantity)
                            .FirstOrDefault()
                    );
                warehouseStockProductVariants.Add((id, productName, category, warehouseVariantQuantity));
            }

            var dataTable = WarehouseStockLogic.ConvertWarehouseStockProductVariantsToDataTable(warehouseStockProductVariants);
            dataTable.Columns[2].ColumnName = "Category";

            return dataTable;
        }

        public static DataTable GetFileInformation(KeyValuePair<string, int?> warehouse)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Warehouse Stock Report", typeof(string));

            dataTable.Rows.Add($"Warehouse: {warehouse.Key}");
            dataTable.Rows.Add($"Date: {GetCurrentTime()}");

            return dataTable;
        }

        private static List<Warehouse> GetWarehouseList()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            return warehouses;
        }

        private static List<Warehouse> GetSelectedWarehouses(int? warehouseID = null)
        {
            List<Warehouse> warehouses = GetWarehouseList();
            if (warehouseID == null)
            {
                return warehouses;
            }

            Warehouse? warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == warehouseID);
            if (warehouse == null)
            {
                throw new Exception("Warehouse does not exists.");
            }

            List<Warehouse> warehouses1 = new List<Warehouse> { warehouse };
            return warehouses1;
        }

        private static List<WarehouseStock> GetSelectedWarehousesStock(List<Warehouse> warehouses)
        {
            List<int> selectedWarehousesID = warehouses.Select(w => w.WarehouseID).ToList();
            return WarehouseStockLogic.GetSelectedWarehousesStocks(selectedWarehousesID);
        }

        private static List<ProductVariant> GetRelevantProductVariants(List<WarehouseStock> warehouseStocks)
        {
            return WarehouseStockLogic.GetRelevantProductVariants(warehouseStocks);
        }

        private static List<Product> GetRelevantProducts(List<ProductVariant> productVariants)
        {
            return WarehouseStockLogic.GetRelevantProducts(productVariants);
        }

        private static string GetProductName(int productID, List<Product>? relevantProducts = null)
        {
            return WarehouseStockLogic.GetProductName(productID, relevantProducts);
        }

        private static string GetProductCategory(int productID, List<Product>? relevantProducts = null)
        {
            int categoryID = GetCategoryID(productID, relevantProducts);
            return GetCategoryName(categoryID);
        }

        private static int GetCategoryID(int productID, List<Product>? relevantProducts = null)
        {
            relevantProducts ??= Program.Warehouse.ProductTable.Products ?? new List<Product>();
            int categoryID = relevantProducts.FirstOrDefault(p => p.ProductID == productID)?.CategoryID ?? 0;
            return categoryID;
        }

        private static string GetCategoryName(int categoryID, List<Category>? relevantCategories = null)
        {
            relevantCategories ??= Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            string categoryName = relevantCategories.FirstOrDefault(p => p.CategoryID == categoryID)?.CategoryName ?? "";
            return categoryName;
        }

        internal static string GetCurrentTime()
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("dd/MM/yyyy HH:mm:ss");
            return formattedDate;
        }
    }
}