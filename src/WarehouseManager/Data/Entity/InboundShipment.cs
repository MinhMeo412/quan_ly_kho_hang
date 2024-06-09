namespace WarehouseManager.Data.Entity
{
    class InboundShipment
    {
        public int InboundShipmentID { get; set; }
        public int? SupplierID { get; set; }
        public int WarehouseID { get; set; }
        public DateTime? InboundShipmentStartingDate { get; set; }
        public string InboundShipmentStatus { get; set; } = "Processing";
        public string? InboundShipmentDescription { get; set; }
        public int? UserID { get; set; }
    }
}