using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class EditInventoryAuditLogic
    {
        //Lấy các thông tin của InventoryAudit được chọn
        private static InventoryAudit GetInventoryAudit(int inventoryAuditID)
        {
            List<InventoryAudit> inventoryAudits = Program.Warehouse.InventoryAuditTable.InventoryAudits ?? new List<InventoryAudit>();
            InventoryAudit inventoryAudit = inventoryAudits.FirstOrDefault(iA => iA.InventoryAuditID == inventoryAuditID) ?? new InventoryAudit(0, 0, "Processing", "", 0, DateTime.Now);
            return inventoryAudit;
        }

        public static DateTime GetInventoryAuditDate(int inventoryAuditID)
        {
            return GetInventoryAudit(inventoryAuditID).InventoryAuditTime;
        }

        //public static string GetInventoryAuditStatus(int inventoryAuditID)
        //{
        //    return GetInventoryAudit(inventoryAuditID).InventoryAuditStatus;
        //}

        //public static string GetInventoryAuditDescription(int inventoryAuditID)
        //{
        //    return $"{GetInventoryAudit(inventoryAuditID).InventoryAuditDescription}";
        //}

        public static int GetInventoryAuditWarehouse(int inventoryAuditID)
        {
            int warehouseID = GetInventoryAudit(inventoryAuditID).WarehouseID;

            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();

            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == warehouseID) ?? new Warehouse(0, "", 0);
            string warehouseName = warehouse.WarehouseName;

            List<string> warehouseNames = GetWarehouseList();
            return warehouseNames.IndexOf(warehouseName);
        }

        public static List<string> GetWarehouseList()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            List<string> warehouseNames = new List<string>();

            foreach (Warehouse warehouse in warehouses)
            {
                warehouseNames.Add(warehouse.WarehouseName);
            }

            return warehouseNames;
        }

        public static string GetInventoryAuditUserName(int inventoryAuditID)
        {
            int userID = GetInventoryAudit(inventoryAuditID).UserID;

            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();

            User user = users.FirstOrDefault(u => u.UserID == userID) ?? new User(0, "", "", "", "", "", 0);
            string userName = user.UserFullName;
            return userName;
        }

        public static string GetProductVariantName(int productVariantID)
        {
            List<ProductVariant> productVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();
            List<Product> products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            ProductVariant productVariant = productVariants.FirstOrDefault(pV => pV.ProductVariantID == productVariantID) ?? new ProductVariant(0, 0, "", "", "");
            Product product = products.FirstOrDefault(p => p.ProductID == productVariant.ProductID) ?? new Product(0, "", "", 0, 0);
            string productVariantName = $"{product.ProductName} - {productVariant.ProductVariantColor} - {productVariant.ProductVariantSize}";
            return productVariantName;
        }

        // Lấy thông tin InventoryAuditDetail của phiếu
        public static DataTable GetInventoryAuditDetailData(int inventoryAuditID)
        {
            List<InventoryAuditDetail> inventoryAuditDetails = Program.Warehouse.InventoryAuditDetailTable.InventoryAuditDetails ?? new List<InventoryAuditDetail>();
            List<InventoryAuditDetail> currentInventoryAuditDetails = inventoryAuditDetails.Where(iAD => iAD.InventoryAuditID == inventoryAuditID).ToList();

            List<(int, string, int)> inventoryAuditDetailRows = new List<(int, string, int)>();

            foreach (InventoryAuditDetail currentInventoryAuditDetail in currentInventoryAuditDetails)
            {
                string productVariantName = GetProductVariantName(currentInventoryAuditDetail.ProductVariantID);

                inventoryAuditDetailRows.Add((
                    currentInventoryAuditDetail.ProductVariantID,
                    productVariantName,
                    currentInventoryAuditDetail.InventoryAuditDetailActualQuantity));
            }

            return ConvertInventoryAuditDetailDataToDataTable(inventoryAuditDetailRows);
        }

        private static DataTable ConvertInventoryAuditDetailDataToDataTable(List<(int, string, int)> inventoryAuditDetailRows)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Variant ID", typeof(int));
            dataTable.Columns.Add("Variant Name", typeof(string));
            dataTable.Columns.Add("Stock Amount", typeof(int)); // Cột này chưa có
            dataTable.Columns.Add("Actual Amount", typeof(int)); //

            foreach (var inventoryAuditDetailRow in inventoryAuditDetailRows)
            {
                dataTable.Rows.Add(
                    inventoryAuditDetailRow.Item1,
                    inventoryAuditDetailRow.Item2,
                    inventoryAuditDetailRow.Item3);
            }

            return dataTable;
        }

        // Save Button
        public static void Save(int inventoryAuditID, string warehouseName, DateTime inventoryAuditTime, string userName)
        {
            SaveInventoryAudit(inventoryAuditID, warehouseName, inventoryAuditTime, userName);
        }

        // Lưu InventoryAudit
        private static void SaveInventoryAudit(int inventoryAuditID, string warehouseName, DateTime inventoryAuditTime, string userName)
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseName == warehouseName) ?? new Warehouse(0, "", 0);
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            User user = users.FirstOrDefault(u => u.UserFullName == userName) ?? new User(0, "", "", "", "", "", 0);
            int warehouseID = warehouse.WarehouseID;
            int userID = user.UserID;

            // Program.Warehouse.InventoryAuditTable.Update(inventoryAuditID, warehouseID, userID, inventoryAuditTime);
        }


        // Add InventoryAuditDetail to Table and Database
        public static DataTable AddInventoryAuditDetail(DataTable currentDataTable, int productVariantID, int quantity, int inventoryAuditID)
        {
            DataTable dataTable = currentDataTable.Copy();

            string productVariantName = GetProductVariantName(productVariantID);
            dataTable.Rows.Add(productVariantID, productVariantName, quantity);

            Program.Warehouse.InventoryAuditDetailTable.Add(inventoryAuditID, productVariantID, quantity);

            return dataTable;
        }

        // Delete InventoryAuditDetail to Table and Database
        public static DataTable DeleteInventoryAuditDetail(DataTable currentDataTable, int row, int productVariantID, int inventoryAuditID)
        {
            DataTable dataTable = currentDataTable.Copy();

            if (row < 0 || row >= dataTable.Rows.Count)
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
            Program.Warehouse.InventoryAuditDetailTable.Delete(inventoryAuditID, productVariantID);
            return dataTable;
        }

        // Update InventoryAuditDetail to Table and Database
        public static DataTable UpdateInventoryAuditDetail(DataTable currentDataTable, int productVariantID, int quantity, int inventoryAuditID)
        {
            DataTable dataTable = currentDataTable.Copy();

            Program.Warehouse.InventoryAuditDetailTable.Update(inventoryAuditID, productVariantID, quantity);

            return dataTable;
        }
    }
}
