using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;
using WarehouseManager.UI.Menu;

namespace WarehouseManager.Core
{
    public class ShipmentTempList
    {
        public string ShipmentType { get; set; }
        public int ShipmentID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

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

            return ConvertAllShipmentListToDataTable(inboundShipments, outboundShipments, stockTransfers, users, warehouses, suppliers);
        }

        // Đổi kiểu dữ liệu từ List<...> sang DataTable
        private static DataTable ConvertAllShipmentListToDataTable(
            List<InboundShipment> inboundShipments,
            List<OutboundShipment> outboundShipments,
            List<StockTransfer> stockTransfers,
            List<User> users,
            List<Warehouse> warehouses,
            List<Supplier> suppliers)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Shipment Type", typeof(string));
            dataTable.Columns.Add("Shipment ID", typeof(int));
            dataTable.Columns.Add("From", typeof(string));
            dataTable.Columns.Add("To", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("User Name", typeof(string));
            dataTable.Columns.Add("Date", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));

            //Create a dictionary to map userID to userName, warehouseID to warehouseName
            var userDictionary = users.ToDictionary(user => user.UserID, user => user.UserFullName);
            var warehouseDictionary = warehouses.ToDictionary(warehouse => warehouse.WarehouseID, warehouse => warehouse.WarehouseName);
            var supplierDictionary = suppliers.ToDictionary(supplier => supplier.SupplierID, supplier => supplier.SupplierName);

            inboundShipments ??= new List<InboundShipment>();
            foreach (InboundShipment inboundShipment in inboundShipments)
            {
                string userName = userDictionary.TryGetValue(inboundShipment.UserID, out var Uname) ? Uname : "Unknow";
                string warehouseName = warehouseDictionary.TryGetValue(inboundShipment.WarehouseID, out var WHname) ? WHname : "Unknow";
                string supplierName = supplierDictionary.TryGetValue(inboundShipment.SupplierID, out var Sname) ? Sname : "Unknow";
                dataTable.Rows.Add(
                "Inbound",
                inboundShipment.InboundShipmentID,
                supplierName,
                warehouseName,
                inboundShipment.InboundShipmentStatus,
                userName,
                inboundShipment.InboundShipmentStartingDate,
                inboundShipment.InboundShipmentDescription);
            }

            outboundShipments ??= new List<OutboundShipment>();
            foreach (OutboundShipment outboundShipment in outboundShipments)
            {
                string userName = userDictionary.TryGetValue(outboundShipment.UserID, out var Uname) ? Uname : "Unknow";
                string warehouseName = warehouseDictionary.TryGetValue(outboundShipment.WarehouseID, out var WHname) ? WHname : "Unknow";
                dataTable.Rows.Add(
                "Outbound",
                outboundShipment.OutboundShipmentID,
                warehouseName,
                outboundShipment.OutboundShipmentAddress,
                outboundShipment.OutboundShipmentStatus,
                userName,
                outboundShipment.OutboundShipmentStartingDate,
                outboundShipment.OutboundShipmentDescription);
            }

