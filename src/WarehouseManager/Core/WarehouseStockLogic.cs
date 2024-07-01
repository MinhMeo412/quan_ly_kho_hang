using System.Data;
using WarehouseManager.Data.Entity;
using Terminal.Gui;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core
{
    public static class WarehouseStockLogic
    {
        // Phải lưu luôn ra ngoài nếu không sort và search của menu này sẽ cực kì chậm.
        // (do phải tìm lại tên và id warehouse trong danh sách dài cực kì nhiều lần.)
        private static List<Warehouse> RelevantWarehouses { get; set; } = new List<Warehouse>();

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

            List<(int, string, string, Dictionary<int, int>)> warehouseStockProductVariants = new List<(int, string, string, Dictionary<int, int>)>();

            foreach (ProductVariant relevantProductVariant in relevantProductVariants)
            {
                int variantID = relevantProductVariant.ProductVariantID;
                string name = $"{GetProductName(relevantProductVariant.ProductID, relevantProducts)} {relevantProductVariant.ProductVariantColor} {relevantProductVariant.ProductVariantSize}";
                string imageURL = $"{relevantProductVariant.ProductVariantImageURL}";
                Dictionary<int, int> warehouseVariantQuantity = selectedWarehouseIDs
                    .ToDictionary(
                        selectedWarehouseID => selectedWarehouseID,
                        selectedWarehouseID => selectedWarehouseStocks
                            .Where(ws => ws.WarehouseID == selectedWarehouseID && ws.ProductVariantID == variantID)
                            .Select(ws => ws.WarehouseStockQuantity)
                            .FirstOrDefault()
                    );

                warehouseStockProductVariants.Add((variantID, name, imageURL, warehouseVariantQuantity));
            }

            RelevantWarehouses = GetRelevantWarehouses(selectedWarehouseIDs);

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

        private static List<WarehouseStock> GetSelectedWarehousesStocks(List<int> selectedWarehousesID)
        {
            List<WarehouseStock> warehouseStocks = Program.Warehouse.WarehouseStockTable.WarehouseStocks ?? new List<WarehouseStock>();

            List<WarehouseStock> relevantWarehousesStocks = warehouseStocks
                .Where(ws => selectedWarehousesID.Contains(ws.WarehouseID))
                .ToList();

            return relevantWarehousesStocks;
        }

        private static List<ProductVariant> GetRelevantProductVariants(List<WarehouseStock> selectedWarehouseStocks)
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

        private static List<Product> GetRelevantProducts(List<ProductVariant> relevantProductVariants)
        {
            // Get all products from the program's warehouse
            List<Product> allProducts = Program.Warehouse.ProductTable.Products ?? new List<Product>();

            // Use LINQ to get distinct relevant product IDs from relevantProductVariants
            List<int> relevantProductIDs = relevantProductVariants.Select(rv => rv.ProductID).Distinct().ToList();

            // Use LINQ to filter allProducts based on relevantProductIDs
            List<Product> relevantProducts = allProducts.Where(p => relevantProductIDs.Contains(p.ProductID)).ToList();

            return relevantProducts;
        }

        private static List<Warehouse> GetRelevantWarehouses(List<int> selectedWarehouseIDs)
        {
            // Assuming Program.Warehouse contains the list of all warehouses
            List<Warehouse> allWarehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();

            // Filter warehouses based on selected IDs
            List<Warehouse> relevantWarehouses = allWarehouses
                .Where(warehouse => selectedWarehouseIDs.Contains(warehouse.WarehouseID))
                .ToList();

            return relevantWarehouses;
        }

        private static DataTable ConvertWarehouseStockProductVariantsToDataTable(List<(int, string, string, Dictionary<int, int>)> warehouseStockProductVariants)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Product", typeof(string));
            dataTable.Columns.Add("Image", typeof(string));
            if (warehouseStockProductVariants.Count > 0)
            {
                warehouseStockProductVariants[0].Item4
                    .ToList()
                    .ForEach(warehousesQuantity => dataTable.Columns.Add($"{GetWarehouseName(warehousesQuantity.Key)}", typeof(int)));
            }

            foreach ((int, string, string, Dictionary<int, int>) row in warehouseStockProductVariants)
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

        private static string GetProductName(int productID, List<Product>? relevantProducts = null)
        {
            relevantProducts ??= Program.Warehouse.ProductTable.Products ?? new List<Product>();
            string productName = relevantProducts.FirstOrDefault(p => p.ProductID == productID)?.ProductName ?? "";
            return productName;
        }

        private static string GetWarehouseName(int warehouseID)
        {
            // to make it cleaner, do the same thing as the GetProductName method above.
            List<Warehouse> relevantWarehouses = new List<Warehouse>(RelevantWarehouses);

            Warehouse? warehouse = relevantWarehouses.FirstOrDefault(w => w.WarehouseID == warehouseID);
            string warehouseName = "";
            if (warehouse != null)
            {
                warehouseName = warehouse.WarehouseName;
            }

            return warehouseName;
        }

        private static List<(int, string, string, Dictionary<int, int>)> ConvertDataTableToWarehouseStockProductVariants(DataTable dataTable)
        {
            // this method gets the warehouse id by using the warehouse name
            // if the arrows is still present it will break the program
            // this is a very hacky way of preventing that. it removes the up/down sorting arrows from datatable column names:
            for (int i = 3; i < dataTable.Columns.Count; i++)
            {
                string upwardsArrow = "\u25B2";
                string downwardsArrow = "\u25BC";

                if (dataTable.Columns[i].ColumnName.Contains(upwardsArrow) || dataTable.Columns[i].ColumnName.Contains(downwardsArrow))
                {
                    dataTable.Columns[i].ColumnName = dataTable.Columns[i].ColumnName.Replace(upwardsArrow, "");
                    dataTable.Columns[i].ColumnName = dataTable.Columns[i].ColumnName.Replace(downwardsArrow, "");
                    dataTable.Columns[i].ColumnName = dataTable.Columns[i].ColumnName.Substring(0, dataTable.Columns[i].ColumnName.Length - 1);
                }
            }

            List<(int, string, string, Dictionary<int, int>)> warehouseStockProductVariants = new List<(int, string, string, Dictionary<int, int>)>();

            foreach (DataRow row in dataTable.Rows)
            {
                int variantID = (int)row[0];
                string name = (string)row[1];
                string imageURL = (string)row[2];

                Dictionary<int, int> warehousesQuantity = new Dictionary<int, int>();
                for (int i = 3; i < dataTable.Columns.Count; i++)
                {
                    warehousesQuantity.Add(GetWarehouseID(dataTable.Columns[i].ColumnName), (int)row[i]);
                }

                warehouseStockProductVariants.Add((variantID, name, imageURL, warehousesQuantity));
            }

            return warehouseStockProductVariants;
        }

        private static int GetWarehouseID(string warehouseName)
        {
            List<Warehouse> warehouses = new List<Warehouse>(RelevantWarehouses);

            Warehouse? warehouse = warehouses.FirstOrDefault(w => w.WarehouseName == warehouseName);

            int warehouseID = 0;
            if (warehouse != null)
            {
                warehouseID = warehouse.WarehouseID;
            }
            return warehouseID;
        }

        public static DataTable SortWarehouseStockBySearchTerm(DataTable dataTable, string searchTerm)
        {
            List<(int, string, string, Dictionary<int, int>)> warehouseStockProductVariants = ConvertDataTableToWarehouseStockProductVariants(dataTable);
            List<((int, string, string, Dictionary<int, int>), double)> warehouseStockSimilarities = new List<((int, string, string, Dictionary<int, int>), double)>();


            foreach ((int, string, string, Dictionary<int, int>) warehouseStockProductVariant in warehouseStockProductVariants)
            {
                double maxSimilarity = Misc.MaxDouble(
                    Misc.JaccardSimilarity($"{warehouseStockProductVariant.Item1}", searchTerm),
                    Misc.JaccardSimilarity(warehouseStockProductVariant.Item2, searchTerm),
                    Misc.JaccardSimilarity($"{warehouseStockProductVariant.Item3}", searchTerm)
                );

                warehouseStockSimilarities.Add((warehouseStockProductVariant, maxSimilarity));
            }

            List<(int, string, string, Dictionary<int, int>)> sortedWarehouseStockProductVariants = warehouseStockSimilarities
                .OrderByDescending(w => w.Item2)
                .Select(w => w.Item1)
                .ToList();

            return ConvertWarehouseStockProductVariantsToDataTable(sortedWarehouseStockProductVariants);

        }

        public static DataTable SortWarehouseStockByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            List<(int, string, string, Dictionary<int, int>)> warehouseStockProductVariants = ConvertDataTableToWarehouseStockProductVariants(dataTable);

            switch (columnToSortBy)
            {
                case 0:
                    warehouseStockProductVariants = warehouseStockProductVariants.OrderBy(w => w.Item1).ToList();
                    break;
                case 1:
                    warehouseStockProductVariants = warehouseStockProductVariants.OrderBy(w => w.Item2).ToList();
                    break;
                case 2:
                    warehouseStockProductVariants = warehouseStockProductVariants.OrderBy(w => w.Item3).ToList();
                    break;
                default:
                    break;
            }

            if (sortColumnInDescendingOrder)
            {
                warehouseStockProductVariants.Reverse();
            }

            DataTable sortedDataTable = ConvertWarehouseStockProductVariantsToDataTable(warehouseStockProductVariants);

            sortedDataTable.Columns[columnToSortBy].ColumnName = Misc.ShowCurrentSortingDirection(sortedDataTable.Columns[columnToSortBy].ColumnName, sortColumnInDescendingOrder);

            return sortedDataTable;
        }
    }
}
