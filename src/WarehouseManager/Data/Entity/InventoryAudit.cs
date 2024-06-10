namespace WarehouseManager.Data.Entity
{
    class InventoryAudit(int inventoryAuditID, int warehouseID, int? userID, DateTime inventoryAuditTime)
    {
        public int InventoryAuditID { get; set; } = inventoryAuditID;
        public int WarehouseID { get; set; } = warehouseID;
        public int? UserID { get; set; } = userID;
        public DateTime InventoryAuditTime { get; set; } = inventoryAuditTime;
    }
}