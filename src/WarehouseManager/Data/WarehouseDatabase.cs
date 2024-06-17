using WarehouseManager.Data.Table;
using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data
{
    class WarehouseDatabase
    {
        private PermissionTable PermissionTable { get; }
        private UserTable UserTable { get; }
        private TokenTable TokenTable { get; }
        private SupplierTable SupplierTable { get; }
        private CategoryTable CategoryTable { get; }
        private ProductTable ProductTable { get; }
        private ProductVariantTable ProductVariantTable { get; }
        private WarehouseAddressTable WarehouseAddressTable { get; }
        private WarehouseTable WarehouseTable { get; }
        private WarehouseStockTable WarehouseStockTable { get; }
        private InventoryAuditTable InventoryAuditTable { get; }
        private InventoryAuditDetailTable InventoryAuditDetailTable { get; }
        private InboundShipmentTable InboundShipmentTable { get; }
        private InboundShipmentDetailTable InboundShipmentDetailTable { get; }
        private OutboundShipmentTable OutboundShipmentTable { get; }
        private OutboundShipmentDetailTable OutboundShipmentDetailTable { get; }
        private StockTransferTable StockTransferTable { get; }
        private StockTransferDetailTable StockTransferDetailTable { get; }

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

            this.PermissionTable = new PermissionTable();
            this.UserTable = new UserTable();
            this.TokenTable = new TokenTable();
            this.SupplierTable = new SupplierTable();
            this.CategoryTable = new CategoryTable();
            this.ProductTable = new ProductTable();
            this.ProductVariantTable = new ProductVariantTable();
            this.WarehouseAddressTable = new WarehouseAddressTable();
            this.WarehouseTable = new WarehouseTable();
            this.WarehouseStockTable = new WarehouseStockTable();
            this.InventoryAuditTable = new InventoryAuditTable();
            this.InventoryAuditDetailTable = new InventoryAuditDetailTable();
            this.InboundShipmentTable = new InboundShipmentTable();
            this.InboundShipmentDetailTable = new InboundShipmentDetailTable();
            this.OutboundShipmentTable = new OutboundShipmentTable();
            this.OutboundShipmentDetailTable = new OutboundShipmentDetailTable();
            this.StockTransferTable = new StockTransferTable();
            this.StockTransferDetailTable = new StockTransferDetailTable();
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
                "token"
            };

            Dictionary<string, object?> output = Procedure.ExecuteNonQuery(this.connectionString, "user_login", inParameters, outParameters);

            bool success = int.Parse($"{output["success"]}") == 1;

            if (success)
            {
                this.token = $"{output["token"]}";
                this.Initialize();
            }

            return success;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"inputted_username", username},
                {"old_password", oldPassword},
                {"new_password", newPassword}
            };
            List<string> outParameters = new List<string>
            {
                "success",
                "token"
            };

            Dictionary<string, object?> output = Procedure.ExecuteNonQuery(this.connectionString, "change_password", inParameters, outParameters);
            bool success = int.Parse($"{output["success"]}") == 1;

            if (success)
            {
                this.token = $"{output["token"]}";
            }

            return success;
        }

        private void Initialize()
        {
            if (this.token == null)
            {
                return;
            }

            try
            {
                this.PermissionTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.UserTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.TokenTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.SupplierTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.CategoryTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.ProductTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.ProductVariantTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.WarehouseAddressTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.WarehouseTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.WarehouseStockTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.InventoryAuditTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.InventoryAuditDetailTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.InboundShipmentTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.InboundShipmentDetailTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.OutboundShipmentTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.OutboundShipmentDetailTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.StockTransferTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }

            try
            {
                this.StockTransferDetailTable.Load(this.connectionString, this.token);
            }
            catch (Exception)
            {
            }
        }

        public List<Permission>? GetPermissions()
        {
            return this.PermissionTable.Permissions;
        }

        public void AddPermission(int permissionLevel, string permissionName, string permissionDescription)
        {
            if (this.token != null)
            {
                this.PermissionTable.Add(this.connectionString, this.token, permissionLevel, permissionName, permissionDescription);
            }
        }

        public void UpdatePermission(int permissionLevel, string permissionName, string permissionDescription)
        {
            if (this.token != null)
            {
                this.PermissionTable.Update(this.connectionString, this.token, permissionLevel, permissionName, permissionDescription);
            }
        }

        public void DeletePermission(int permissionLevel)
        {
            if (this.token != null)
            {
                this.PermissionTable.Delete(this.connectionString, this.token, permissionLevel);
            }
        }

        public List<User>? GetUsers()
        {
            return this.UserTable.Users;
        }

        public void AddUser(int userID, string userName, string userPassword, string userFullName, string userEmail, string userPhoneNumber, int permissionLevel)
        {
            if (this.token != null)
            {
                this.UserTable.Add(this.connectionString, this.token, userID, userName, userPassword, userFullName, userEmail, userPhoneNumber, permissionLevel);
            }
        }

        public void UpdateUser(int userID, string userName, string userPassword, string userFullName, string userEmail, string userPhoneNumber, int permissionLevel)
        {
            if (this.token != null)
            {
                this.UserTable.Update(this.connectionString, this.token, userID, userName, userPassword, userFullName, userEmail, userPhoneNumber, permissionLevel);
            }
        }

        public void DeleteUser(int userID)
        {
            if (this.token != null)
            {
                this.UserTable.Delete(this.connectionString, this.token, userID);
            }
        }

        public List<Token>? GetTokens()
        {
            return this.TokenTable.Tokens;  // Assuming TokenTable has a property for Tokens
        }

        public void AddToken(string tokenUUID, int userID, DateTime tokenLastActivityTimeStamp)
        {
            if (this.token != null)
            {
                this.TokenTable.Add(this.connectionString, this.token, tokenUUID, userID, tokenLastActivityTimeStamp);
            }
        }

        public void UpdateToken(string tokenUUID, int userID, DateTime tokenLastActivityTimeStamp)
        {
            if (this.token != null)
            {
                this.TokenTable.Update(this.connectionString, this.token, tokenUUID, userID, tokenLastActivityTimeStamp);
            }
        }

        public void DeleteToken(string tokenUUID)
        {
            if (this.token != null)
            {
                this.TokenTable.Delete(this.connectionString, this.token, tokenUUID);
            }
        }

        public List<Supplier>? GetSuppliers()
        {
            return this.SupplierTable.Suppliers;
        }

        public void AddSupplier(int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            if (this.token != null)
            {
                this.SupplierTable.Add(this.connectionString, this.token, supplierID, supplierName, supplierDescription, supplierAddress, supplierEmail, supplierPhoneNumber, supplierWebsite);
            }
        }

        public void UpdateSupplier(int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            if (this.token != null)
            {
                this.SupplierTable.Update(this.connectionString, this.token, supplierID, supplierName, supplierDescription, supplierAddress, supplierEmail, supplierPhoneNumber, supplierWebsite);
            }
        }

        public void DeleteSupplier(int supplierID)
        {
            if (this.token != null)
            {
                this.SupplierTable.Delete(this.connectionString, this.token, supplierID);
            }
        }


        public List<Category>? GetCategories()
        {
            return this.CategoryTable.Categories;
        }

        public void AddCategory(int categoryID, string categoryName, string categoryDescription)
        {
            if (this.token != null)
            {
                this.CategoryTable.Add(this.connectionString, this.token, categoryID, categoryName, categoryDescription);
            }
        }

        public void UpdateCategory(int categoryID, string categoryName, string categoryDescription)
        {
            if (this.token != null)
            {
                this.CategoryTable.Update(this.connectionString, this.token, categoryID, categoryName, categoryDescription);
            }
        }

        public void DeleteCategory(int categoryID)
        {
            if (this.token != null)
            {
                this.CategoryTable.Delete(this.connectionString, this.token, categoryID);
            }
        }

        public List<Product>? GetProducts()
        {
            return this.ProductTable.Products;
        }

        public void AddProduct(int productID, string productName, string? productDescription, int? productPrice, int? categoryID)
        {
            if (this.token != null)
            {
                this.ProductTable.Add(this.connectionString, this.token, productID, productName, productDescription, productPrice, categoryID);
            }
        }

        public void UpdateProduct(int productID, string productName, string? productDescription, int? productPrice, int? categoryID)
        {
            if (this.token != null)
            {
                this.ProductTable.Update(this.connectionString, this.token, productID, productName, productDescription, productPrice, categoryID);
            }
        }

        public void DeleteProduct(int productID)
        {
            if (this.token != null)
            {
                this.ProductTable.Delete(this.connectionString, this.token, productID);
            }
        }

        public List<ProductVariant>? GetProductVariants()
        {
            return this.ProductVariantTable.ProductVariants;
        }

        public void AddProductVariant(int productVariantID, int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
        {
            if (this.token != null)
            {
                this.ProductVariantTable.Add(this.connectionString, this.token, productVariantID, productID, productVariantImageURL, productVariantColor, productVariantSize);
            }
        }

        public void UpdateProductVariant(int productVariantID, int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
        {
            if (this.token != null)
            {
                this.ProductVariantTable.Update(this.connectionString, this.token, productVariantID, productID, productVariantImageURL, productVariantColor, productVariantSize);
            }
        }

        public void DeleteProductVariant(int productVariantID)
        {
            if (this.token != null)
            {
                this.ProductVariantTable.Delete(this.connectionString, this.token, productVariantID);
            }
        }


        public List<WarehouseAddress>? GetWarehouseAddresses()
        {
            return this.WarehouseAddressTable.WarehouseAddresses;
        }

        public void AddWarehouseAddress(int warehouseAddressID, string warehouseAddressAddress, string? warehouseAddressDistrict, string? warehouseAddressPostalCode, string? warehouseAddressCity, string? warehouseAddressCountry)
        {
            if (this.token != null)
            {
                this.WarehouseAddressTable.Add(this.connectionString, this.token, warehouseAddressID, warehouseAddressAddress, warehouseAddressDistrict, warehouseAddressPostalCode, warehouseAddressCity, warehouseAddressCountry);
            }
        }

        public void UpdateWarehouseAddress(int warehouseAddressID, string warehouseAddressAddress, string? warehouseAddressDistrict, string? warehouseAddressPostalCode, string? warehouseAddressCity, string? warehouseAddressCountry)
        {
            if (this.token != null)
            {
                this.WarehouseAddressTable.Update(this.connectionString, this.token, warehouseAddressID, warehouseAddressAddress, warehouseAddressDistrict, warehouseAddressPostalCode, warehouseAddressCity, warehouseAddressCountry);
            }
        }

        public void DeleteWarehouseAddress(int warehouseAddressID)
        {
            if (this.token != null)
            {
                this.WarehouseAddressTable.Delete(this.connectionString, this.token, warehouseAddressID);
            }
        }

        public List<Warehouse>? GetWarehouses()
        {
            return this.WarehouseTable.Warehouses;
        }

        public void AddWarehouse(int warehouseID, string warehouseName, int? warehouseAddressID)
        {
            if (this.token != null)
            {
                this.WarehouseTable.Add(this.connectionString, this.token, warehouseID, warehouseName, warehouseAddressID);
            }
        }

        public void UpdateWarehouse(int warehouseID, string warehouseName, int? warehouseAddressID)
        {
            if (this.token != null)
            {
                this.WarehouseTable.Update(this.connectionString, this.token, warehouseID, warehouseName, warehouseAddressID);
            }
        }

        public void DeleteWarehouse(int warehouseID)
        {
            if (this.token != null)
            {
                this.WarehouseTable.Delete(this.connectionString, this.token, warehouseID);
            }
        }

        public List<WarehouseStock>? GetWarehouseStocks()
        {
            return this.WarehouseStockTable.WarehouseStocks;
        }

        public void AddWarehouseStock(int warehouseID, int productVariantID, int warehouseStockQuantity)
        {
            if (this.token != null)
            {
                this.WarehouseStockTable.Add(this.connectionString, this.token, warehouseID, productVariantID, warehouseStockQuantity);
            }
        }

        public void UpdateWarehouseStock(int warehouseID, int productVariantID, int warehouseStockQuantity)
        {
            if (this.token != null)
            {
                this.WarehouseStockTable.Update(this.connectionString, this.token, warehouseID, productVariantID, warehouseStockQuantity);
            }
        }

        public void DeleteWarehouseStock(int warehouseID, int productVariantID)
        {
            if (this.token != null)
            {
                this.WarehouseStockTable.Delete(this.connectionString, this.token, warehouseID, productVariantID);
            }
        }

        public List<InventoryAudit>? GetInventoryAudits()
        {
            return this.InventoryAuditTable.InventoryAudits;  // Assuming InventoryAuditTable has a property for InventoryAudits
        }

        public void AddInventoryAudit(int inventoryAuditID, int warehouseID, int? userID, DateTime inventoryAuditTime)
        {
            if (this.token != null)
            {
                this.InventoryAuditTable.Add(this.connectionString, this.token, inventoryAuditID, warehouseID, userID, inventoryAuditTime);
            }
        }

        public void UpdateInventoryAudit(int inventoryAuditID, int warehouseID, int? userID, DateTime inventoryAuditTime)
        {
            if (this.token != null)
            {
                this.InventoryAuditTable.Update(this.connectionString, this.token, inventoryAuditID, warehouseID, userID, inventoryAuditTime);
            }
        }

        public void DeleteInventoryAudit(int inventoryAuditID)
        {
            if (this.token != null)
            {
                this.InventoryAuditTable.Delete(this.connectionString, this.token, inventoryAuditID);
            }
        }

        public List<InventoryAuditDetail>? GetInventoryAuditDetails()
        {
            return this.InventoryAuditDetailTable.InventoryAuditDetails;
        }

        public void AddInventoryAuditDetail(int inventoryAuditID, int productVariantID, int inventoryAuditDetailActualQuantity)
        {
            if (this.token != null)
            {
                this.InventoryAuditDetailTable.Add(this.connectionString, this.token, inventoryAuditID, productVariantID, inventoryAuditDetailActualQuantity);
            }
        }

        public void UpdateInventoryAuditDetail(int inventoryAuditID, int productVariantID, int inventoryAuditDetailActualQuantity)
        {
            if (this.token != null)
            {
                this.InventoryAuditDetailTable.Update(this.connectionString, this.token, inventoryAuditID, productVariantID, inventoryAuditDetailActualQuantity);
            }
        }

        public void DeleteInventoryAuditDetail(int inventoryAuditID, int productVariantID)
        {
            if (this.token != null)
            {
                this.InventoryAuditDetailTable.Delete(this.connectionString, this.token, inventoryAuditID, productVariantID);
            }
        }


        public List<InboundShipment>? GetInboundShipments()
        {
            return this.InboundShipmentTable.InboundShipments;
        }

        public void AddInboundShipment(int inboundShipmentID, int? supplierID, int warehouseID, DateTime? inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, int? userID)
        {
            if (this.token != null)
            {
                this.InboundShipmentTable.Add(this.connectionString, this.token, inboundShipmentID, supplierID, warehouseID, inboundShipmentStartingDate, inboundShipmentStatus, inboundShipmentDescription, userID);
            }
        }

        public void UpdateInboundShipment(int inboundShipmentID, int? supplierID, int warehouseID, DateTime? inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, int? userID)
        {
            if (this.token != null)
            {
                this.InboundShipmentTable.Update(this.connectionString, this.token, inboundShipmentID, supplierID, warehouseID, inboundShipmentStartingDate, inboundShipmentStatus, inboundShipmentDescription, userID);
            }
        }

        public void DeleteInboundShipment(int inboundShipmentID)
        {
            if (this.token != null)
            {
                this.InboundShipmentTable.Delete(this.connectionString, this.token, inboundShipmentID);
            }
        }

        public List<InboundShipmentDetail>? GetInboundShipmentDetails()
        {
            return this.InboundShipmentDetailTable.InboundShipmentDetails;
        }

        public void AddInboundShipmentDetail(int inboundShipmentID, int productVariantID, int inboundShipmentDetailAmount)
        {
            if (this.token != null)
            {
                this.InboundShipmentDetailTable.Add(this.connectionString, this.token, inboundShipmentID, productVariantID, inboundShipmentDetailAmount);
            }
        }

        public void UpdateInboundShipmentDetail(int inboundShipmentID, int productVariantID, int inboundShipmentDetailAmount)
        {
            if (this.token != null)
            {
                this.InboundShipmentDetailTable.Update(this.connectionString, this.token, inboundShipmentID, productVariantID, inboundShipmentDetailAmount);
            }
        }

        public void DeleteInboundShipmentDetail(int inboundShipmentID, int productVariantID)
        {
            if (this.token != null)
            {
                this.InboundShipmentDetailTable.Delete(this.connectionString, this.token, inboundShipmentID, productVariantID);
            }
        }

        public List<OutboundShipment>? GetOutboundShipments()
        {
            return this.OutboundShipmentTable.OutboundShipments;
        }

        public void AddOutboundShipment(int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int? userID)
        {
            if (this.token != null)
            {
                this.OutboundShipmentTable.Add(this.connectionString, this.token, outboundShipmentID, warehouseID, outboundShipmentAddress, outboundShipmentStartingDate, outboundShipmentStatus, outboundShipmentDescription, userID);
            }
        }

        public void UpdateOutboundShipment(int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int? userID)
        {
            if (this.token != null)
            {
                this.OutboundShipmentTable.Update(this.connectionString, this.token, outboundShipmentID, warehouseID, outboundShipmentAddress, outboundShipmentStartingDate, outboundShipmentStatus, outboundShipmentDescription, userID);
            }
        }

        public void DeleteOutboundShipment(int outboundShipmentID)
        {
            if (this.token != null)
            {
                this.OutboundShipmentTable.Delete(this.connectionString, this.token, outboundShipmentID);
            }
        }

        public List<OutboundShipmentDetail>? GetOutboundShipmentDetails()
        {
            return this.OutboundShipmentDetailTable.OutboundShipmentDetails;
        }

        public void AddOutboundShipmentDetail(int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
        {
            if (this.token != null)
            {
                this.OutboundShipmentDetailTable.Add(this.connectionString, this.token, outboundShipmentID, productVariantID, outboundShipmentDetailAmount);
            }
        }

        public void UpdateOutboundShipmentDetail(int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
        {
            if (this.token != null)
            {
                this.OutboundShipmentDetailTable.Update(this.connectionString, this.token, outboundShipmentID, productVariantID, outboundShipmentDetailAmount);
            }
        }

        public void DeleteOutboundShipmentDetail(int outboundShipmentID, int productVariantID)
        {
            if (this.token != null)
            {
                this.OutboundShipmentDetailTable.Delete(this.connectionString, this.token, outboundShipmentID, productVariantID);
            }
        }

        public List<StockTransfer>? GetStockTransfers()
        {
            return this.StockTransferTable.StockTransfers;
        }

        public void AddStockTransfer(int stockTransferID, int fromWarehouseID, int toWarehouseID, DateTime? stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, int? userID)
        {
            if (this.token != null)
            {
                this.StockTransferTable.Add(this.connectionString, this.token, stockTransferID, fromWarehouseID, toWarehouseID, stockTransferStartingDate, stockTransferStatus, stockTransferDescription, userID);
            }
        }

        public void UpdateStockTransfer(int stockTransferID, int fromWarehouseID, int toWarehouseID, DateTime? stockTransferStartingDate, string stockTransferStatus, string? stockTransferDescription, int? userID)
        {
            if (this.token != null)
            {
                this.StockTransferTable.Update(this.connectionString, this.token, stockTransferID, fromWarehouseID, toWarehouseID, stockTransferStartingDate, stockTransferStatus, stockTransferDescription, userID);
            }
        }

        public void DeleteStockTransfer(int stockTransferID)
        {
            if (this.token != null)
            {
                this.StockTransferTable.Delete(this.connectionString, this.token, stockTransferID);
            }
        }

        public List<StockTransferDetail>? GetStockTransferDetails()
        {
            return this.StockTransferDetailTable.StockTransferDetails;
        }

        public void AddStockTransferDetail(int stockTransferID, int productVariantID, int stockTransferDetailAmount)
        {
            if (this.token != null)
            {
                this.StockTransferDetailTable.Add(this.connectionString, this.token, stockTransferID, productVariantID, stockTransferDetailAmount);
            }
        }

        public void UpdateStockTransferDetail(int stockTransferID, int productVariantID, int stockTransferDetailAmount)
        {
            if (this.token != null)
            {
                this.StockTransferDetailTable.Update(this.connectionString, this.token, stockTransferID, productVariantID, stockTransferDetailAmount);
            }
        }

        public void DeleteStockTransferDetail(int stockTransferID, int productVariantID)
        {
            if (this.token != null)
            {
                this.StockTransferDetailTable.Delete(this.connectionString, this.token, stockTransferID, productVariantID);
            }
        }



    }
}