using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class WarehouseStockTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;
        public List<WarehouseStock>? WarehouseStocks { get; private set; }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawWarehouseStocks = Procedure.ExecuteReader(this.ConnectionString, "read_warehouse_stock", inParameters);

            List<WarehouseStock> warehouseStocks = new List<WarehouseStock>();
            foreach (List<object?> rawWarehouseStock in rawWarehouseStocks)
            {
                WarehouseStock warehouseStock = new WarehouseStock(
                     (int)(rawWarehouseStock[0] ?? 0),
                     (int)(rawWarehouseStock[1] ?? 0),
                     (int)(rawWarehouseStock[2] ?? 0)
                );
                warehouseStocks.Add(warehouseStock);
            }

            this.WarehouseStocks = warehouseStocks;
        }

        public void Add(int warehouseID, int productVariantID, int warehouseStockQuantity)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_warehouse_id", warehouseID},
                {"input_product_variant_id", productVariantID},
                {"input_warehouse_stock_quantity", warehouseStockQuantity}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_warehouse_stock", inParameters);
        }

        public void Update(int warehouseID, int productVariantID, int warehouseStockQuantity)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_warehouse_id", warehouseID},
                {"input_product_variant_id", productVariantID},
                {"input_warehouse_stock_quantity", warehouseStockQuantity}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_warehouse_stock", inParameters);
        }

        public void Delete(int warehouseID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_warehouse_id", warehouseID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_warehouse_stock", inParameters);
        }
    }
}
