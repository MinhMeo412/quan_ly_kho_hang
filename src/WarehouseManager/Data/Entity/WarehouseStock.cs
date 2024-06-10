namespace WarehouseManager.Data.Entity
{
    class WarehouseStock(int warehouseID, int productVariantID, int warehouseStockQuantity)
    {
        public int WarehouseID { get; set; } = warehouseID;
        public int ProductVariantID { get; set; } = productVariantID;
        public int WarehouseStockQuantity { get; set; } = warehouseStockQuantity;
    }
}