using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class WarehouseStockTable
    {
        public List<WarehouseStock>? WarehouseStocks { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawWarehouseStocks = Procedure.ExecuteReader(connectionString, "read_warehouse_stock", inParameters);

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

        public void Add(string connectionString, string token, int warehouseID, int productVariantID, int warehouseStockQuantity)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_warehouse_id", warehouseID},
                {"input_product_variant_id", productVariantID},
                {"input_warehouse_stock_quantity", warehouseStockQuantity}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_warehouse_stock", inParameters);

            WarehouseStock warehouseStock = new WarehouseStock(warehouseID, productVariantID, warehouseStockQuantity);

            this.WarehouseStocks ??= new List<WarehouseStock>();
            this.WarehouseStocks.Add(warehouseStock);
        }

        public void Update(string connectionString, string token, int warehouseID, int productVariantID, int warehouseStockQuantity)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_warehouse_id", warehouseID},
                {"input_product_variant_id", productVariantID},
                {"input_warehouse_stock_quantity", warehouseStockQuantity}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_warehouse_stock", inParameters);

            var warehouseStock = this.WarehouseStocks?.FirstOrDefault(ws => ws.WarehouseID == warehouseID && ws.ProductVariantID == productVariantID);
            if (warehouseStock != null)
            {
                warehouseStock.WarehouseStockQuantity = warehouseStockQuantity;
            }
        }

        public void Delete(string connectionString, string token, int warehouseID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_warehouse_id", warehouseID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_warehouse_stock", inParameters);

            var warehouseStock = this.WarehouseStocks?.FirstOrDefault(ws => ws.WarehouseID == warehouseID && ws.ProductVariantID == productVariantID);
            if (warehouseStock != null)
            {
                this.WarehouseStocks ??= new List<WarehouseStock>();
                this.WarehouseStocks.Remove(warehouseStock);
            }
        }
    }
}
