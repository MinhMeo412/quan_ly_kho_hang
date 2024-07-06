using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core
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
        public static DataTable AddOutboundShipmentDetail(DataTable currentDataTable, int productVariantID, int quantity)
        {
            DataTable dataTable = currentDataTable.Copy();
            string productVariantName = EditOutboundShipmentLogic.GetProductVariantName(productVariantID);
            dataTable.Rows.Add(productVariantID, productVariantName, quantity);
            return dataTable;
        }

        // Delete OutboundShipmentDetail only to DataTable (not include DataBase)
        public static DataTable DeleteOutboundShipmentDetail(DataTable currentDataTable, int row)
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


        //Save function
        public static void Save(string outboundShipmentAddress, string warehouseName, DateTime outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, string userName, DataTable dataTable)
        {
            int outboundShipmentID = GetCurrentHighestOutboundShipmentID() + 1;
            AddOutboundShipment(outboundShipmentID, outboundShipmentAddress, warehouseName, outboundShipmentStartingDate, outboundShipmentStatus, outboundShipmentDescription, userName);
            AddOutboundShipmentDetail(outboundShipmentID, dataTable);
        }

        //Get highest ShipmentID
        private static int GetCurrentHighestOutboundShipmentID()
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