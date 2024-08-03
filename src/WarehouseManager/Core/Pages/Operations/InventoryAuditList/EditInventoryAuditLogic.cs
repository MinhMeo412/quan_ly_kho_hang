using System.Data;
using Mysqlx.Crud;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class EditInventoryAuditLogic
    {
        public static string GetWarehouseName(int inventoryAuditID)
        {
            List<InventoryAudit> inventoryAudits = GetInventoryAudits();
            List<Warehouse> warehouses = GetWarehouses();
            return GetWarehouseName(inventoryAuditID, inventoryAudits, warehouses);
        }

        public static List<string> GetStatusList(int inventoryAuditID)
        {
            List<InventoryAudit> inventoryAudits = GetInventoryAudits();
            string currentStatus = GetCurrentStatus(inventoryAuditID, inventoryAudits);
            List<string> statuses = new List<string>();

            switch (currentStatus)
            {
                case "Processing":
                    statuses.Add("Processing");
                    statuses.Add("Completed (Unreconciled)");
                    statuses.Add("Completed (Reconciled)");
                    break;
                case "Completed (Unreconciled)":
                    statuses.Add("Completed (Unreconciled)");
                    statuses.Add("Completed (Reconciled)");
                    break;
                case "Completed (Reconciled)":
                    statuses.Add("Completed (Reconciled)");
                    break;
                default:
                    break;
            }

            return statuses;
        }

        public static int GetStatusIndex(int inventoryAuditID)
        {
            List<string> statuses = GetStatusList(inventoryAuditID);
            List<InventoryAudit> inventoryAudits = GetInventoryAudits();
            string currentStatus = GetCurrentStatus(inventoryAuditID, inventoryAudits);
            return statuses.IndexOf(currentStatus);
        }

        public static string GetUsername(int inventoryAuditID)
        {
            List<InventoryAudit> inventoryAudits = GetInventoryAudits();
            List<User> users = GetUsers();

            int userID = GetUserID(inventoryAuditID, inventoryAudits);
            return GetUserName(userID, users);
        }

        public static DateTime GetCreationDate(int inventoryAuditID)
        {
            return GetCreationDate(inventoryAuditID, GetInventoryAudits());
        }

        public static string GetDescription(int inventoryAuditID)
        {
            return $"{GetInventoryAudit(inventoryAuditID, GetInventoryAudits()).InventoryAuditDescription}";
        }

        public static Dictionary<int, string> GetVariantList()
        {
            return AddInventoryAuditLogic.GetVariantList();
        }

        public static int GetVariantID(string variantName, Dictionary<int, string> variantList)
        {
            return AddInventoryAuditLogic.GetVariantID(variantName, variantList);
        }

        public static int GetVariantIndex(string variantID, Dictionary<int, string> variantList)
        {
            return AddInventoryAuditLogic.GetVariantIndex(variantID, variantList);
        }

        public static DataTable AddVariant(string warehouseName, string variantID, string quantity, DataTable dataTable)
        {
            return AddInventoryAuditLogic.AddVariant(warehouseName, variantID, quantity, dataTable);
        }

        public static DataTable Search(DataTable dataTable, string searchTerm)
        {
            return AddInventoryAuditLogic.Search(dataTable, searchTerm);
        }

        public static DataTable GetData(int inventoryAuditID)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Product");
            dataTable.Columns.Add("Stock Amount");
            dataTable.Columns.Add("Actual Amount");
            dataTable.Columns.Add("Difference");

            List<InventoryAuditDetail> inventoryAuditDetails = GetRelevantInventoryAuditDetail(inventoryAuditID, GetInventoryAuditDetails());
            List<Product> products = GetProducts();
            List<ProductVariant> productVariants = GetProductVariants();

            foreach (InventoryAuditDetail inventoryAuditDetail in inventoryAuditDetails)
            {
                int variantID = inventoryAuditDetail.ProductVariantID;
                string productName = GetProductVariantName(inventoryAuditDetail.ProductVariantID, productVariants, products);
                int stockAmount = inventoryAuditDetail.InventoryAuditDetailStockQuantity;
                int actualAmount = inventoryAuditDetail.InventoryAuditDetailActualQuantity;


                dataTable.Rows.Add(
                    variantID,
                    productName,
                    stockAmount,
                    actualAmount,
                    actualAmount - stockAmount
                );
            }

            return RefreshStockAmount(inventoryAuditID, dataTable);
        }

        public static DataTable EditVariant(int row, string newAmount, DataTable dataTable)
        {
            int newActualAmount = int.TryParse(newAmount, out int parsedQuantity) ? parsedQuantity : 0;
            int stockAmount = int.Parse($"{dataTable.Rows[row][2]}");

            dataTable.Rows[row][3] = newActualAmount;
            dataTable.Rows[row][4] = newActualAmount - stockAmount;
            
            return dataTable;
        }

        public static DataTable DeleteVariant(DataTable currentDataTable, int row)
        {
            return AddInventoryAuditLogic.DeleteVariant(currentDataTable, row);
        }

        public static DataTable GetAllStock(string warehouseName, DataTable dataTable)
        {
            return AddInventoryAuditLogic.GetAllStock(warehouseName, dataTable);
        }

        public static void Save(int inventoryAuditID, string status, string description, DataTable dataTable)
        {
            UpdateInventoryAudit(inventoryAuditID, status, description);
            UpdateInventoryAuditDetail(inventoryAuditID, RefreshStockAmount(inventoryAuditID, dataTable));
        }

        private static List<Warehouse> GetWarehouses()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetWarehouses();
        }

        private static List<InventoryAudit> GetInventoryAudits()
        {
            List<InventoryAudit> inventoryAudits = Program.Warehouse.InventoryAuditTable.InventoryAudits ?? new List<InventoryAudit>();
            return inventoryAudits;
        }

        private static List<InventoryAuditDetail> GetInventoryAuditDetails()
        {
            List<InventoryAuditDetail> inventoryAuditDetails = Program.Warehouse.InventoryAuditDetailTable.InventoryAuditDetails ?? new List<InventoryAuditDetail>();
            return inventoryAuditDetails;
        }

        private static List<User> GetUsers()
        {
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            return users;
        }

        private static List<Product> GetProducts()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetProducts();
        }

        private static List<ProductVariant> GetProductVariants()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetProductVariants();
        }

        private static List<WarehouseStock> GetWarehouseStocks()
        {
            List<WarehouseStock> warehouseStocks = Program.Warehouse.WarehouseStockTable.WarehouseStocks ?? new List<WarehouseStock>();
            return warehouseStocks;
        }

        private static int GetWarehouseID(int inventoryAuditID, List<InventoryAudit> inventoryAudits)
        {
            return GetInventoryAudit(inventoryAuditID, inventoryAudits).WarehouseID;
        }

        private static string GetWarehouseName(int warehouseID, List<Warehouse> warehouses)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetWarehouseName(warehouseID, warehouses);
        }

        private static string GetWarehouseName(int inventoryAuditID, List<InventoryAudit> inventoryAudits, List<Warehouse> warehouses)
        {
            int warehouseID = GetWarehouseID(inventoryAuditID, inventoryAudits);
            return GetWarehouseName(warehouseID, warehouses);
        }

        private static string GetCurrentStatus(int inventoryAuditID, List<InventoryAudit> inventoryAudits)
        {
            InventoryAudit inventoryAudit = GetInventoryAudit(inventoryAuditID, inventoryAudits);
            return inventoryAudit.InventoryAuditStatus;
        }

        private static InventoryAudit GetInventoryAudit(int inventoryAuditID, List<InventoryAudit> inventoryAudits)
        {
            InventoryAudit inventoryAudit = inventoryAudits.FirstOrDefault(ia => ia.InventoryAuditID == inventoryAuditID) ?? new InventoryAudit(0, 0, "", null, 0, DateTime.Now);
            return inventoryAudit;
        }

        private static int GetUserID(int inventoryAuditID, List<InventoryAudit> inventoryAudits)
        {
            return GetInventoryAudit(inventoryAuditID, inventoryAudits).UserID;
        }

        private static string GetUserName(int userID, List<User> users)
        {
            User user = users.FirstOrDefault(u => u.UserID == userID) ?? new User(0, "", "", "", null, null, 4);
            return user.UserFullName;
        }

        private static DateTime GetCreationDate(int inventoryAuditID, List<InventoryAudit> inventoryAudits)
        {
            return GetInventoryAudit(inventoryAuditID, inventoryAudits).InventoryAuditTime;
        }

        private static List<InventoryAuditDetail> GetRelevantInventoryAuditDetail(int inventoryAuditID, List<InventoryAuditDetail> inventoryAuditDetails)
        {
            List<InventoryAuditDetail> relevantInventoryAuditDetails = inventoryAuditDetails
                .Where(ia => ia.InventoryAuditID == inventoryAuditID)
                .ToList();
            return relevantInventoryAuditDetails;
        }

        private static string GetProductVariantName(int variantID, List<ProductVariant> productVariants, List<Product> products)
        {
            return AddInventoryAuditLogic.GetProductVariantName(variantID, productVariants, products);
        }

        private static void UpdateInventoryAudit(int inventoryAuditID, string status, string description)
        {
            List<InventoryAudit> inventoryAudits = GetInventoryAudits();

            int warehouseID = GetWarehouseID(inventoryAuditID, inventoryAudits);
            int userID = GetUserID(inventoryAuditID, inventoryAudits);
            DateTime dateTime = GetCreationDate(inventoryAuditID, inventoryAudits);

            Program.Warehouse.InventoryAuditTable.Update(
                inventoryAuditID,
                warehouseID,
                status,
                description,
                userID,
                dateTime
            );
        }

        private static void UpdateInventoryAuditDetail(int inventoryAuditID, DataTable dataTable)
        {
            List<int> newVariantIDs = new List<int>();
            List<int> updatedVariantIDs = new List<int>();
            List<int> deletedVariantIDs = new List<int>();

            List<InventoryAuditDetail> relevantInventoryAuditDetails = GetRelevantInventoryAuditDetail(inventoryAuditID, GetInventoryAuditDetails());
            List<int> databaseVariantIDs = relevantInventoryAuditDetails.Select(i => i.ProductVariantID).Distinct().ToList();
            List<int> dataTableVariantIDs = new List<int>();

            foreach (DataRow row in dataTable.Rows)
            {
                dataTableVariantIDs.Add(int.Parse($"{row[0]}"));
            }

            foreach (int id in dataTableVariantIDs)
            {
                if (!databaseVariantIDs.Contains(id))
                {
                    newVariantIDs.Add(id);
                }
                else
                {
                    updatedVariantIDs.Add(id);
                }
            }

            foreach (int id in databaseVariantIDs)
            {
                if (!dataTableVariantIDs.Contains(id))
                {
                    deletedVariantIDs.Add(id);
                }
            }

            foreach (int id in newVariantIDs)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (id == int.Parse($"{row[0]}"))
                    {
                        AddInventoryAuditDetail(
                            inventoryAuditID,
                            id,
                            int.Parse($"{row[2]}"),
                            int.Parse($"{row[3]}")
                        );
                    }
                }
            }

            foreach (int id in updatedVariantIDs)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (id == int.Parse($"{row[0]}"))
                    {
                        UpdateInventoryAuditDetail(
                            inventoryAuditID,
                            id,
                            int.Parse($"{row[2]}"),
                            int.Parse($"{row[3]}")
                        );
                    }
                }
            }

            foreach (int id in deletedVariantIDs)
            {
                DeleteInventoryAuditDetail(inventoryAuditID, id);
            }
        }

        private static DataTable RefreshStockAmount(int inventoryAuditID, DataTable dataTable)
        {
            int warehouseID = GetWarehouseID(inventoryAuditID, GetInventoryAudits());
            List<WarehouseStock> warehouseStocks = GetWarehouseStocks();

            DataTable newDataTable = new DataTable();
            newDataTable.Columns.Add("ID");
            newDataTable.Columns.Add("Product");
            newDataTable.Columns.Add("Stock Amount");
            newDataTable.Columns.Add("Actual Amount");
            newDataTable.Columns.Add("Difference");


            foreach (DataRow row in dataTable.Rows)
            {
                int variantID = int.Parse($"{row[0]}");
                string productName = $"{row[1]}";
                int currentStockAmount = GetStockAmount(warehouseID, variantID, warehouseStocks);
                int actualAmount = int.Parse($"{row[3]}");
                int difference = actualAmount - currentStockAmount;

                newDataTable.Rows.Add(
                    variantID,
                    productName,
                    currentStockAmount,
                    actualAmount,
                    difference
                );
            }

            return newDataTable;
        }

        private static int GetStockAmount(int warehouseID, int variantID, List<WarehouseStock> warehouseStocks)
        {
            return AddInventoryAuditLogic.GetStockAmount(warehouseID, variantID, warehouseStocks);
        }

        private static void AddInventoryAuditDetail(int inventoryAuditID, int productVariantID, int stockAmount, int actualAmount)
        {
            Program.Warehouse.InventoryAuditDetailTable.Add(inventoryAuditID, productVariantID, stockAmount, actualAmount);
        }

        private static void UpdateInventoryAuditDetail(int inventoryAuditID, int productVariantID, int stockAmount, int actualAmount)
        {
            Program.Warehouse.InventoryAuditDetailTable.Update(inventoryAuditID, productVariantID, stockAmount, actualAmount);
        }

        private static void DeleteInventoryAuditDetail(int inventoryAuditID, int productVariantID)
        {
            Program.Warehouse.InventoryAuditDetailTable.Delete(inventoryAuditID, productVariantID);
        }

    }
}
