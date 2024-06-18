using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class StockTransferTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;
        public List<StockTransfer>? StockTransfers { get; private set; }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawStockTransfers = Procedure.ExecuteReader(this.ConnectionString, "read_stock_transfer", inParameters);

            List<StockTransfer> stockTransfers = new List<StockTransfer>();
            foreach (List<object?> rawStockTransfer in rawStockTransfers)
            {
                StockTransfer stockTransfer = new StockTransfer(
                    (int)(rawStockTransfer[0] ?? 0),
                    (int)(rawStockTransfer[1] ?? 0),
                    (int)(rawStockTransfer[2] ?? 0),
                    (DateTime?)rawStockTransfer[3],
                    (string)(rawStockTransfer[4] ?? 0),
                    (string?)rawStockTransfer[5],
                    (int?)rawStockTransfer[6]
                );
                stockTransfers.Add(stockTransfer);
            }

            this.StockTransfers = stockTransfers;
        }

        public void Add(int stockTransferID, int fromWarehouseID, int toWarehouseID, DateTime? stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_stock_transfer_id", stockTransferID},
                {"input_from_warehouse_id", fromWarehouseID},
                {"input_to_warehouse_id", toWarehouseID},
                {"input_stock_transfer_starting_date", stockTransferStartingDate},
                {"input_stock_transfer_status", stockTransferStatus},
                {"input_stock_transfer_description", stockTransferDescription},
                {"input_user_id", userID}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_stock_transfer", inParameters);
        }

        public void Update(int stockTransferID, int fromWarehouseID, int toWarehouseID, DateTime? stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_stock_transfer_id", stockTransferID},
                {"input_from_warehouse_id", fromWarehouseID},
                {"input_to_warehouse_id", toWarehouseID},
                {"input_stock_transfer_starting_date", stockTransferStartingDate},
                {"input_stock_transfer_status", stockTransferStatus},
                {"input_stock_transfer_description", stockTransferDescription},
                {"input_user_id", userID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_stock_transfer", inParameters);
        }

        public void Delete(int stockTransferID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_stock_transfer_id", stockTransferID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_stock_transfer", inParameters);
        }
    }
}
