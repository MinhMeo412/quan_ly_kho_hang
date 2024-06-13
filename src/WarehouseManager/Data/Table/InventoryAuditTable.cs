using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class InventoryAuditTable
    {
        public List<InventoryAudit>? InventoryAudits { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {{"input_token", token}};
            
            List<List<object>> rawInventoryAudits = Procedure.ExecuteReader(connectionString, "read_inventory_audit", inParameters);

            List<InventoryAudit> inventoryAudits = new List<InventoryAudit>();
            foreach (List<object> rawInventoryAudit in rawInventoryAudits)
            {
                InventoryAudit inventoryAudit = new InventoryAudit(
                    (int)rawInventoryAudit[0],    // inventory_audit_id
                    (int)rawInventoryAudit[1],    // warehouse_id
                    (int?)rawInventoryAudit[2],   // user_id (nullable)
                    (DateTime)rawInventoryAudit[3] // inventory_audit_time
                );
                inventoryAudits.Add(inventoryAudit);
            }

            this.InventoryAudits = inventoryAudits;
        }

        public void Add(string connectionString, string token, int inventoryAuditID, int warehouseID, int userID, DateTime inventoryAuditTime)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"warehouse_id",warehouseID},
                {"user_id", userID}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_inventory_audit", inParameters);

            InventoryAudit inventoryAudit = new InventoryAudit(inventoryAuditID, warehouseID, userID, inventoryAuditTime);

            this.InventoryAudits ??= new List<InventoryAudit>();
            this.InventoryAudits.Add(inventoryAudit);
        }
    }
}