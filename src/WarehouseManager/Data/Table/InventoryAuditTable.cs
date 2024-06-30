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
                    rawAudit[2] is DBNull ? null : (int?)rawAudit[2],
                    (DateTime)(rawAudit[3] ?? DateTime.Now)
                );
                inventoryAudits.Add(audit);
            }

            this.InventoryAudits = inventoryAudits;
        }

        public void Add(int inventoryAuditID, int warehouseID, int? userID, DateTime inventoryAuditTime)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_warehouse_id", warehouseID},
                {"input_user_id", userID},
                {"input_inventory_audit_time", inventoryAuditTime}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_inventory_audit", inParameters);
        }

        public void Update(int inventoryAuditID, int warehouseID, int? userID, DateTime inventoryAuditTime)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_warehouse_id", warehouseID},
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
