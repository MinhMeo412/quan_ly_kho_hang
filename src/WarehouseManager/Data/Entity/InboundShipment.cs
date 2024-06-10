namespace WarehouseManager.Data.Entity
{
    class InboundShipment(int inboundShipmentID, int? supplierID, int warehouseID, DateTime? inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, int? userID)
    {
        public int InboundShipmentID { get; set; } = inboundShipmentID;
        public int? SupplierID { get; set; } = supplierID;
        public int WarehouseID { get; set; } = warehouseID;
        public DateTime? InboundShipmentStartingDate { get; set; } = inboundShipmentStartingDate;
        public string InboundShipmentStatus { get; set; } = inboundShipmentStatus;
        public string? InboundShipmentDescription { get; set; } = inboundShipmentDescription;
        public int? UserID { get; set; } = userID;
    }
}