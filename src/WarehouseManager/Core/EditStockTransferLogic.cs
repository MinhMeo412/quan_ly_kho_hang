using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core
{
    public static class EditStockTransferLogic
    {
        //Lấy các thông tin của StockTransfer được chọn
        private static StockTransfer GetStockTransfer(int stockTransferID)
        {
            List<StockTransfer> stockTransfers = Program.Warehouse.StockTransferTable.StockTransfers ?? new List<StockTransfer>();
            StockTransfer stockTransfer = stockTransfers.FirstOrDefault(sT => sT.StockTransferID == stockTransferID) ?? new StockTransfer(0, 0, 0, DateTime.Now, "", "", 0);
            return stockTransfer;
        }

        public static DateTime GetStockTransferDate(int stockTransferID)
        {
            return GetStockTransfer(stockTransferID).StockTransferStartingDate ?? DateTime.Now;
        }

        public static string GetStockTransferStatus(int stockTransferID)
        {
            return GetStockTransfer(stockTransferID).StockTransferStatus;
        }

        public static string GetStockTransferDescription(int stockTransferID)
        {
            return $"{GetStockTransfer(stockTransferID).StockTransferDescription}";
        }

        public static int GetStockTransferFromWarehouse(int stockTransferID)
        {
            int fromWarehouseID = GetStockTransfer(stockTransferID).FromWarehouseID;

            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();

            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == fromWarehouseID) ?? new Warehouse(0, "", 0);
            string warehouseName = warehouse.WarehouseName;

            List<string> warehouseNames = GetWarehouseList();
            return warehouseNames.IndexOf(warehouseName);
        }

        public static int GetStockTransferToWarehouse(int stockTransferID)
        {
            int toWarehouseID = GetStockTransfer(stockTransferID).ToWarehouseID;

            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();

            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == toWarehouseID) ?? new Warehouse(0, "", 0);
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

        public static string GetStockTransferUserName(int stockTransferID)
        {
            int userID = GetStockTransfer(stockTransferID).UserID;

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

        // Lấy thông tin StockTransferDetail của phiếu
        public static DataTable GetStockTransferDetailData(int stockTransferID)
        {
            List<StockTransferDetail> stockTransferDetailsDetails = Program.Warehouse.StockTransferDetailTable.StockTransferDetails ?? new List<StockTransferDetail>();
            List<StockTransferDetail> currentStockTransferDetails = stockTransferDetailsDetails.Where(sTD => sTD.StockTransferID == stockTransferID).ToList();

            List<(int, string, int)> stockTransferDetailRows = new List<(int, string, int)>();

            foreach (StockTransferDetail currentStockTransferDetail in currentStockTransferDetails)
            {
                string productVariantName = GetProductVariantName(currentStockTransferDetail.ProductVariantID);

                stockTransferDetailRows.Add((
                    currentStockTransferDetail.ProductVariantID,
                    productVariantName,
                    currentStockTransferDetail.StockTransferDetailAmount));
            }

            return ConvertStockTransferDetailDataToDataTable(stockTransferDetailRows);
        }

        private static DataTable ConvertStockTransferDetailDataToDataTable(List<(int, string, int)> stockTransferDetailRows)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Variant ID", typeof(int));
            dataTable.Columns.Add("Variant Name", typeof(string));
            dataTable.Columns.Add("Amount", typeof(int));

            foreach (var stockTransferDetailRow in stockTransferDetailRows)
            {
                dataTable.Rows.Add(
                    stockTransferDetailRow.Item1,
                    stockTransferDetailRow.Item2,
                    stockTransferDetailRow.Item3);
            }

            return dataTable;
        }


        // Save Button
        public static void Save(int stockTransferID, string fromWarehouseName, string toWarehouseName, DateTime stockTransferStartingDate, string stockTransferStatus, string stockTransferDescription, string userName)
        {
            SaveStockTransfer(stockTransferID, fromWarehouseName, toWarehouseName, stockTransferStartingDate, stockTransferStatus, stockTransferDescription, userName);
        }

        // Lưu StockTransfer
        private static void SaveStockTransfer(int stockTransferID, string fromWarehouseName, string toWarehouseName, DateTime stockTransferStartingDate, string stockTransferStatus, string stockTransferDescription, string userName)
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            Warehouse fromWarehouse = warehouses.FirstOrDefault(w => w.WarehouseName == fromWarehouseName) ?? new Warehouse(0, "", 0);
            Warehouse toWarehouse = warehouses.FirstOrDefault(w => w.WarehouseName == toWarehouseName) ?? new Warehouse(0, "", 0);
            int fromWarehouseID = fromWarehouse.WarehouseID;
            int toWarehouseID = toWarehouse.WarehouseID;

            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            User user = users.FirstOrDefault(u => u.UserFullName == userName) ?? new User(0, "", "", "", "", "", 0);
            int userID = user.UserID;

            Program.Warehouse.StockTransferTable.Update(stockTransferID, fromWarehouseID, toWarehouseID, stockTransferStartingDate, stockTransferStatus, stockTransferDescription, userID);
        }


        // Add StockTransferDetail to Table and Database
        public static DataTable AddStockTransferDetail(DataTable currentDataTable, int productVariantID, int quantity, int stockTransferID)
        {
            DataTable dataTable = currentDataTable.Copy();

            string productVariantName = GetProductVariantName(productVariantID);
            dataTable.Rows.Add(productVariantID, productVariantName, quantity);

            Program.Warehouse.StockTransferDetailTable.Add(stockTransferID, productVariantID, quantity);

            return dataTable;
        }

        // Delete StockTransferDetail to Table and Database
        public static DataTable DeleteStockTransferDetail(DataTable currentDataTable, int row, int productVariantID, int stockTransferID)
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
            Program.Warehouse.StockTransferDetailTable.Delete(stockTransferID, productVariantID);
            return dataTable;
        }

        // Update StockTransferDetail to Table and Database
        public static DataTable UpdateStockTransferDetail(DataTable currentDataTable, int productVariantID, int quantity, int stockTransferID)
        {
            DataTable dataTable = currentDataTable.Copy();

            Program.Warehouse.StockTransferDetailTable.Update(stockTransferID, productVariantID, quantity);

            return dataTable;
        }
    }
}
