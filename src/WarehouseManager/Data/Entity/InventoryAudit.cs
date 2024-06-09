namespace WarehouseManager.Data.Entity
{
    class InventoryAudit
    {
        public int InventoryAuditID { get; set; }
        public int WarehouseID { get; set; }
        public int? UserID { get; set; }
        public DateTime InventoryAuditTime { get; set; }
    }
}