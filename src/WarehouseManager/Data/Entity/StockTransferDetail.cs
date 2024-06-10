namespace WarehouseManager.Data.Entity
{
    class StockTransferDetail(int stockTransferID, int productVariantID, int stockTransferDetailAmount)
    {
        public int StockTransferID { get; set; } = stockTransferID;
        public int ProductVariantID { get; set; } = productVariantID;
        public int StockTransferDetailAmount { get; set; } = stockTransferDetailAmount;
    }
}