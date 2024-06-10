namespace WarehouseManager.Data.Entity
{
    class StockTransfer(int stockTransferID, int fromWarehouseID, int toWarehouseID, DateTime? stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, int? userID)
    {
        public int StockTransferID { get; set; } = stockTransferID;
        public int FromWarehouseID { get; set; } = fromWarehouseID;
        public int ToWarehouseID { get; set; } = toWarehouseID;
        public DateTime? StockTransferStartingDate { get; set; } = stockTransferStartingDate;
        public string StockTransferStatus { get; set; } = stockTransferStatus;
        public string? StockTransferDescription { get; set; } = stockTransferDescription;
        public int? UserID { get; set; } = userID;
    }
}