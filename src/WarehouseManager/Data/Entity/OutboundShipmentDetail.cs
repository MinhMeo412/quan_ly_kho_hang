namespace WarehouseManager.Data.Entity
{
    class OutboundShipmentDetail(int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
    {
        public int OutboundShipmentID { get; set; } = outboundShipmentID;
        public int ProductVariantID { get; set; } = productVariantID;
        public int OutboundShipmentDetailAmount { get; set; } = outboundShipmentDetailAmount;
    }
}