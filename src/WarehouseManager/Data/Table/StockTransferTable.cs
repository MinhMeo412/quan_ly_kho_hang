using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class StockTransferTable
    {
        public List<StockTransfer>? StockTransfers { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {{"input_token", token}};
            
            List<List<object>> rawStockTransfers = Procedure.ExecuteReader(connectionString, "read_stock_transfer", inParameters);

            List<StockTransfer> stockTransfers = new List<StockTransfer>();
            foreach (List<object> rawStockTransfer in rawStockTransfers)
            {
                StockTransfer stockTransfer = new StockTransfer(
                    (int)rawStockTransfer[0],    // stock_transfer_id
                    (int)rawStockTransfer[1],    // from_warehouse_id
                    (int)rawStockTransfer[2],    // to_warehouse_id
                    (DateTime)rawStockTransfer[3],// stock_transfer_starting_date
                    (string)rawStockTransfer[4], // stock_transfer_status
                    (string)rawStockTransfer[5], // stock_transfer_description
                    (int?)rawStockTransfer[6]    // user_id (nullable)
                );
                stockTransfers.Add(stockTransfer);
            }

            this.StockTransfers = stockTransfers;
        }

        public void Add(string connectionString, string token, int stockTransferID, int fromWarehouseID, int toWarehouseID, DateTime? stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, int? userID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"from_warehouse_id", fromWarehouseID},
                {"to_warehouse_id", toWarehouseID},
                {"stock_transfer_description", stockTransferDescription},
                {"user_id", userID}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_stock_transfer", inParameters);

            StockTransfer stockTransfer = new StockTransfer(stockTransferID, fromWarehouseID, toWarehouseID, stockTransferStartingDate, stockTransferStatus, stockTransferDescription, userID);

            this.StockTransfers ??= new List<StockTransfer>();
            this.StockTransfers.Add(stockTransfer);
        }

        public void Update(string connectionString, string token, int stockTransferID, int fromWarehouseID, int toWarehouseID, DateTime? stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, int? userID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"stock_transfer_id", stockTransferID},
                {"new_from_warehouse_id", fromWarehouseID},
                {"new_to_warehouse_id", toWarehouseID},
                {"new_stock_transfer_starting_date", stockTransferStartingDate},
                {"new_stock_transfer_status", stockTransferStatus},
                {"new_stock_transfer_description", stockTransferDescription},
                {"new_user_id", userID}
            };
            Procedure.ExecuteNonQuery(connectionString, "update_stock_transfer", inParameters);

            var stockTransfer = this.StockTransfers?.FirstOrDefault(temp => temp.StockTransferID == stockTransferID);
            if (stockTransfer != null)
            {
                stockTransfer.FromWarehouseID = fromWarehouseID;
                stockTransfer.ToWarehouseID = toWarehouseID;
                stockTransfer.StockTransferStartingDate = stockTransferStartingDate;
                stockTransfer.StockTransferStatus = stockTransferStatus;
                stockTransfer.StockTransferDescription = stockTransferDescription;
                stockTransfer.UserID = userID;
            }
        }

        public void Delete(string connectionString, string token, int stockTransferID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"target_transfer_id", stockTransferID}
            };
            Procedure.ExecuteNonQuery(connectionString, "delete_stock_transfer", inParameters);

            var stockTransfer = this.StockTransfers?.FirstOrDefault(temp => temp.StockTransferID == stockTransferID);
            if (stockTransfer != null)
            {
                this.StockTransfers ??= new List<StockTransfer>();
                this.StockTransfers.Remove(stockTransfer);
            }
        }
    }
}