namespace WarehouseManager.Data.Entity
{
    class InventoryAudit(int inventoryAuditID, int warehouseID, string inventoryAuditStatus, int userID, DateTime inventoryAuditTime)
    {
        public int InventoryAuditID { get; set; } = inventoryAuditID;
        public int WarehouseID { get; set; } = warehouseID;
        public string InventoryAuditStatus {get; set;} = inventoryAuditStatus;
        public int UserID { get; set; } = userID;
        public DateTime InventoryAuditTime { get; set; } = inventoryAuditTime;
    }
}