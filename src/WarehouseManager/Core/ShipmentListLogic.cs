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
            List<(string, int, string, string, string, string, DateTime?, string?)> shipments = ConvertDataTableToShipmentMenuRows(dataTable);
            List<((string, int, string, string, string, string, DateTime?, string?), double)> shipmentSimilarities = new List<((string, int, string, string, string, string, DateTime?, string?), double)>();

            foreach ((string, int, string, string, string, string, DateTime?, string?) shipment in shipments)
            {
                double maxSimilarity = Misc.MaxDouble(
                    Misc.JaccardSimilarity($"{shipment.Item1}", searchTerm),
                    Misc.JaccardSimilarity($"{shipment.Item2}", searchTerm),
                    Misc.JaccardSimilarity($"{shipment.Item3}", searchTerm),
                    Misc.JaccardSimilarity($"{shipment.Item4}", searchTerm),
                    Misc.JaccardSimilarity($"{shipment.Item5}", searchTerm),
                    Misc.JaccardSimilarity($"{shipment.Item6}", searchTerm),
                    Misc.JaccardSimilarity($"{shipment.Item7}", searchTerm),
                    Misc.JaccardSimilarity($"{shipment.Item8}", searchTerm)
                );

                shipmentSimilarities.Add((shipment, maxSimilarity));
            }

            List<(string, int, string, string, string, string, DateTime?, string?)> sortedShipments = shipmentSimilarities
                .OrderByDescending(ss => ss.Item2)
                .Select(ss => ss.Item1)
                .ToList();

            return ConvertShipmentMenuRowsToDataTable(sortedShipments);
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
            List<(string, int, string, string, string, string, DateTime?, string?)> shipments = ConvertDataTableToShipmentMenuRows(dataTable);

            switch (columnToSortBy)
            {
                case 0:
                    shipments = shipments.OrderBy(s => s.Item1).ToList();
                    break;
                case 1:
                    shipments = shipments.OrderBy(s => s.Item2).ToList();
                    break;
                case 2:
                    shipments = shipments.OrderBy(s => s.Item3).ToList();
                    break;
                case 3:
                    shipments = shipments.OrderBy(s => s.Item4).ToList();
                    break;
                case 4:
                    shipments = shipments.OrderBy(s => s.Item5).ToList();
                    break;
                case 5:
                    shipments = shipments.OrderBy(s => s.Item6).ToList();
                    break;
                case 6:
                    shipments = shipments.OrderBy(s => s.Item7).ToList();
                    break;
                case 7:
                    shipments = shipments.OrderBy(s => s.Item8).ToList();
                    break;
                default:
                    break;
            }

            if (sortColumnInDescendingOrder)
            {
                shipments.Reverse();
            }

            DataTable sortedDataTable = ConvertShipmentMenuRowsToDataTable(shipments);

            sortedDataTable.Columns[columnToSortBy].ColumnName = Misc.ShowCurrentSortingDirection(sortedDataTable.Columns[columnToSortBy].ColumnName, sortColumnInDescendingOrder);

            return sortedDataTable;
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