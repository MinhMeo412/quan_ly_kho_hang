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

        public void Update(string connectionString, string token, int inventoryAuditID, int warehouseID, int userID, DateTime inventoryAuditTime)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"inventory_audit_id",inventoryAuditID},
                {"new_warehouse_id", warehouseID},
                {"new_user_id", userID},
                {"new_inventory_audit_time", inventoryAuditTime}
            };
            Procedure.ExecuteNonQuery(connectionString, "update_inventory_audit", inParameters);

            var inventoryAudit = this.InventoryAudits?.FirstOrDefault(temp => temp.InventoryAuditID == inventoryAuditID);
            if (inventoryAudit != null)
            {
                inventoryAudit.WarehouseID = warehouseID;
                inventoryAudit.UserID = userID;
                inventoryAudit.InventoryAuditTime = inventoryAuditTime;
            }
        }

        public void Delete(string connectionString, string token, int inventoryAuditID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"target_audit_id",inventoryAuditID}
            };
            Procedure.ExecuteNonQuery(connectionString, "delete_inventory_audit", inParameters);

            var inventoryAudit = this.InventoryAudits?.FirstOrDefault(temp => temp.InventoryAuditID == inventoryAuditID);
            if (inventoryAudit != null)
            {
                this.InventoryAudits ??= new List<InventoryAudit>();
                this.InventoryAudits.Remove(inventoryAudit);
            }
        }
    }
}