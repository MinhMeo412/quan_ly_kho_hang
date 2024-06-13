using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class WarehouseStockTable
    {
        public List<WarehouseStock>? WarehouseStocks { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {{"input_token", token}};
            
            List<List<object>> rawWarehouseStocks = Procedure.ExecuteReader(connectionString, "read_warehouse_stock", inParameters);

            List<WarehouseStock> warehouseStocks = new List<WarehouseStock>();
            foreach (List<object> rawWarehouseStock in rawWarehouseStocks)
            {
                WarehouseStock warehouseStock = new WarehouseStock(
                    (int)rawWarehouseStock[0],  // warehouse_id
                    (int)rawWarehouseStock[1],  // product_variant_id
                    (int)rawWarehouseStock[2]   // warehouse_stock_quantity
                );
                warehouseStocks.Add(warehouseStock);
            }

            this.WarehouseStocks = warehouseStocks;
        }

        public void Add(string connectionString, string token, int warehouseID, int productVariantID, int warehouseStockQuantity)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"warehouse_id", warehouseID},
                {"product_variant_id", productVariantID},
                {"warehouse_stock_quantity", warehouseStockQuantity}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_warehouse_stock", inParameters);

            WarehouseStock warehouseStock = new WarehouseStock(warehouseID,productVariantID,warehouseStockQuantity);

            this.WarehouseStocks ??= new List<WarehouseStock>();
            this.WarehouseStocks.Add(warehouseStock);
        }

    }
}