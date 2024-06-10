namespace WarehouseManager.Data.Entity
{
    class InboundShipmentDetail(int inboundShipmentID, int productVariantID, int inboundShipmentDetailAmount)
    {
        public int InboundShipmentID { get; set; } = inboundShipmentID;
        public int ProductVariantID { get; set; } = productVariantID;
        public int InboundShipmentDetailAmount { get; set; } = inboundShipmentDetailAmount;
    }
}