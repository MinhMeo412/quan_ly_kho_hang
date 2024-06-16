using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class StockTransferDetailTable
    {
        public List<StockTransferDetail>? StockTransferDetails { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawStockTransferDetails = Procedure.ExecuteReader(connectionString, "read_stock_transfer_detail", inParameters);

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

        public void Add(string connectionString, string token, int stockTransferID, int productVariantID, int stockTransferDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_stock_transfer_id", stockTransferID},
                {"input_product_variant_id", productVariantID},
                {"input_stock_transfer_detail_amount", stockTransferDetailAmount}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_stock_transfer_detail", inParameters);

            StockTransferDetail stockTransferDetail = new StockTransferDetail(stockTransferID, productVariantID, stockTransferDetailAmount);

            this.StockTransferDetails ??= new List<StockTransferDetail>();
            this.StockTransferDetails.Add(stockTransferDetail);
        }

        public void Update(string connectionString, string token, int stockTransferID, int productVariantID, int stockTransferDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_stock_transfer_id", stockTransferID},
                {"input_product_variant_id", productVariantID},
                {"input_stock_transfer_detail_amount", stockTransferDetailAmount}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_stock_transfer_detail", inParameters);

            var stockTransferDetail = this.StockTransferDetails?.FirstOrDefault(std => std.StockTransferID == stockTransferID && std.ProductVariantID == productVariantID);
            if (stockTransferDetail != null)
            {
                stockTransferDetail.StockTransferDetailAmount = stockTransferDetailAmount;
            }
        }

        public void Delete(string connectionString, string token, int stockTransferID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_stock_transfer_id", stockTransferID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_stock_transfer_detail", inParameters);

            var stockTransferDetail = this.StockTransferDetails?.FirstOrDefault(std => std.StockTransferID == stockTransferID && std.ProductVariantID == productVariantID);
            if (stockTransferDetail != null)
            {
                this.StockTransferDetails ??= new List<StockTransferDetail>();
                this.StockTransferDetails.Remove(stockTransferDetail);
            }
        }
    }
}
