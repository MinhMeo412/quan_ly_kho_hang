using WarehouseManager.Data.Table;

namespace WarehouseManager.Data
{
    class WarehouseDatabase
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

        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        private string connectionString;

        public WarehouseDatabase(string server = "localhost", string user = "root", string password = "1234", string database = "warehouse")
        {
            this.Server = server;
            this.User = user;
            this.Password = password;
            this.Database = database;

            this.connectionString = $"server={this.Server}; user={this.User}; password={this.Password}; database={this.Database}";
        }
    }
}