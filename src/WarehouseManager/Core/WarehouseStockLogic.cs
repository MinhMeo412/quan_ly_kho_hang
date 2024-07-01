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
            List<WarehouseStock> warehouseStocks = Program.Warehouse.WarehouseStockTable.WarehouseStocks ?? new List<WarehouseStock>();
            List<Product>? products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            List<ProductVariant> productVariants = GetProductVariants(warehouseChecklistDict);





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

        private static List<ProductVariant> GetProductVariants(Dictionary<CheckBox, int> warehouseChecklistDict)
        {
            List<WarehouseStock> warehouseStocks = Program.Warehouse.WarehouseStockTable.WarehouseStocks ?? new List<WarehouseStock>();
            List<ProductVariant> allProductVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();


            List<ProductVariant> relevantProductVariants = new List<ProductVariant>();
            foreach (KeyValuePair<CheckBox, int> warehouseChecklist in warehouseChecklistDict)
            {
                int warehouseID = warehouseChecklist.Value;

                List<int> productVariantIDs = warehouseStocks
                    .Where(ws => ws.WarehouseID == warehouseID)
                    .Select(ws => ws.ProductVariantID)
                    .ToList();


                
                foreach (ProductVariant productVariant in allProductVariants)
                {
                    if (productVariantIDs.Contains(productVariant.ProductID) && !relevantProductVariants.Contains(productVariant))
                    {
                        relevantProductVariants.Add(productVariant);
                    }
                }
            }


            return relevantProductVariants;
        }
    }
}