using WarehouseManager.Data.Table;
using WarehouseManager.Data.Utility;

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
        private string? token;

        public WarehouseDatabase(string server = "localhost", string user = "root", string password = "", string database = "warehouse")
        {
            this.Server = server;
            this.User = user;
            this.Password = password;
            this.Database = database;

            this.connectionString = $"server={this.Server}; user={this.User}; password={this.Password}; database={this.Database}";
        }

        public bool Login(string username, string password)
        {
            Dictionary<string, object> inParameters = new Dictionary<string, object>{
                {"inputted_username", username},
                {"inputted_password", password}
            };
            List<string> outParameters = new List<string>
            {
                "success",
                "token"
            };

            Dictionary<string, object> output = Procedure.ExecuteNonQuery(this.connectionString, "user_login", inParameters, outParameters);
            this.token = $"{output["token"]}";

            bool loggedIn = int.Parse($"{output["success"]}") == 1;
            return loggedIn;
        }
    }
}