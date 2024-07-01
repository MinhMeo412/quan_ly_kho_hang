namespace WarehouseManager.Data.Entity
{
    class StockTransfer
    {
        public int StockTransferID { get; set; }
        public int FromWarehouseID { get; set; }
        public int ToWarehouseID { get; set; }
        public DateTime? StockTransferStartingDate { get; set; }
        public string StockTransferStatus { get; set; }
        public string? StockTransferDescription { get; set; }
        public int UserID { get; set; }

        public StockTransfer(int stockTransferID, int fromWarehouseID, int toWarehouseID, DateTime? stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, int userID)
        {
            StockTransferID = stockTransferID;
            FromWarehouseID = fromWarehouseID;
            ToWarehouseID = toWarehouseID;
            StockTransferStartingDate = stockTransferStartingDate;
            StockTransferStatus = stockTransferStatus;
            StockTransferDescription = stockTransferDescription;
            UserID = userID;
        }

        //public StockTransfer() { } // Constructor không tham số (nếu cần)
    }
}