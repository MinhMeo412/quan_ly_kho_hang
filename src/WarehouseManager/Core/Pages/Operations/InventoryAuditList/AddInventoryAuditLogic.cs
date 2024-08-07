using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;
using WarehouseManager.UI.Pages;

namespace WarehouseManager.Core.Pages
{
    public static class AddInventoryAuditLogic
    {
        public static List<string> GetWarehouseNames()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetWarehouseList();
        }

        public static Dictionary<int, string> GetVariantList()
        {
            List<Product> products = GetProducts();
            List<ProductVariant> productVariants = GetProductVariants();

            Dictionary<int, string> variantDict = new Dictionary<int, string>();
            foreach (ProductVariant productVariant in productVariants)
            {
                variantDict[productVariant.ProductVariantID] = GetProductVariantName(productVariant, products);
            }

            return variantDict;
        }

        public static int GetVariantID(string variantName, Dictionary<int, string> variantList)
        {
            foreach (KeyValuePair<int, string> variant in variantList)
            {
                if (variant.Value == variantName)
                {
                    return variant.Key;
                }
            }
            return -1;
        }

        public static int GetVariantIndex(string variantID, Dictionary<int, string> variantList)
        {
            int productVariantID = int.TryParse(variantID, out int parsedID) ? parsedID : -1;

            int index = 0;
            foreach (KeyValuePair<int, string> variant in variantList)
            {
                if (variant.Key == productVariantID)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public static DataTable AddVariant(string warehouseName, string variantID, string quantity, DataTable dataTable)
        {
            List<Warehouse> warehouses = GetWarehouses();
            List<WarehouseStock> warehouseStocks = GetWarehouseStocks();
            List<Product> products = GetProducts();
            List<ProductVariant> productVariants = GetProductVariants();

            return AddVariant(warehouseName, variantID, quantity, dataTable, warehouses, warehouseStocks, products, productVariants);
        }

        internal static DataTable AddVariant(string warehouseName, string variantID, string quantity, DataTable dataTable, List<Warehouse> warehouses, List<WarehouseStock> warehouseStocks, List<Product> products, List<ProductVariant> productVariants)
        {
            if (!VariantAlreadyExist(variantID, dataTable))
            {
                int warehouseID = GetWarehouseID(warehouseName, warehouses);
                int productVariantID = int.TryParse(variantID, out int parsedID) ? parsedID : -1;
                string productName = GetProductVariantName(productVariantID, productVariants, products);
                int stockAmount = GetStockAmount(warehouseID, productVariantID, warehouseStocks);
                int actualAmount = int.TryParse(quantity, out int parsedQuantity) ? parsedQuantity : 0;

                dataTable.Rows.Add(
                    productVariantID,
                    productName,
                    stockAmount,
                    actualAmount,
                    actualAmount - stockAmount
                );
            }
            return dataTable;
        }

        public static DataTable Search(DataTable dataTable, string searchTerm)
        {
            return SortDataTable.BySearchTerm(dataTable, searchTerm);
        }

        public static DataTable GetDataTable()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Product");
            dataTable.Columns.Add("Stock Amount");
            dataTable.Columns.Add("Actual Amount");
            dataTable.Columns.Add("Difference");

            return dataTable;
        }
        public static DataTable EditVariant(int row, string newAmount, DataTable dataTable)
        {
            return EditInventoryAuditLogic.EditVariant(row, newAmount, dataTable);
        }

        public static DataTable DeleteVariant(DataTable currentDataTable, int row)
        {
            DataTable dataTable = currentDataTable.Copy();

            if (row < 0)
            {
                return dataTable;
            }

            DataRow rowToDelete = dataTable.Rows[row];

            if (rowToDelete != null)
            {
                // Mark the row for deletion
                rowToDelete.Delete();

                // Commit the deletion
                dataTable.AcceptChanges();
            }

            return dataTable;
        }

        public static DataTable GetAllStock(string warehouseName, DataTable dataTable)
        {
            List<Warehouse> warehouses = GetWarehouses();
            List<WarehouseStock> warehouseStocks = GetWarehouseStocks();
            List<WarehouseStock> relevantWarehouseStocks = GetRelevantWarehouseStocks(GetWarehouseID(warehouseName, warehouses), warehouseStocks);
            List<Product> products = GetProducts();
            List<ProductVariant> productVariants = GetProductVariants();

            foreach (WarehouseStock warehouseStock in relevantWarehouseStocks)
            {
                if (warehouseStock.WarehouseStockQuantity > 0)
                {
                    dataTable = AddVariant(
                        warehouseName,
                        $"{warehouseStock.ProductVariantID}",
                        "0",
                        dataTable,
                        warehouses,
                        warehouseStocks,
                        products,
                        productVariants
                    );
                }
            }

            return dataTable;
        }

        public static void Save(string warehouseName, string description, DataTable dataTable)
        {
            int inventoryAuditID = GetCurrentHighestInventoryAuditID() + 1;
            AddInventoryAudit(inventoryAuditID, warehouseName, description);
            AddInventoryAuditDetail(inventoryAuditID, dataTable);

            EditInventoryAudit.Display(inventoryAuditID, true);
        }

        private static List<Product> GetProducts()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetProducts();
        }

