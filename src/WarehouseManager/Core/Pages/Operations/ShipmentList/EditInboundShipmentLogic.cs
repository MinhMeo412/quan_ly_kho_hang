using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class EditInboundShipmentLogic
    {
        //Lấy các thông tin của InboundShipment được chọn
        private static InboundShipment GetInboundshipment(int inboundShipmentID)
        {
            List<InboundShipment> inboundShipments = Program.Warehouse.InboundShipmentTable.InboundShipments ?? new List<InboundShipment>();
            InboundShipment inboundShipment = inboundShipments.FirstOrDefault(iS => iS.InboundShipmentID == inboundShipmentID) ?? new InboundShipment(0, 0, 0, DateTime.Now, "", "", 0);
            return inboundShipment;
        }

        public static DateTime GetInboundShipmentDate(int inboundShipmentID)
        {
            return GetInboundshipment(inboundShipmentID).InboundShipmentStartingDate ?? DateTime.Now;
        }

        public static string GetInboundShipmentStatus(int inboundShipmentID)
        {
            return $"{GetInboundshipment(inboundShipmentID).InboundShipmentStatus}";
        }

        public static string GetInboundShipmentDescription(int inboundShipmentID)
        {
            return $"{GetInboundshipment(inboundShipmentID).InboundShipmentDescription}";
        }

        public static int GetInboundShipmentSupplier(int inboundShipmentID)
        {
            int supllierID = GetInboundshipment(inboundShipmentID).SupplierID;

            List<Supplier> suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();

            Supplier supplier = suppliers.FirstOrDefault(s => s.SupplierID == supllierID) ?? new Supplier(0, "", "", "", "", "", "");
            string supplierName = supplier.SupplierName;

            List<string> supplierNames = GetSupplierList();
            return supplierNames.IndexOf(supplierName);
        }

        public static List<string> GetSupplierList()
        {
            List<Supplier> suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();
            List<string> supplierNames = new List<string>();

            foreach (Supplier supplier in suppliers)
            {
                supplierNames.Add(supplier.SupplierName);
            }

            return supplierNames;
        }

        public static int GetInboundShipmentWarehouse(int inboundShipmentID)
        {
            int warehouseID = GetInboundshipment(inboundShipmentID).WarehouseID;

            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();

            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == warehouseID) ?? new Warehouse(0, "", 0);
            string warehouseName = warehouse.WarehouseName;

            List<string> warehouseNames = GetWarehouseList();
            return warehouseNames.IndexOf(warehouseName);
        }

        public static List<string> GetWarehouseList()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            List<string> warehouseNames = new List<string>();

            foreach (Warehouse warehouse in warehouses)
            {
                warehouseNames.Add(warehouse.WarehouseName);
            }

            return warehouseNames;
        }

        public static string GetInboundShipmentUserName(int inboundShipmentID)
        {
            int userID = GetInboundshipment(inboundShipmentID).UserID;

            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();

            User user = users.FirstOrDefault(u => u.UserID == userID) ?? new User(0, "", "", "", "", "", 0);
            string userName = user.UserFullName;
            return userName;
        }

        public static string GetProductVariantName(int productVariantID)
        {
            List<ProductVariant> productVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();
            List<Product> products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            ProductVariant productVariant = productVariants.FirstOrDefault(pV => pV.ProductVariantID == productVariantID) ?? new ProductVariant(0, 0, "", "", "");
            Product product = products.FirstOrDefault(p => p.ProductID == productVariant.ProductID) ?? new Product(0, "", "", 0, 0);
            string productVariantName = $"{product.ProductName} - {productVariant.ProductVariantColor} - {productVariant.ProductVariantSize}";
            return productVariantName;
        }

        // Lấy thông tin InboundShipmentDetail của phiếu
        public static DataTable GetInboundShipmentDetailData(int inboundShipmentID)
        {
            List<InboundShipmentDetail> inboundShipmentDetails = Program.Warehouse.InboundShipmentDetailTable.InboundShipmentDetails ?? new List<InboundShipmentDetail>();
            List<InboundShipmentDetail> currentInboundShipmentDetails = inboundShipmentDetails.Where(iSD => iSD.InboundShipmentID == inboundShipmentID).ToList();

            List<(int, string, int)> inboundShipmentDetailRows = new List<(int, string, int)>();

            foreach (InboundShipmentDetail currentInboundShipmentDetail in currentInboundShipmentDetails)
            {
                string productVariantName = GetProductVariantName(currentInboundShipmentDetail.ProductVariantID);

                inboundShipmentDetailRows.Add((
                    currentInboundShipmentDetail.ProductVariantID,
                    productVariantName,
                    currentInboundShipmentDetail.InboundShipmentDetailAmount));
            }

            return ConvertInboundShipmentDetailDataToDataTable(inboundShipmentDetailRows);
        }

        private static DataTable ConvertInboundShipmentDetailDataToDataTable(List<(int, string, int)> inboundShipmentDetailRows)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Variant ID", typeof(int));
            dataTable.Columns.Add("Variant Name", typeof(string));
            dataTable.Columns.Add("Amount", typeof(int));

            foreach (var inboundShipmentDetailRow in inboundShipmentDetailRows)
            {
                dataTable.Rows.Add(
                    inboundShipmentDetailRow.Item1,
                    inboundShipmentDetailRow.Item2,
                    inboundShipmentDetailRow.Item3);
            }

            return dataTable;
        }


        // Save Button
        public static void Save(int inboundShipmentID, string supplierName, string warehouseName, DateTime inboundShipmentStartingDate, string inboundShipmentStatus, string inboundShipmentDescription, string userName)
        {
            SaveInboundShipment(inboundShipmentID, supplierName, warehouseName, inboundShipmentStartingDate, inboundShipmentStatus, inboundShipmentDescription, userName);
        }

        // Lưu InboundShipment
        private static void SaveInboundShipment(int inboundShipmentID, string supplierName, string warehouseName, DateTime inboundShipmentStartingDate, string inboundShipmentStatus, string inboundShipmentDescription, string userName)
        {
            List<Supplier> suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();
            Supplier supplier = suppliers.FirstOrDefault(s => s.SupplierName == supplierName) ?? new Supplier(0, "", "", "", "", "", "");
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseName == warehouseName) ?? new Warehouse(0, "", 0);
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            User user = users.FirstOrDefault(u => u.UserFullName == userName) ?? new User(0, "", "", "", "", "", 0);
            int warehouseID = warehouse.WarehouseID;
            int supplierID = supplier.SupplierID;
            int userID = user.UserID;

            Program.Warehouse.InboundShipmentTable.Update(inboundShipmentID, supplierID, warehouseID, inboundShipmentStartingDate, inboundShipmentStatus, inboundShipmentDescription, userID);
        }


        // Add InboundShipmentDetail to Table and Database
        public static DataTable AddInboundShipmentDetail(DataTable currentDataTable, int productVariantID, int quantity, int inboundShipmentID)
        {
            DataTable dataTable = currentDataTable.Copy();

            string productVariantName = GetProductVariantName(productVariantID);
            dataTable.Rows.Add(productVariantID, productVariantName, quantity);

            Program.Warehouse.InboundShipmentDetailTable.Add(inboundShipmentID, productVariantID, quantity);

            return dataTable;
        }

        // Delete InboundShipmentDetail to Table and Database
        public static DataTable DeleteInboundShipmentDetail(DataTable currentDataTable, int row, int productVariantID, int inboundShipmentID)
        {
            DataTable dataTable = currentDataTable.Copy();

            if (row < 0 || row >= dataTable.Rows.Count)
            {
                return dataTable;
            }

            DataRow rowToDelete = dataTable.Rows[row];

            if (rowToDelete != null)
            {
                // Mark the row for deletion
                rowToDelete.Delete();
                // Commit the deletion
                dataTable.AcceptChanges();
            }
            Program.Warehouse.InboundShipmentDetailTable.Delete(inboundShipmentID, productVariantID);
            return dataTable;
        }

        // Update InboundShipmentDetail to Table and Database
        public static DataTable UpdateInboundShipmentDetail(DataTable currentDataTable, int productVariantID, int quantity, int inboundShipmentID)
        {
            DataTable dataTable = currentDataTable.Copy();

            Program.Warehouse.InboundShipmentDetailTable.Update(inboundShipmentID, productVariantID, quantity);

            return dataTable;
        }
    }
}
