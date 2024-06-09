namespace WarehouseManager.Data.Entity
{
    class StockTransfer
    {
        public int StockTransferID { get; set; }
        public int FromWarehouseID { get; set; }
        public int ToWarehouseID { get; set; }
        public DateTime? StockTransferStartingDate { get; set; }
        public string StockTransferStatus { get; set; } = "Processing";
        public string? StockTransferDescription { get; set; }
        public int? UserID { get; set; }
    }
}