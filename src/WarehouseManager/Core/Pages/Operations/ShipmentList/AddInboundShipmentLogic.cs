using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddInboundShipmentLogic
    {
        //Get Warehouse List
        public static List<string> GetWarehouseList()
        {
            return EditInboundShipmentLogic.GetWarehouseList();
        }

        //Get Supplier List
        public static List<string> GetSupplierList()
        {
            return EditInboundShipmentLogic.GetSupplierList();
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

        // Add InboundShipmentDetail only to DataTable (not include DataBase)
        public static DataTable AddInboundShipmentDetailToDataTable(DataTable currentDataTable, int productVariantID, int quantity)
        {
            DataTable dataTable = currentDataTable.Copy();
            string productVariantName = EditInboundShipmentLogic.GetProductVariantName(productVariantID);
            dataTable.Rows.Add(productVariantID, productVariantName, quantity);
            return dataTable;
        }


        //Save function
        public static void Save(string supplierName, string warehouseName, DateTime inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, string userName, DataTable dataTable)
        {
            int inboundShipmentID = GetCurrentHighestInboundShipmentID() + 1;
            AddInboundShipment(inboundShipmentID, supplierName, warehouseName, inboundShipmentStartingDate, inboundShipmentStatus, inboundShipmentDescription, userName);
            AddInboundShipmentDetail(inboundShipmentID, dataTable);
        }

        //Get highest ShipmentID
        public static int GetCurrentHighestInboundShipmentID()
        {
            List<InboundShipment> inboundShipments = Program.Warehouse.InboundShipmentTable.InboundShipments ?? new List<InboundShipment>();
            int highestInboundShipmentID = inboundShipments.Max(i => i.InboundShipmentID);
            return highestInboundShipmentID;
        }

        // Add InboundShipment to DataBase
        private static void AddInboundShipment(int inboundShipmentID, string supplierName, string warehouseName, DateTime inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, string userName)
        {
            List<Supplier>? suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();
            Supplier? supplier = suppliers.FirstOrDefault(s => s.SupplierName == supplierName);

            int? supplierID = null;
            if (supplier != null)
            {
                supplierID = supplier.SupplierID;
            }

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

            Program.Warehouse.InboundShipmentTable.Add(inboundShipmentID, supplierID, warehouseID, inboundShipmentStartingDate, inboundShipmentStatus, inboundShipmentDescription, userID);
        }

        // Add InboundShipmentDetail to DataBase
        private static void AddInboundShipmentDetail(int inboundShipmentID, DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                AddInboundShipmentDetailToDataBase(inboundShipmentID, (int)row[0], (int)row[2]);
            }
        }
        private static void AddInboundShipmentDetailToDataBase(int inboundShipmentID, int productVariantID, int quantity)
        {
            Program.Warehouse.InboundShipmentDetailTable.Add(inboundShipmentID, productVariantID, quantity);
        }






















    }
}