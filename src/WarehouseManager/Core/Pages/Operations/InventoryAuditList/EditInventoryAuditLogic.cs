using System.Data;
using Org.BouncyCastle.Math.EC;
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

        public static DataTable AddVariant(int inventoryAuditID, string warehouseName, string variantID, string quantity, DataTable dataTable)
        {
            string status = GetCurrentStatus(inventoryAuditID, GetInventoryAudits());
            if (status != "Processing")
            {
                throw new Exception("Inventory Audit is already completed.");
            }
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

            string status = GetCurrentStatus(inventoryAuditID, GetInventoryAudits());
            if (status == "Processing")
            {
                dataTable = RefreshStockAmount(inventoryAuditID, dataTable);
            }

            return dataTable;
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

            if (status == "Processing")
            {
                dataTable = RefreshStockAmount(inventoryAuditID, dataTable);
            }
            UpdateInventoryAuditDetail(inventoryAuditID, dataTable);

            if (status == "Completed (Reconciled)")
            {
                ReconcileStockAmount(inventoryAuditID, dataTable);
            }
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

        private static List<InboundShipment> GetInboundShipments()
        {
            List<InboundShipment> inboundShipments = Program.Warehouse.InboundShipmentTable.InboundShipments ?? new List<InboundShipment>();
            return inboundShipments;
        }

        private static List<OutboundShipment> GetOutboundShipments()
        {
            List<OutboundShipment> outboundShipments = Program.Warehouse.OutboundShipmentTable.OutboundShipments ?? new List<OutboundShipment>();
            return outboundShipments;
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

        private static void ReconcileStockAmount(int inventoryAuditID, DataTable dataTable)
        {
            int warehouseID = GetWarehouseID(inventoryAuditID, GetInventoryAudits());

            int inboundShipmentID = GetCurrentHighestInboundShipmentID() + 1;
            CreateReconcilingInboundShipment(inboundShipmentID, warehouseID);

            int outboundShipmentID = GetCurrentHighestOutboundShipmentID() + 1;
            CreateReconcilingOutboundShipment(outboundShipmentID, warehouseID);

            foreach (DataRow row in dataTable.Rows)
            {
                int productVariantID = int.Parse($"{row[0]}");
                int stockAmount = int.Parse($"{row[2]}");
                int actualAmount = int.Parse($"{row[3]}");
                int difference = actualAmount - stockAmount;

                if (difference > 0)
                {
                    CreateInboundShipmentDetail(inboundShipmentID, productVariantID, difference);
                }

                if (difference < 0)
                {
                    CreateOutboundShipmentDetail(outboundShipmentID, productVariantID, Math.Abs(difference));
                }
            }

            CompleteReconcilingInboundShipment(inboundShipmentID, warehouseID);
            CompleteReconcilingOutboundShipment(outboundShipmentID, warehouseID);
        }

        private static int GetCurrentHighestInboundShipmentID()
        {
            return GetInboundShipments().Max(i => i.InboundShipmentID);
        }

        private static int GetCurrentHighestOutboundShipmentID()
        {
            return GetOutboundShipments().Max(i => i.OutboundShipmentID);
        }

        private static void CreateReconcilingInboundShipment(int inboundShipmentID, int warehouseID)
        {
            int supplierID = 1;
            DateTime inboundShipmentStartingDate = DateTime.Now;
            string status = "Processing";
            string description = "Shipping inbound to reconcile for gained stocks.";
            int userID = AddInventoryAuditLogic.GetUserID($"{Program.Warehouse.Username}", GetUsers());

            Program.Warehouse.InboundShipmentTable.Add(inboundShipmentID, supplierID, warehouseID, inboundShipmentStartingDate, status, description, userID);
        }

        private static void CreateReconcilingOutboundShipment(int outboundShipmentID, int warehouseID)
        {
            string address = "Inventory Audit";
            DateTime outboundShipmentStartingDate = DateTime.Now;
            string status = "Processing";
            string description = "Shipping outbound to reconcile for gained stocks.";
            int userID = AddInventoryAuditLogic.GetUserID($"{Program.Warehouse.Username}", GetUsers());

            Program.Warehouse.OutboundShipmentTable.Add(outboundShipmentID, warehouseID, address, outboundShipmentStartingDate, status, description, userID);
        }

        private static void CreateInboundShipmentDetail(int inboundShipmentID, int productVariantID, int inboundShipmentDetailAmount)
        {
            Program.Warehouse.InboundShipmentDetailTable.Add(inboundShipmentID, productVariantID, inboundShipmentDetailAmount);
        }

        private static void CreateOutboundShipmentDetail(int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
        {
            Program.Warehouse.OutboundShipmentDetailTable.Add(outboundShipmentID, productVariantID, outboundShipmentDetailAmount);
        }

        private static void CompleteReconcilingInboundShipment(int inboundShipmentID, int warehouseID)
        {
            int supplierID = 1;
            DateTime inboundShipmentStartingDate = DateTime.Now;
            string status = "Completed";
            string description = "Shipping inbound to reconcile for gained stocks.";
            int userID = AddInventoryAuditLogic.GetUserID($"{Program.Warehouse.Username}", GetUsers());

            Program.Warehouse.InboundShipmentTable.Update(inboundShipmentID, supplierID, warehouseID, inboundShipmentStartingDate, status, description, userID);
        }

        private static void CompleteReconcilingOutboundShipment(int outboundShipmentID, int warehouseID)
        {
            string address = "Inventory Audit";
            DateTime outboundShipmentStartingDate = DateTime.Now;
            string status = "Completed";
            string description = "Shipping outbound to reconcile for gained stocks.";
            int userID = AddInventoryAuditLogic.GetUserID($"{Program.Warehouse.Username}", GetUsers());

            Program.Warehouse.OutboundShipmentTable.Update(outboundShipmentID, warehouseID, address, outboundShipmentStartingDate, status, description, userID);
        }
    }
}
