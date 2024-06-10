using WarehouseManager.Data.Table;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data
{
    class WarehouseDatabase
    {
        private PermissionTable? PermissionTable { get; }
        private UserTable? UserTable { get; }
        private TokenTable? TokenTable { get; }
        private SupplierTable? SupplierTable { get; }
        private CategoryTable? CategoryTable { get; }
        private ProductTable? ProductTable { get; }
        private ProductVariantTable? ProductVariantTable { get; }
        private WarehouseAddressTable? WarehouseAddressTable { get; }
        private WarehouseTable? WarehouseTable { get; }
        private WarehouseStockTable? WarehouseStockTable { get; }
        private InventoryAuditTable? InventoryAuditTable { get; }
        private InventoryAuditDetailTable? InventoryAuditDetailTable { get; }
        private InboundShipmentTable? InboundShipmentTable { get; }
        private InboundShipmentDetailTable? InboundShipmentDetailTable { get; }
        private OutboundShipmentTable? OutboundShipmentTable { get; }
        private OutboundShipmentDetailTable? OutboundShipmentDetailTable { get; }
        private StockTransferTable? StockTransferTable { get; }
        private StockTransferDetailTable? StockTransferDetailTable { get; }

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