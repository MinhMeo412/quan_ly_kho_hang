using System.Data;
using WarehouseManager.Data.Entity;
using Terminal.Gui;

namespace WarehouseManager.Core
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
            List<WarehouseStock> selectedWarehouseStocks = GetSelectedWarehousesStocks(warehouseChecklistDict);
            List<ProductVariant> relevantProductVariants = GetRelevantProductVariants(warehouseStocks);
            List<Product> roducts = GetRelevantProducts(productVariants);




            var dataTable = new DataTable();

            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Product", typeof(string));
            dataTable.Columns.Add("Image", typeof(string));

            foreach (KeyValuePair<CheckBox, int> warehouseChecklist in warehouseChecklistDict)
            {
                dataTable.Columns.Add($"{warehouseChecklist.Key.Text}", typeof(string));
            }


            return dataTable;
        }

        private static List<WarehouseStock> GetSelectedWarehousesStocks(Dictionary<CheckBox, int> warehouseChecklistDict)
        {
            List<WarehouseStock> warehouseStocks = Program.Warehouse.WarehouseStockTable.WarehouseStocks ?? new List<WarehouseStock>();

            List<int> selectedWarehousesID = warehouseChecklistDict.Values.ToList();

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
            List<Product> allProducts = Program.Warehouse.ProductTable.Products ?? new List<Product>();

            List<Product> relevantProducts = new List<Product>();

            foreach (ProductVariant relevantProductVariant in relevantProductVariants)
            {
                int productID = relevantProductVariant.ProductID;
                Product? product = allProducts.FirstOrDefault(p => p.ProductID == productID);

                if (product != null && !relevantProducts.Contains(product))
                {
                    relevantProducts.Add(product);
                }
            }

            return relevantProducts;
        }
    }
}