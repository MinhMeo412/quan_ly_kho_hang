using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class InventoryAuditDetailTable
    {
        public List<InventoryAuditDetail>? InventoryAuditDetails { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawInventoryAuditDetails = Procedure.ExecuteReader(connectionString, "read_inventory_audit_detail", inParameters);

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

        public void Add(string connectionString, string token, int inventoryAuditID, int productVariantID, int inventoryAuditDetailActualQuantity)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_product_variant_id", productVariantID},
                {"input_inventory_audit_detail_actual_quantity", inventoryAuditDetailActualQuantity}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_inventory_audit_detail", inParameters);

            InventoryAuditDetail detail = new InventoryAuditDetail(
                inventoryAuditID, productVariantID, inventoryAuditDetailActualQuantity);

            this.InventoryAuditDetails ??= new List<InventoryAuditDetail>();
            this.InventoryAuditDetails.Add(detail);
        }

        public void Update(string connectionString, string token, int inventoryAuditID, int productVariantID, int inventoryAuditDetailActualQuantity)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_product_variant_id", productVariantID},
                {"input_inventory_audit_detail_actual_quantity", inventoryAuditDetailActualQuantity}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_inventory_audit_detail", inParameters);

            var detail = this.InventoryAuditDetails?.FirstOrDefault(d => d.InventoryAuditID == inventoryAuditID && d.ProductVariantID == productVariantID);
            if (detail != null)
            {
                detail.InventoryAuditDetailActualQuantity = inventoryAuditDetailActualQuantity;
            }
        }

        public void Delete(string connectionString, string token, int inventoryAuditID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inventory_audit_id", inventoryAuditID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_inventory_audit_detail", inParameters);

            var detail = this.InventoryAuditDetails?.FirstOrDefault(d => d.InventoryAuditID == inventoryAuditID && d.ProductVariantID == productVariantID);
            if (detail != null)
            {
                this.InventoryAuditDetails ??= new List<InventoryAuditDetail>();
                this.InventoryAuditDetails.Remove(detail);
            }
        }
    }
}
