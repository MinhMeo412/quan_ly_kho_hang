using WarehouseManager.Data.Table;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data
{
    class WarehouseDatabase
    {
        public PermissionTable PermissionTable { get; private set; }
        public UserTable UserTable { get; private set; }
        public TokenTable TokenTable { get; private set; }
        public SupplierTable SupplierTable { get; private set; }
        public CategoryTable CategoryTable { get; private set; }
        public ProductTable ProductTable { get; private set; }
        public ProductVariantTable ProductVariantTable { get; private set; }
        public WarehouseAddressTable WarehouseAddressTable { get; private set; }
        public WarehouseTable WarehouseTable { get; private set; }
        public WarehouseStockTable WarehouseStockTable { get; private set; }
        public InventoryAuditTable InventoryAuditTable { get; private set; }
        public InventoryAuditDetailTable InventoryAuditDetailTable { get; private set; }
        public InboundShipmentTable InboundShipmentTable { get; private set; }
        public InboundShipmentDetailTable InboundShipmentDetailTable { get; private set; }
        public OutboundShipmentTable OutboundShipmentTable { get; private set; }
        public OutboundShipmentDetailTable OutboundShipmentDetailTable { get; private set; }
        public StockTransferTable StockTransferTable { get; private set; }
        public StockTransferDetailTable StockTransferDetailTable { get; private set; }

        private string ConnectionString;
        private string? Token;
        public string? Username;
        public int? PermissionLevel;

        public WarehouseDatabase(string server = "localhost", string user = "root", string password = "", string database = "warehouse")
        {
            this.ConnectionString = $"server={server}; user={user}; password={password}; database={database}";

            this.PermissionTable = new PermissionTable(this.ConnectionString, this.Token);
            this.UserTable = new UserTable(this.ConnectionString, this.Token);
            this.TokenTable = new TokenTable(this.ConnectionString, this.Token);
            this.SupplierTable = new SupplierTable(this.ConnectionString, this.Token);
            this.CategoryTable = new CategoryTable(this.ConnectionString, this.Token);
            this.ProductTable = new ProductTable(this.ConnectionString, this.Token);
            this.ProductVariantTable = new ProductVariantTable(this.ConnectionString, this.Token);
            this.WarehouseAddressTable = new WarehouseAddressTable(this.ConnectionString, this.Token);
            this.WarehouseTable = new WarehouseTable(this.ConnectionString, this.Token);
            this.WarehouseStockTable = new WarehouseStockTable(this.ConnectionString, this.Token);
            this.InventoryAuditTable = new InventoryAuditTable(this.ConnectionString, this.Token);
            this.InventoryAuditDetailTable = new InventoryAuditDetailTable(this.ConnectionString, this.Token);
            this.InboundShipmentTable = new InboundShipmentTable(this.ConnectionString, this.Token);
            this.InboundShipmentDetailTable = new InboundShipmentDetailTable(this.ConnectionString, this.Token);
            this.OutboundShipmentTable = new OutboundShipmentTable(this.ConnectionString, this.Token);
            this.OutboundShipmentDetailTable = new OutboundShipmentDetailTable(this.ConnectionString, this.Token);
            this.StockTransferTable = new StockTransferTable(this.ConnectionString, this.Token);
            this.StockTransferDetailTable = new StockTransferDetailTable(this.ConnectionString, this.Token);
        }

        public bool Login(string username, string password)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"inputted_username", username},
                {"inputted_password", password}
            };
            List<string> outParameters = new List<string>
            {
                "success",
                "token",
                "permission_level"
            };

            Dictionary<string, object?> output = Procedure.ExecuteNonQuery(this.ConnectionString, "user_login", inParameters, outParameters);

            bool success = int.Parse($"{output["success"]}") == 1;

            if (success)
            {
                this.Token = $"{output["token"]}";
                this.Username = username;
                this.PermissionLevel = int.Parse($"{output["permission_level"]}");
                this.Initialize();
            }

            return success;
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"inputted_username", this.Username},
                {"old_password", oldPassword},
                {"new_password", newPassword}
            };
            List<string> outParameters = new List<string>
            {
                "success",
                "token"
            };

            Dictionary<string, object?> output = Procedure.ExecuteNonQuery(this.ConnectionString, "change_password", inParameters, outParameters);
            bool success = int.Parse($"{output["success"]}") == 1;

            if (success)
            {
                this.Token = $"{output["token"]}";
                this.Initialize();
            }

            return success;
        }

        private void Initialize()
        {
            this.PermissionTable = new PermissionTable(this.ConnectionString, this.Token);
            this.UserTable = new UserTable(this.ConnectionString, this.Token);
            this.TokenTable = new TokenTable(this.ConnectionString, this.Token);
            this.SupplierTable = new SupplierTable(this.ConnectionString, this.Token);
            this.CategoryTable = new CategoryTable(this.ConnectionString, this.Token);
            this.ProductTable = new ProductTable(this.ConnectionString, this.Token);
            this.ProductVariantTable = new ProductVariantTable(this.ConnectionString, this.Token);
            this.WarehouseAddressTable = new WarehouseAddressTable(this.ConnectionString, this.Token);
            this.WarehouseTable = new WarehouseTable(this.ConnectionString, this.Token);
            this.WarehouseStockTable = new WarehouseStockTable(this.ConnectionString, this.Token);
            this.InventoryAuditTable = new InventoryAuditTable(this.ConnectionString, this.Token);
            this.InventoryAuditDetailTable = new InventoryAuditDetailTable(this.ConnectionString, this.Token);
            this.InboundShipmentTable = new InboundShipmentTable(this.ConnectionString, this.Token);
            this.InboundShipmentDetailTable = new InboundShipmentDetailTable(this.ConnectionString, this.Token);
            this.OutboundShipmentTable = new OutboundShipmentTable(this.ConnectionString, this.Token);
            this.OutboundShipmentDetailTable = new OutboundShipmentDetailTable(this.ConnectionString, this.Token);
            this.StockTransferTable = new StockTransferTable(this.ConnectionString, this.Token);
            this.StockTransferDetailTable = new StockTransferDetailTable(this.ConnectionString, this.Token);
        }
    }
}