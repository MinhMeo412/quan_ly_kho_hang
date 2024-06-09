namespace WarehouseManager.Data.Entity
{
    class InventoryAuditDetail
    {
        public int InventoryAuditID { get; set; }
        public int ProductVariantID { get; set; }
        public int InventoryAuditDetailActualQuantity { get; set; }
    }
}