using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class InventoryAuditTable
    {
        public List<InventoryAudit>? InventoryAudits { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawInventoryAudits = Procedure.ExecuteReader(connectionString, "read_inventory_audit", inParameters);

            List<InventoryAudit> inventoryAudits = new List<InventoryAudit>();
            foreach (List<object?> rawAudit in rawInventoryAudits)
            {
                InventoryAudit audit = new InventoryAudit(
                    (int)(rawAudit[0] ?? 0),
                    (int)(rawAudit[1] ?? 0),
                    (int?)rawAudit[2],
                    (DateTime)(rawAudit[3] ?? DateTime.Now)
                );
                inventoryAudits.Add(audit);
            }

            this.InventoryAudits = inventoryAudits;
        }

        public void Add(string connectionString, string token, int inventoryAuditID, int warehouseID, int? userID, DateTime inventoryAuditTime)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_warehouse_id", warehouseID},
                {"input_user_id", userID},
                {"input_inventory_audit_time", inventoryAuditTime}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_inventory_audit", inParameters);

            InventoryAudit audit = new InventoryAudit(
                inventoryAuditID, warehouseID, userID, inventoryAuditTime);

            this.InventoryAudits ??= new List<InventoryAudit>();
            this.InventoryAudits.Add(audit);
        }

        public void Update(string connectionString, string token, int inventoryAuditID, int warehouseID, int? userID, DateTime inventoryAuditTime)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_warehouse_id", warehouseID},
                {"input_user_id", userID},
                {"input_inventory_audit_time", inventoryAuditTime}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_inventory_audit", inParameters);

            var audit = this.InventoryAudits?.FirstOrDefault(a => a.InventoryAuditID == inventoryAuditID);
            if (audit != null)
            {
                audit.WarehouseID = warehouseID;
                audit.UserID = userID;
                audit.InventoryAuditTime = inventoryAuditTime;
            }
        }

        public void Delete(string connectionString, string token, int inventoryAuditID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inventory_audit_id", inventoryAuditID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_inventory_audit", inParameters);

            var audit = this.InventoryAudits?.FirstOrDefault(a => a.InventoryAuditID == inventoryAuditID);
            if (audit != null)
            {
                this.InventoryAudits ??= new List<InventoryAudit>();
                this.InventoryAudits.Remove(audit);
            }
        }
    }
}
