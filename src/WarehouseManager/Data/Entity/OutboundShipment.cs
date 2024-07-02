namespace WarehouseManager.Data.Entity
{
    class OutboundShipment
    {
        public int OutboundShipmentID { get; set; }
        public int WarehouseID { get; set; }
        public string OutboundShipmentAddress { get; set; }
        public DateTime? OutboundShipmentStartingDate { get; set; }
        public string OutboundShipmentStatus { get; set; }
        public string? OutboundShipmentDescription { get; set; }
        public int UserID { get; set; }

        public OutboundShipment(int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int userID)
        {
            OutboundShipmentID = outboundShipmentID;
            WarehouseID = warehouseID;
            OutboundShipmentAddress = outboundShipmentAddress;
            OutboundShipmentStartingDate = outboundShipmentStartingDate;
            OutboundShipmentStatus = outboundShipmentStatus;
            OutboundShipmentDescription = outboundShipmentDescription;
            UserID = userID;
        }

        //public OutboundShipment() { } // Constructor không tham số (nếu cần)
    }
}

