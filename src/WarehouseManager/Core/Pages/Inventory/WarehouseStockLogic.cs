using System.Data;
using WarehouseManager.Data.Entity;
using Terminal.Gui;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core.Pages
{
    public static class WarehouseStockLogic
    {
        public static Dictionary<CheckBox, int> GetWarehouseChecklistDict()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            Dictionary<CheckBox, int> warehouseChecklistDict = new Dictionary<CheckBox, int>();

            foreach (Warehouse warehouse in warehouses)
            {
                warehouseChecklistDict[new CheckBox($"{warehouse.WarehouseName}")] = warehouse.WarehouseID;
            }

            return warehouseChecklistDict;
        }

        public static DataTable GetData(Dictionary<CheckBox, int> warehouseChecklistDict)
        {
            List<int> selectedWarehouseIDs = GetSelectedWarehouseIDs(warehouseChecklistDict);
            List<WarehouseStock> selectedWarehouseStocks = GetSelectedWarehousesStocks(selectedWarehouseIDs);
            List<ProductVariant> relevantProductVariants = GetRelevantProductVariants(selectedWarehouseStocks);
            List<Product> relevantProducts = GetRelevantProducts(relevantProductVariants);

            List<(string, string, string, Dictionary<int, int>)> warehouseStockProductVariants = new List<(string, string, string, Dictionary<int, int>)>();

            foreach (ProductVariant relevantProductVariant in relevantProductVariants)
            {
                string variantID = $"P{relevantProductVariant.ProductID}-V{relevantProductVariant.ProductVariantID}";
                string name = $"{GetProductName(relevantProductVariant.ProductID, relevantProducts)} {relevantProductVariant.ProductVariantColor} {relevantProductVariant.ProductVariantSize}";
                string imageURL = $"{relevantProductVariant.ProductVariantImageURL}";
                Dictionary<int, int> warehouseVariantQuantity = selectedWarehouseIDs
                    .ToDictionary(
                        selectedWarehouseID => selectedWarehouseID,
                        selectedWarehouseID => selectedWarehouseStocks
                            .Where(ws => ws.WarehouseID == selectedWarehouseID && ws.ProductVariantID == relevantProductVariant.ProductVariantID)
                            .Select(ws => ws.WarehouseStockQuantity)
                            .FirstOrDefault()
                    );
                warehouseStockProductVariants.Add((variantID, name, imageURL, warehouseVariantQuantity));
            }

            return ConvertWarehouseStockProductVariantsToDataTable(warehouseStockProductVariants);
        }

        private static List<int> GetSelectedWarehouseIDs(Dictionary<CheckBox, int> warehouseChecklistDict)
        {
            List<int> selectedWarehousesID = warehouseChecklistDict
                .Where(warehouseCheckboxPair => warehouseCheckboxPair.Key.Checked)
                .Select(warehouseCheckboxPair => warehouseCheckboxPair.Value)
                .ToList();
            return selectedWarehousesID;
        }

        internal static List<WarehouseStock> GetSelectedWarehousesStocks(List<int> selectedWarehousesID)
        {
            List<WarehouseStock> warehouseStocks = Program.Warehouse.WarehouseStockTable.WarehouseStocks ?? new List<WarehouseStock>();

            List<WarehouseStock> relevantWarehousesStocks = warehouseStocks
                .Where(ws => selectedWarehousesID.Contains(ws.WarehouseID))
                .ToList();

            return relevantWarehousesStocks;
        }

        internal static List<ProductVariant> GetRelevantProductVariants(List<WarehouseStock> selectedWarehouseStocks)
        {
            // Get all product variants from the program's warehouse
            List<ProductVariant> allProductVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();

            // Use LINQ to get distinct product variant IDs associated with selected warehouse stocks
            List<int> relevantProductVariantIDs = selectedWarehouseStocks
                .Select(ws => ws.ProductVariantID)
                .Distinct()
                .ToList();

            // Use LINQ to filter allProductVariants based on relevantProductVariantIDs
            List<ProductVariant> relevantProductVariants = allProductVariants
                .Where(pv => relevantProductVariantIDs.Contains(pv.ProductVariantID))
                .ToList();

            return relevantProductVariants;
        }

        internal static List<Product> GetRelevantProducts(List<ProductVariant> relevantProductVariants)
        {
            // Get all products from the program's warehouse
            List<Product> allProducts = Program.Warehouse.ProductTable.Products ?? new List<Product>();

            // Use LINQ to get distinct relevant product IDs from relevantProductVariants
            List<int> relevantProductIDs = relevantProductVariants.Select(rv => rv.ProductID).Distinct().ToList();

            // Use LINQ to filter allProducts based on relevantProductIDs
            List<Product> relevantProducts = allProducts.Where(p => relevantProductIDs.Contains(p.ProductID)).ToList();

            return relevantProducts;
        }

        internal static DataTable ConvertWarehouseStockProductVariantsToDataTable(List<(string, string, string, Dictionary<int, int>)> warehouseStockProductVariants)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("ID", typeof(string));
            dataTable.Columns.Add("Product", typeof(string));
            dataTable.Columns.Add("Image", typeof(string));
            if (warehouseStockProductVariants.Count > 0)
            {
                warehouseStockProductVariants[0].Item4
                    .ToList()
                    .ForEach(warehousesQuantity => dataTable.Columns.Add($"{GetWarehouseName(warehousesQuantity.Key)}", typeof(int)));
            }

            foreach ((string, string, string, Dictionary<int, int>) row in warehouseStockProductVariants)
            {
                List<object> values = new List<object> {
                    row.Item1,
                    row.Item2,
                    row.Item3
                };

                foreach (KeyValuePair<int, int> warehouseQuantities in row.Item4)
                {
                    values.Add(warehouseQuantities.Value);
                }

                dataTable.Rows.Add(values.ToArray());
            }

            return dataTable;
        }

        internal static string GetProductName(int productID, List<Product>? relevantProducts = null)
        {
            relevantProducts ??= Program.Warehouse.ProductTable.Products ?? new List<Product>();
            string productName = relevantProducts.FirstOrDefault(p => p.ProductID == productID)?.ProductName ?? "";
            return productName;
        }

        private static string GetWarehouseName(int warehouseID)
        {
            // to make it cleaner, do the same thing as the GetProductName method above.
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();

            Warehouse? warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == warehouseID);
            string warehouseName = "";
            if (warehouse != null)
            {
                warehouseName = warehouse.WarehouseName;
            }

            return warehouseName;
        }

        public static DataTable SortWarehouseStockBySearchTerm(DataTable dataTable, string searchTerm)
        {
            return SortDataTable.BySearchTerm(dataTable, searchTerm);
        }

        public static DataTable SortWarehouseStockByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            return SortDataTable.ByColumn(dataTable, columnToSortBy, sortColumnInDescendingOrder);
        }
    }
}
