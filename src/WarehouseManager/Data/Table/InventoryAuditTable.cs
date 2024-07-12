using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class InventoryAuditTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<InventoryAudit>? _inventoryAudits;
        public List<InventoryAudit>? InventoryAudits
        {
            get
            {
                this.Load();
                return this._inventoryAudits;
            }
            private set
            {
                this._inventoryAudits = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawInventoryAudits = Procedure.ExecuteReader(this.ConnectionString, "read_inventory_audit", inParameters);

            List<InventoryAudit> inventoryAudits = new List<InventoryAudit>();
            foreach (List<object?> rawAudit in rawInventoryAudits)
            {
                InventoryAudit audit = new InventoryAudit(
                    (int)(rawAudit[0] ?? 0),
                    (int)(rawAudit[1] ?? 0),
                    (string)(rawAudit[2] ?? ""),
                    (int)(rawAudit[3] ?? 0),
                    (DateTime)(rawAudit[4] ?? DateTime.Now)
                );
                inventoryAudits.Add(audit);
            }

            this.InventoryAudits = inventoryAudits;
        }

        public void Add(int inventoryAuditID, int warehouseID, string inventoryAuditStatus, int? userID, DateTime inventoryAuditTime)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_warehouse_id", warehouseID},
                {"input_inventory_audit_status", inventoryAuditStatus},
                {"input_user_id", userID},
                {"input_inventory_audit_time", inventoryAuditTime}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_inventory_audit", inParameters);
        }

        public void Update(int inventoryAuditID, int warehouseID, string inventoryAuditStatus, int? userID, DateTime inventoryAuditTime)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_warehouse_id", warehouseID},
                {"input_inventory_audit_status", inventoryAuditStatus},
                {"input_user_id", userID},
                {"input_inventory_audit_time", inventoryAuditTime}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_inventory_audit", inParameters);
        }

        public void Delete(int inventoryAuditID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inventory_audit_id", inventoryAuditID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_inventory_audit", inParameters);
        }
    }
}
