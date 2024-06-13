using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class InventoryAuditDetailTable
    {
        public List<InventoryAuditDetail>? InventoryAuditDetails { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {{"input_token", token}};
            
            List<List<object>> rawInventoryAuditDetails = Procedure.ExecuteReader(connectionString, "read_inventory_audit_detail", inParameters);

            List<InventoryAuditDetail> inventoryAuditDetails = new List<InventoryAuditDetail>();
            foreach (List<object> rawInventoryAuditDetail in rawInventoryAuditDetails)
            {
                InventoryAuditDetail inventoryAuditDetail = new InventoryAuditDetail(
                    (int)rawInventoryAuditDetail[0], // inventory_audit_id
                    (int)rawInventoryAuditDetail[1], // product_variant_id
                    (int)rawInventoryAuditDetail[2]  // inventory_audit_detail_actual_quantity
                );
                inventoryAuditDetails.Add(inventoryAuditDetail);
            }

            this.InventoryAuditDetails = inventoryAuditDetails;
        }

        public void Add(string connectionString, string token, int inventoryAuditID, int productVariantID, int inventoryAuditDetailActualQuantity);
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"inventory_audit_id", inventoryAuditID},
                {"product_variant_id", productVariantID},
                {"inventory_audit_detail_actual_quantity", inventoryAuditDetailActualQuantity}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_inventory_audit_detail", inParameters);

            InventoryAuditDetail inventoryAuditDetail = new InventoryAuditDetail(inventoryAuditID, productVariantID, inventoryAuditDetailActualQuantity);
            
            this.InventoryAuditDetail ??= new List<InventoryAuditDetail>();
            this.InventoryAuditDetail.Add(InventoryAuditDetail);
        }
    }
}