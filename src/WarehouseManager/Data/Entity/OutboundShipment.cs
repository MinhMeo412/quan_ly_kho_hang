namespace WarehouseManager.Data.Entity
{
    class OutboundShipment
    {
        public int OutboundShipmentID { get; set; }
        public int WarehouseID { get; set; }
        public string OutboundShipmentAddress { get; set; } = "";
        public DateTime? OutboundShipmentStartingDate { get; set; }
        public string OutboundShipmentStatus { get; set; } =  "Processing";
        public string? OutboundShipmentDescription { get; set; }
        public int? UserID { get; set; }
    }
}