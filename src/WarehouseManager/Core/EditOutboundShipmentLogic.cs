using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core
{
    public static class EditOutboundShipmentLogic
    {
        //Lấy các thông tin của OutboundShipment được chọn
        private static OutboundShipment GetOutboundshipment(int outboundShipmentID)
        {
            List<OutboundShipment> outboundShipments = Program.Warehouse.OutboundShipmentTable.OutboundShipments ?? new List<OutboundShipment>();
            OutboundShipment outboundShipment = outboundShipments.FirstOrDefault(oS => oS.OutboundShipmentID == outboundShipmentID) ?? new OutboundShipment(0, 0, "", DateTime.Now, "", "", 0);
            return outboundShipment;
        }

        public static DateTime GetOutboundShipmentDate(int outboundShipmentID)
        {
            return GetOutboundshipment(outboundShipmentID).OutboundShipmentStartingDate ?? DateTime.Now;
        }

        public static string GetOutboundShipmentStatus(int outboundShipmentID)
        {
            return GetOutboundshipment(outboundShipmentID).OutboundShipmentStatus;
        }

        public static string GetOutboundShipmentDescription(int outboundShipmentID)
        {
            return $"{GetOutboundshipment(outboundShipmentID).OutboundShipmentDescription}";
        }

        public static string GetOutboundShipmentAddress(int outboundShipmentID)
        {
            return $"{GetOutboundshipment(outboundShipmentID).OutboundShipmentAddress}";
        }

        public static int GetOutboundShipmentWarehouse(int outboundShipmentID)
        {
            int warehouseID = GetOutboundshipment(outboundShipmentID).WarehouseID;

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

        public static string GetOutboundShipmentUserName(int outboundShipmentID)
        {
            int userID = GetOutboundshipment(outboundShipmentID).UserID;

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

        // Lấy thông tin OutboundShipmentDetail của phiếu
        public static DataTable GetOutboundShipmentDetailData(int outboundShipmentID)
        {
            List<OutboundShipmentDetail> outboundShipmentDetails = Program.Warehouse.OutboundShipmentDetailTable.OutboundShipmentDetails ?? new List<OutboundShipmentDetail>();
            List<OutboundShipmentDetail> currentOutboundShipmentDetails = outboundShipmentDetails.Where(oSD => oSD.OutboundShipmentID == outboundShipmentID).ToList();

            List<(int, string, int)> outboundShipmentDetailRows = new List<(int, string, int)>();

            foreach (OutboundShipmentDetail currentOutboundShipmentDetail in currentOutboundShipmentDetails)
            {
                string productVariantName = GetProductVariantName(currentOutboundShipmentDetail.ProductVariantID);

                outboundShipmentDetailRows.Add((
                    currentOutboundShipmentDetail.ProductVariantID,
                    productVariantName,
                    currentOutboundShipmentDetail.OutboundShipmentDetailAmount));
            }

            return ConvertOutboundShipmentDetailDataToDataTable(outboundShipmentDetailRows);
        }

        private static DataTable ConvertOutboundShipmentDetailDataToDataTable(List<(int, string, int)> outboundShipmentDetailRows)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Variant ID", typeof(int));
            dataTable.Columns.Add("Variant Name", typeof(string));
            dataTable.Columns.Add("Amount", typeof(int));

            foreach (var outboundShipmentDetailRow in outboundShipmentDetailRows)
            {
                dataTable.Rows.Add(
                    outboundShipmentDetailRow.Item1,
                    outboundShipmentDetailRow.Item2,
                    outboundShipmentDetailRow.Item3);
            }

            return dataTable;
        }


        // Save Button
        public static void Save(int outboundShipmentID, string outboundShipmentAddress, string warehouseName, DateTime outboundShipmentStartingDate, string outboundShipmentStatus, string outboundShipmentDescription, string userName)
        {
            SaveOutboundShipment(outboundShipmentID, outboundShipmentAddress, warehouseName, outboundShipmentStartingDate, outboundShipmentStatus, outboundShipmentDescription, userName);
        }

        // Lưu OutboundShipment
        private static void SaveOutboundShipment(int outboundShipmentID, string outboundShipmentAddress, string warehouseName, DateTime outboundShipmentStartingDate, string outboundShipmentStatus, string outboundShipmentDescription, string userName)
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseName == warehouseName) ?? new Warehouse(0, "", 0);
            List<User> users = Program.Warehouse.UserTable.Users ?? new List<User>();
            User user = users.FirstOrDefault(u => u.UserFullName == userName) ?? new User(0, "", "", "", "", "", 0);
            int warehouseID = warehouse.WarehouseID;
            int userID = user.UserID;

            Program.Warehouse.OutboundShipmentTable.Update(outboundShipmentID, warehouseID, outboundShipmentAddress, outboundShipmentStartingDate, outboundShipmentStatus, outboundShipmentDescription, userID);
        }


        // Add OutboundShipmentDetail to Table and Database
        public static DataTable AddOutboundShipmentDetail(DataTable currentDataTable, int productVariantID, int quantity, int outboundShipmentID)
        {
            DataTable dataTable = currentDataTable.Copy();

            string productVariantName = GetProductVariantName(productVariantID);
            dataTable.Rows.Add(productVariantID, productVariantName, quantity);

            Program.Warehouse.OutboundShipmentDetailTable.Add(outboundShipmentID, productVariantID, quantity);

            return dataTable;
        }

        // Delete OutboundShipmentDetail to Table and Database
        public static DataTable DeleteOutboundShipmentDetail(DataTable currentDataTable, int row, int productVariantID, int outboundShipmentID)
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
            Program.Warehouse.OutboundShipmentDetailTable.Delete(outboundShipmentID, productVariantID);
            return dataTable;
        }

        // Update OutboundShipmentDetail to Table and Database
        public static DataTable UpdateOutboundShipmentDetail(DataTable currentDataTable, int productVariantID, int quantity, int outboundShipmentID)
        {
            DataTable dataTable = currentDataTable.Copy();

            Program.Warehouse.OutboundShipmentDetailTable.Update(outboundShipmentID, productVariantID, quantity);

            return dataTable;
        }
    }
}
