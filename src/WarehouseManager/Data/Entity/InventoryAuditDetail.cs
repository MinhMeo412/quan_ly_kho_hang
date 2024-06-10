namespace WarehouseManager.Data.Entity
{
    class InventoryAuditDetail(int inventoryAuditID, int productVariantID, int inventoryAuditDetailActualQuantity)
    {
        public int InventoryAuditID { get; set; } = inventoryAuditID;
        public int ProductVariantID { get; set; } = productVariantID;
        public int InventoryAuditDetailActualQuantity { get; set; } = inventoryAuditDetailActualQuantity;
    }
}