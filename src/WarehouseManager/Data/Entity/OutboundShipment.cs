namespace WarehouseManager.Data.Entity
{
    class OutboundShipment(int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int? userID)
    {
        public int OutboundShipmentID { get; set; } = outboundShipmentID;
        public int WarehouseID { get; set; } = warehouseID;
        public string OutboundShipmentAddress { get; set; } = outboundShipmentAddress;
        public DateTime? OutboundShipmentStartingDate { get; set; } = outboundShipmentStartingDate;
        public string OutboundShipmentStatus { get; set; } = outboundShipmentStatus;
        public string? OutboundShipmentDescription { get; set; } = outboundShipmentDescription;
        public int? UserID { get; set; } = userID;
    }
}