            stockTransfers ??= new List<StockTransfer>();
            foreach (StockTransfer stockTransfer in stockTransfers)
            {
                string userName = userDictionary.TryGetValue(stockTransfer.UserID, out var Uname) ? Uname : "Unknow";
                string fromWarehouseName = warehouseDictionary.TryGetValue(stockTransfer.FromWarehouseID, out var FWHname) ? FWHname : "Unknow";
                string toWarehouseName = warehouseDictionary.TryGetValue(stockTransfer.ToWarehouseID, out var TWHname) ? TWHname : "Unknow";
                dataTable.Rows.Add(
                "Transfer",
                stockTransfer.StockTransferID,
                fromWarehouseName,
                toWarehouseName,
                stockTransfer.StockTransferStatus,
                userName,
                stockTransfer.StockTransferStartingDate,
                stockTransfer.StockTransferDescription);
            }
            return dataTable;
        }

        // Đổi kiểu dữ liệu từ DataTable sang List<InboundShipment> (để thực hiện LINQ)
        private static List<ShipmentTempList> ConvertDataTableToList(DataTable dataTable)
        {
            List<ShipmentTempList> shipmentList = new List<ShipmentTempList>();

            foreach (DataRow row in dataTable.Rows)
            {
                ShipmentTempList shipments = new ShipmentTempList
                {
                    ShipmentType = row["Shipment Type"].ToString(),
                    ShipmentID = (int)row["Shipment ID"],
                    From = row["From"].ToString(),
                    To = row["To"].ToString(),
                    Status = row["Status"].ToString(),
                    UserName = row["User Name"].ToString(),
                    Date = DateTime.Parse(row["Date"].ToString()),
                    Description = row["Description"].ToString()
                };
                shipmentList.Add(shipments);
            }
            return shipmentList;
        }


        /// <summary>
        /// Sort theo từ mà người dùng nhập vào ô tìm kiếm
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="searchTerm"></param>
        /// <returns>DataTable</returns>


        public static DataTable SortShipmentListBySearchTerm(DataTable dataTable, string searchTerm)
        {
            List<ShipmentTempList> shipments = ConvertDataTableToList(dataTable);
            List<(ShipmentTempList, double)> shipmentSimilarities = new List<(ShipmentTempList, double)>();

            foreach (ShipmentTempList shipment in shipments)
            {
                double maxSimilarity = Misc.MaxDouble(
                    Misc.JaccardSimilarity($"{shipment.ShipmentID}", searchTerm),
                    Misc.JaccardSimilarity(shipment.From, searchTerm),
                    Misc.JaccardSimilarity(shipment.To, searchTerm),
                    Misc.JaccardSimilarity(shipment.Description ?? "", searchTerm),
                    Misc.JaccardSimilarity(shipment.ShipmentType, searchTerm),
                    Misc.JaccardSimilarity(shipment.Status, searchTerm),
                    Misc.JaccardSimilarity(shipment.UserName, searchTerm),
                    DateSimilarity(shipment.Date, searchTerm)
                );

                shipmentSimilarities.Add((shipment, maxSimilarity));
            }

            List<ShipmentTempList> sortedShipments = shipmentSimilarities
                .OrderByDescending(ss => ss.Item2)
                .Select(ss => ss.Item1)
                .ToList();

            return ConvertShipmentListToDataTable(sortedShipments);
        }

        // Đổi kiểu dữ liệu từ List<ShipmentList> sang DataTable
        private static DataTable ConvertShipmentListToDataTable(List<ShipmentTempList> shipmentList)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Shipment Type", typeof(string));
            dataTable.Columns.Add("Shipment ID", typeof(int));
            dataTable.Columns.Add("From", typeof(string));
            dataTable.Columns.Add("To", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("User Name", typeof(string));
            dataTable.Columns.Add("Date", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));

            foreach (ShipmentTempList shipment in shipmentList)
            {
                dataTable.Rows.Add(
                    shipment.ShipmentType,
                    shipment.ShipmentID,
                    shipment.From,
                    shipment.To,
                    shipment.Status,
                    shipment.UserName,
                    shipment.Date.ToString(), // Chỉnh sửa phần này nếu cần thiết
                    shipment.Description
                );
            }
            return dataTable;
        }

        //Tìm kiếm theo DateTime
        public static double DateSimilarity(DateTime date, string searchTerm)
        {
            if (DateTime.TryParse(searchTerm, out DateTime searchDate))
            {
                int dateDiff = Math.Abs(date.Day - searchDate.Day);
                int monthDiff = Math.Abs(date.Month - searchDate.Month);
                int yearDiff = Math.Abs(date.Year - searchDate.Year);

                double similarity = 1.0 / (1.0 + dateDiff + monthDiff + yearDiff);
                return similarity;
            }
            else
            {
                return 0.0;
            }
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
            List<ShipmentTempList> shipments = ConvertDataTableToList(dataTable);

            switch (columnToSortBy)
            {
                case 0:
                    shipments = shipments.OrderBy(s => s.ShipmentType).ToList();
                    break;
                case 1:
                    shipments = shipments.OrderBy(s => s.ShipmentID).ToList();
                    break;
                case 2:
                    shipments = shipments.OrderBy(s => s.From).ToList();
                    break;
                case 3:
                    shipments = shipments.OrderBy(s => s.To).ToList();
                    break;
                case 4:
                    shipments = shipments.OrderBy(s => s.Status).ToList();
                    break;
                case 5:
                    shipments = shipments.OrderBy(s => s.UserName).ToList();
                    break;
                case 6:
                    shipments = shipments.OrderBy(s => s.Date).ToList();
                    break;
                case 7:
                    shipments = shipments.OrderBy(s => s.Description).ToList();
                    break;
                default:
                    break;
            }

            if (sortColumnInDescendingOrder)
            {
                shipments.Reverse();
            }

            DataTable sortedDataTable = ConvertShipmentListToDataTable(shipments);

            sortedDataTable.Columns[columnToSortBy].ColumnName = Misc.ShowCurrentSortingDirection(sortedDataTable.Columns[columnToSortBy].ColumnName, sortColumnInDescendingOrder);

            return sortedDataTable;
        }

        //Không có Update và xóa ở menu này
    }
}