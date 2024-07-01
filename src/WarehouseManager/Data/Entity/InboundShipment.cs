namespace WarehouseManager.Data.Entity
{
    public class InboundShipment
    {
        public int InboundShipmentID { get; set; }
        public int SupplierID { get; set; }
        public int WarehouseID { get; set; }
        public DateTime? InboundShipmentStartingDate { get; set; }
        public string InboundShipmentStatus { get; set; }
        public string? InboundShipmentDescription { get; set; }
        public int UserID { get; set; }

        public InboundShipment(int inboundShipmentID, int supplierID, int warehouseID, DateTime? inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, int userID)
        {
            InboundShipmentID = inboundShipmentID;
            SupplierID = supplierID;
            WarehouseID = warehouseID;
            InboundShipmentStartingDate = inboundShipmentStartingDate;
            InboundShipmentStatus = inboundShipmentStatus;
            InboundShipmentDescription = inboundShipmentDescription;
            UserID = userID;
        }

        //public InboundShipment() { } // Constructor không tham số (nếu cần)
    }
}
