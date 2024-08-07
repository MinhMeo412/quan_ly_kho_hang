using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.UI.Pages;

namespace WarehouseManager.Core.Pages
{
    public static class AddStockTransferLogic
    {
        //Get Warehouse List
        public static List<string> GetWarehouseList()
        {
            return EditStockTransferLogic.GetWarehouseList();
        }

        //Get Current Account User Full Name
        public static string GetUserFullName()
        {
            string userName = $"{Program.Warehouse.Username}";
            List<User>? users = Program.Warehouse.UserTable.Users ?? new List<User>();
            User? user = users.FirstOrDefault(u => u.UserName == userName);

            string userFullName = "";
            if (user != null)
            {
                userFullName = user.UserFullName;
            }

            return userFullName;
        }

        //Create DataTable
        public static DataTable GetDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Variant ID", typeof(int));
            dataTable.Columns.Add("Variant Name", typeof(string));
            dataTable.Columns.Add("Amount", typeof(int));

            return dataTable;
        }

        // Add StockTransferDetail only to DataTable (not include DataBase)
        public static DataTable AddStockTransferDetailToDataTable(DataTable currentDataTable, int productVariantID, int quantity)
        {
            DataTable dataTable = currentDataTable.Copy();
            string productVariantName = EditStockTransferLogic.GetProductVariantName(productVariantID);
            dataTable.Rows.Add(productVariantID, productVariantName, quantity);
            return dataTable;
        }

        //Check if the added Variant is correct
        public static string CheckVariantAdded(string warehouseName, int productVariantID, int quantity)
        {
            List<Warehouse>? warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            Warehouse? warehouse = warehouses.FirstOrDefault(w => w.WarehouseName == warehouseName);
            int warehouseID = 0;
            if (warehouse == null)
            {
                return "Error: Warehouse not found.";

            }

            warehouseID = warehouse.WarehouseID;

            List<WarehouseStock>? warehouseStocks = Program.Warehouse.WarehouseStockTable.WarehouseStocks ?? new List<WarehouseStock>();
            WarehouseStock? warehouseStock = warehouseStocks.FirstOrDefault(wS => wS.ProductVariantID == productVariantID && wS.WarehouseID == warehouseID);

            if (warehouseStock == null)
            {
                return "Error: There is no product in warehouse.";

            }

            if (quantity > warehouseStock.WarehouseStockQuantity)
            {
                return "Error: Not enough stock.";

            }
            return "";
        }

        //Save function
        public static void Save(string fromWarehouseName, string toWarehouseName, DateTime stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, string userName, DataTable dataTable)
        {
            int stockTransferID = GetCurrentHighestStockTransferID() + 1;
            AddStockTransfer(stockTransferID, fromWarehouseName, toWarehouseName, stockTransferStartingDate, stockTransferStatus, stockTransferDescription, userName);
            AddStockTransferDetail(stockTransferID, dataTable);

            EditStockTransfer.Display(stockTransferID, true);
        }

        //Get highest ShipmentID
        public static int GetCurrentHighestStockTransferID()
        {
            List<StockTransfer> stockTransfers = Program.Warehouse.StockTransferTable.StockTransfers ?? new List<StockTransfer>();
            int highestStockTransferID = stockTransfers.Max(s => s.StockTransferID);
            return highestStockTransferID;
        }

        // Add StockTransfer to DataBase
        private static void AddStockTransfer(int stockTransferID, string fromWarehouseName, string toWarehouseName, DateTime stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, string userName)
        {
            List<Warehouse>? warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            Warehouse? formWarehouse = warehouses.FirstOrDefault(w => w.WarehouseName == fromWarehouseName);
            Warehouse? toWarehouse = warehouses.FirstOrDefault(w => w.WarehouseName == toWarehouseName);

            int fromWarehouseID = 0;
            if (formWarehouse != null)
            {
                fromWarehouseID = formWarehouse.WarehouseID;
            }

            int toWarehouseID = 0;
            if (toWarehouse != null)
            {
                toWarehouseID = toWarehouse.WarehouseID;
            }

            List<User>? users = Program.Warehouse.UserTable.Users ?? new List<User>();
            User? user = users.FirstOrDefault(u => u.UserFullName == userName);

            int userID = 0;
            if (user != null)
            {
                userID = user.UserID;
            }

            Program.Warehouse.StockTransferTable.Add(stockTransferID, fromWarehouseID, toWarehouseID, stockTransferStartingDate, stockTransferStatus, stockTransferDescription, userID);
        }

        // Add StockTransferDetail to DataBase
        private static void AddStockTransferDetail(int stockTransferID, DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                AddStockTransferDetailToDataBase(stockTransferID, (int)row[0], (int)row[2]);
            }
        }
        private static void AddStockTransferDetailToDataBase(int stockTransferID, int productVariantID, int quantity)
        {
            Program.Warehouse.StockTransferDetailTable.Add(stockTransferID, productVariantID, quantity);
        }
    }
}