        private static List<ProductVariant> GetProductVariants()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetProductVariants();
        }

        private static List<WarehouseStock> GetWarehouseStocks()
        {
            List<WarehouseStock> warehouseStocks = Program.Warehouse.WarehouseStockTable.WarehouseStocks ?? new List<WarehouseStock>();
            return warehouseStocks;
        }

        private static List<Warehouse> GetWarehouses()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetWarehouses();
        }

        private static List<InventoryAudit> GetInventoryAudits()
        {
            List<InventoryAudit> inventoryAudits = Program.Warehouse.InventoryAuditTable.InventoryAudits ?? new List<InventoryAudit>();
            return inventoryAudits;
        }

        private static List<User> GetUsers()
        {
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            return users;
        }

        internal static string GetProductVariantName(int variantID, List<ProductVariant> productVariants, List<Product> products)
        {
            ProductVariant variant = productVariants.FirstOrDefault(pv => pv.ProductVariantID == variantID) ?? new ProductVariant(0, 0, null, null, null);
            return GetProductVariantName(variant, products);
        }

        internal static string GetProductVariantName(ProductVariant variant, List<Product> products)
        {
            int productID = variant.ProductID;
            Product product = products.FirstOrDefault(p => p.ProductID == productID) ?? new Product(0, "", null, null, null);
            return $"{product.ProductName} {variant.ProductVariantColor} {variant.ProductVariantSize}";
        }

        private static bool VariantAlreadyExist(string variantID, DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if ($"{row[0]}" == variantID)
                {
                    return true;
                }
            }

            return false;
        }

        private static int GetWarehouseID(string warehouseName, List<Warehouse> warehouses)
        {
            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseName == warehouseName) ?? new Warehouse(0, "", null);
            return warehouse.WarehouseID;
        }

        internal static int GetStockAmount(int warehouseID, int variantID, List<WarehouseStock> warehouseStocks)
        {
            WarehouseStock warehouseStock = warehouseStocks.FirstOrDefault(ws => ws.WarehouseID == warehouseID && ws.ProductVariantID == variantID) ?? new WarehouseStock(0, 0, 0);
            return warehouseStock.WarehouseStockQuantity;
        }

        private static List<WarehouseStock> GetRelevantWarehouseStocks(int warehouseID, List<WarehouseStock> warehouseStocks)
        {
            List<WarehouseStock> relevantWarehousesStocks = warehouseStocks
            .Where(ws => ws.WarehouseID == warehouseID)
            .ToList();

            return relevantWarehousesStocks;
        }

        private static int GetCurrentHighestInventoryAuditID()
        {
            List<InventoryAudit> inventoryAudits = GetInventoryAudits();
            int highestID = inventoryAudits.Max(i => i.InventoryAuditID);
            return highestID;
        }

        private static void AddInventoryAudit(int inventoryAuditID, string warehouseName, string description)
        {
            int warehouseID = GetWarehouseID(warehouseName, GetWarehouses());
            string inventoryAuditStatus = "Processing";
            int userID = GetUserID($"{Program.Warehouse.Username}", GetUsers());
            DateTime dateTime = DateTime.Now;

            Program.Warehouse.InventoryAuditTable.Add(
                inventoryAuditID,
                warehouseID,
                inventoryAuditStatus,
                description,
                userID,
                dateTime
            );
        }

        private static void AddInventoryAuditDetail(int inventoryAuditID, DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                Program.Warehouse.InventoryAuditDetailTable.Add(
                    inventoryAuditID,
                    int.Parse($"{row[0]}"),
                    int.Parse($"{row[2]}"),
                    int.Parse($"{row[3]}")
                );
            }
        }

        internal static int GetUserID(string username, List<User> users)
        {
            User user = users.FirstOrDefault(u => u.UserName == username) ?? new User(0, "", "", "", null, null, 4);
            return user.UserID;
        }

    }
}
