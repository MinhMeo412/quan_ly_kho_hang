using WarehouseManager.Data.Table;

namespace WarehouseManager.Data
{
    class Database
    {
        public PermissionTable? PermissionTable { get; }
        public UserTable? UserTable { get; }
        public TokenTable? TokenTable { get; }
        public SupplierTable? SupplierTable { get; }
        public CategoryTable? CategoryTable { get; }
        public ProductTable? ProductTable { get; }
        public ProductVariantTable? ProductVariantTable { get; }
        public WarehouseAddressTable? WarehouseAddressTable { get; }
        public WarehouseTable? WarehouseTable { get; }
        public WarehouseStockTable? WarehouseStockTable { get; }
        public InventoryAuditTable? InventoryAuditTable { get; }
        public InventoryAuditDetailTable? InventoryAuditDetailTable { get; }
        public InboundShipmentTable? InboundShipmentTable { get; }
        public InboundShipmentDetailTable? InboundShipmentDetailTable { get; }
        public OutboundShipmentTable? OutboundShipmentTable { get; }
        public OutboundShipmentDetailTable? OutboundShipmentDetailTable { get; }
        public StockTransferTable? StockTransferTable { get; }
        public StockTransferDetailTable? StockTransferDetailTable { get; }
    }
}