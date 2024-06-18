using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class StockTransferDetailTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<StockTransferDetail>? _stockTransferDetails;
        public List<StockTransferDetail>? StockTransferDetails
        {
            get
            {
                this.Load();
                return this._stockTransferDetails;
            }
            private set
            {
                this._stockTransferDetails = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawStockTransferDetails = Procedure.ExecuteReader(this.ConnectionString, "read_stock_transfer_detail", inParameters);

            List<StockTransferDetail> stockTransferDetails = new List<StockTransferDetail>();
            foreach (List<object?> rawStockTransferDetail in rawStockTransferDetails)
            {
                StockTransferDetail stockTransferDetail = new StockTransferDetail(
                    (int)(rawStockTransferDetail[0] ?? 0),
                    (int)(rawStockTransferDetail[1] ?? 0),
                    (int)(rawStockTransferDetail[2] ?? 0)
                );
                stockTransferDetails.Add(stockTransferDetail);
            }

            this.StockTransferDetails = stockTransferDetails;
        }

        public void Add(int stockTransferID, int productVariantID, int stockTransferDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_stock_transfer_id", stockTransferID},
                {"input_product_variant_id", productVariantID},
                {"input_stock_transfer_detail_amount", stockTransferDetailAmount}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_stock_transfer_detail", inParameters);
        }

        public void Update(int stockTransferID, int productVariantID, int stockTransferDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_stock_transfer_id", stockTransferID},
                {"input_product_variant_id", productVariantID},
                {"input_stock_transfer_detail_amount", stockTransferDetailAmount}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_stock_transfer_detail", inParameters);
        }

        public void Delete(int stockTransferID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_stock_transfer_id", stockTransferID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_stock_transfer_detail", inParameters);
        }
    }
}
