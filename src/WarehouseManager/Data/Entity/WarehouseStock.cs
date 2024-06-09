namespace WarehouseManager.Data.Entity
{
    class WarehouseStock
    {
        public int WarehouseID { get; set; }
        public int ProductVariantID { get; set; }
        public int WarehouseStockQuantity { get; set; }
    }
}