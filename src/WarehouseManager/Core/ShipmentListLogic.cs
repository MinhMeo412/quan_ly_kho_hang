using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;
using WarehouseManager.UI.Menu;

namespace WarehouseManager.Core
{
    public static class ShipmentListLogic
    {
        // Lấy dữ liệu về và đổi kiểu dữ liệu sang dạng DataTable
        public static DataTable GetData()
        {
            List<InboundShipment> inboundShipments = Program.Warehouse.InboundShipmentTable.InboundShipments ?? new List<InboundShipment>();
            List<OutboundShipment> outboundShipments = Program.Warehouse.OutboundShipmentTable.OutboundShipments ?? new List<OutboundShipment>();
            List<StockTransfer> stockTransfers = Program.Warehouse.StockTransferTable.StockTransfers ?? new List<StockTransfer>();
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            List<Supplier> suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();

            List<(string, int, string, string, string, string, DateTime?, string?)> shipmentMenuRows = new List<(string, int, string, string, string, string, DateTime?, string?)>();

            foreach (InboundShipment inboundShipment in inboundShipments)
            {
                User user = users.FirstOrDefault(u => u.UserID == inboundShipment.UserID) ?? new User(0, "", "", "", "", "", 0);
                string userName = user.UserFullName;
                Supplier supplier = suppliers.FirstOrDefault(s => s.SupplierID == inboundShipment.SupplierID) ?? new Supplier(0, "", "", "", "", "", "");
                string supplierName = supplier.SupplierName;
                Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == inboundShipment.WarehouseID) ?? new Warehouse(0, "", 0);
                string warehouseName = warehouse.WarehouseName;

                shipmentMenuRows.Add((
                    "Inbound",
                    inboundShipment.InboundShipmentID,
                    supplierName,
                    warehouseName,
                    inboundShipment.InboundShipmentStatus,
                    userName,
                    inboundShipment.InboundShipmentStartingDate,
                    inboundShipment.InboundShipmentDescription));
            }

            foreach (OutboundShipment outboundShipment in outboundShipments)
            {
                User user = users.FirstOrDefault(u => u.UserID == outboundShipment.UserID) ?? new User(0, "", "", "", "", "", 0);
                string userName = user.UserFullName;
                Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == outboundShipment.WarehouseID) ?? new Warehouse(0, "", 0);
                string warehouseName = warehouse.WarehouseName;

                shipmentMenuRows.Add((
                    "Outbound",
                    outboundShipment.OutboundShipmentID,
                    warehouseName,
                    outboundShipment.OutboundShipmentAddress,
                    outboundShipment.OutboundShipmentStatus,
                    userName,
                    outboundShipment.OutboundShipmentStartingDate,
                    outboundShipment.OutboundShipmentDescription
                ));
            }

            foreach (StockTransfer stockTransfer in stockTransfers)
            {
                User user = users.FirstOrDefault(u => u.UserID == stockTransfer.UserID) ?? new User(0, "", "", "", "", "", 0);
                string userName = user.UserFullName;
                Warehouse fromWarehouse = warehouses.FirstOrDefault(w => w.WarehouseID == stockTransfer.FromWarehouseID) ?? new Warehouse(0, "", 0);
                string fromWarehouseName = fromWarehouse.WarehouseName;
                Warehouse toWarehouse = warehouses.FirstOrDefault(w => w.WarehouseID == stockTransfer.ToWarehouseID) ?? new Warehouse(0, "", 0);
                string toWarehouseName = toWarehouse.WarehouseName;

                shipmentMenuRows.Add((
                    "Transfer",
                    stockTransfer.StockTransferID,
                    fromWarehouseName,
                    toWarehouseName,
                    stockTransfer.StockTransferStatus,
                    userName,
                    stockTransfer.StockTransferStartingDate,
                    stockTransfer.StockTransferDescription));
            }

            return ConvertShipmentMenuRowsToDataTable(shipmentMenuRows);
        }

        // Đổi kiểu dữ liệu từ List<...> sang DataTable
        private static DataTable ConvertShipmentMenuRowsToDataTable(List<(string, int, string, string, string, string, DateTime?, string?)> shipmentMenuRows)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Shipment Type", typeof(string));
            dataTable.Columns.Add("Shipment ID", typeof(int));
            dataTable.Columns.Add("From", typeof(string));
            dataTable.Columns.Add("To", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("User Name", typeof(string));
            dataTable.Columns.Add("Date", typeof(DateTime));
            dataTable.Columns.Add("Description", typeof(string));

            foreach (var shipmentMenuRow in shipmentMenuRows)
            {
                dataTable.Rows.Add(
                    shipmentMenuRow.Item1,
                    shipmentMenuRow.Item2,
                    shipmentMenuRow.Item3,
                    shipmentMenuRow.Item4,
                    shipmentMenuRow.Item5,
                    shipmentMenuRow.Item6,
                    shipmentMenuRow.Item7,
                    shipmentMenuRow.Item8);
            }

            return dataTable;
        }

        // Đổi kiểu dữ liệu từ DataTable sang List<...> (để thực hiện LINQ)
        private static List<(string, int, string, string, string, string, DateTime?, string?)> ConvertDataTableToShipmentMenuRows(DataTable dataTable)
        {
            List<(string, int, string, string, string, string, DateTime?, string?)> shipmentMenuRows = new List<(string, int, string, string, string, string, DateTime?, string?)>();

            foreach (DataRow row in dataTable.Rows)
            {
                shipmentMenuRows.Add(((string)row[0], (int)row[1], (string)row[2], (string)row[3], (string)row[4], (string)row[5], (DateTime?)row[6], (string?)row[7]));
            }
            return shipmentMenuRows;
        }


        /// <summary>
        /// Sort theo từ mà người dùng nhập vào ô tìm kiếm
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="searchTerm"></param>
        /// <returns>DataTable</returns>
        public static DataTable SortShipmentListBySearchTerm(DataTable dataTable, string searchTerm)
        {
            return SortDataTable.BySearchTerm(dataTable, searchTerm);
        }


        /// <summary>
        /// sort theo cột mà người dùng bấm vào
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="columnToSortBy"></param>
        /// <param name="sortColumnInDescendingOrder"></param>
        /// <returns>DataTable</returns>
        public static DataTable SortShipmentListByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            return SortDataTable.ByColumn(dataTable, columnToSortBy, sortColumnInDescendingOrder);
        }

        //Delete shipment 
        public static DataTable DeleteShipment(DataTable dataTable, string shipmentType, int shipmentID)
        {
            switch (shipmentType)
            {
                case "Inbound":
                    Program.Warehouse.InboundShipmentTable.Delete(shipmentID);
                    break;
                case "Outbound":
                    Program.Warehouse.OutboundShipmentTable.Delete(shipmentID);
                    break;
                case "Transfer":
                    Program.Warehouse.StockTransferTable.Delete(shipmentID);
                    break;
                default:
                    throw new ArgumentException("Invalid shipment type");
            }

            List<(string, int, string, string, string, string, DateTime?, string?)> shipments = ConvertDataTableToShipmentMenuRows(dataTable);

            shipments.RemoveAll(s => s.Item1 == shipmentType && s.Item2 == shipmentID);

            return ConvertShipmentMenuRowsToDataTable(shipments);
        }
    }
}