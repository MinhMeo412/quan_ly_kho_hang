using System.Data;
using System.Net.WebSockets;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddOutboundShipmentLogic
    {
        //Get Warehouse List
        public static List<string> GetWarehouseList()
        {
            return EditOutboundShipmentLogic.GetWarehouseList();
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

        // Add OutboundShipmentDetail only to DataTable (not include DataBase)
        public static DataTable AddOutboundShipmentDetailToDataTable(DataTable currentDataTable, int productVariantID, int quantity)
        {
            DataTable dataTable = currentDataTable.Copy();
            string productVariantName = EditOutboundShipmentLogic.GetProductVariantName(productVariantID);
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
        public static void Save(string outboundShipmentAddress, string warehouseName, DateTime outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, string userName, DataTable dataTable)
        {
            int outboundShipmentID = GetCurrentHighestOutboundShipmentID() + 1;
            AddOutboundShipment(outboundShipmentID, outboundShipmentAddress, warehouseName, outboundShipmentStartingDate, outboundShipmentStatus, outboundShipmentDescription, userName);
            AddOutboundShipmentDetail(outboundShipmentID, dataTable);
        }

        //Get highest ShipmentID
        public static int GetCurrentHighestOutboundShipmentID()
        {
            List<OutboundShipment> outboundShipments = Program.Warehouse.OutboundShipmentTable.OutboundShipments ?? new List<OutboundShipment>();
            int highestOutboundShipmentID = outboundShipments.Max(o => o.OutboundShipmentID);
            return highestOutboundShipmentID;
        }

        // Add OutboundShipment to DataBase
        private static void AddOutboundShipment(int outboundShipmentID, string outboundShipmentAddress, string warehouseName, DateTime outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, string userName)
        {
            List<Warehouse>? warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            Warehouse? warehouse = warehouses.FirstOrDefault(w => w.WarehouseName == warehouseName);

            int warehouseID = 0;
            if (warehouse != null)
            {
                warehouseID = warehouse.WarehouseID;
            }

            List<User>? users = Program.Warehouse.UserTable.Users ?? new List<User>();
            User? user = users.FirstOrDefault(u => u.UserFullName == userName);

            int userID = 0;
            if (user != null)
            {
                userID = user.UserID;
            }

            Program.Warehouse.OutboundShipmentTable.Add(outboundShipmentID, warehouseID, outboundShipmentAddress, outboundShipmentStartingDate, outboundShipmentStatus, outboundShipmentDescription, userID);
        }

        // Add OutboundShipmentDetail to DataBase
        private static void AddOutboundShipmentDetail(int outboundShipmentID, DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                AddOutboundShipmentDetailToDataBase(outboundShipmentID, (int)row[0], (int)row[2]);
            }
        }
        private static void AddOutboundShipmentDetailToDataBase(int outboundShipmentID, int productVariantID, int quantity)
        {
            Program.Warehouse.OutboundShipmentDetailTable.Add(outboundShipmentID, productVariantID, quantity);
        }
    }
}