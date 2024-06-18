using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class InventoryAuditDetailTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<InventoryAuditDetail>? _inventoryAuditDetails;
        public List<InventoryAuditDetail>? InventoryAuditDetails
        {
            get
            {
                this.Load();
                return this._inventoryAuditDetails;
            }
            private set
            {
                this._inventoryAuditDetails = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawInventoryAuditDetails = Procedure.ExecuteReader(this.ConnectionString, "read_inventory_audit_detail", inParameters);

            List<InventoryAuditDetail> inventoryAuditDetails = new List<InventoryAuditDetail>();
            foreach (List<object?> rawDetail in rawInventoryAuditDetails)
            {
                InventoryAuditDetail detail = new InventoryAuditDetail(
                    (int)(rawDetail[0] ?? 0),
                    (int)(rawDetail[1] ?? 0),
                    (int)(rawDetail[2] ?? 0)
                );
                inventoryAuditDetails.Add(detail);
            }

            this.InventoryAuditDetails = inventoryAuditDetails;
        }

        public void Add(int inventoryAuditID, int productVariantID, int inventoryAuditDetailActualQuantity)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_product_variant_id", productVariantID},
                {"input_inventory_audit_detail_actual_quantity", inventoryAuditDetailActualQuantity}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_inventory_audit_detail", inParameters);
        }

        public void Update(int inventoryAuditID, int productVariantID, int inventoryAuditDetailActualQuantity)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_product_variant_id", productVariantID},
                {"input_inventory_audit_detail_actual_quantity", inventoryAuditDetailActualQuantity}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_inventory_audit_detail", inParameters);
        }

        public void Delete(int inventoryAuditID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_inventory_audit_detail", inParameters);
        }
    }
}
