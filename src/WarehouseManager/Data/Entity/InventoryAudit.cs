namespace WarehouseManager.Data.Entity
{
    class InventoryAudit(int inventoryAuditID, int warehouseID, string inventoryAuditStatus, string? inventoryAuditDescription, int userID, DateTime inventoryAuditTime)
    {
        public int InventoryAuditID { get; set; } = inventoryAuditID;
        public int WarehouseID { get; set; } = warehouseID;
        public string InventoryAuditStatus {get; set;} = inventoryAuditStatus;
        public string? InventoryAuditDescription {get; set;} = inventoryAuditDescription;
        public int UserID { get; set; } = userID;
        public DateTime InventoryAuditTime { get; set; } = inventoryAuditTime;
    }
}