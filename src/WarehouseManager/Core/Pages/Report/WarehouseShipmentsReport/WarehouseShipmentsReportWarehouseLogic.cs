using System.Data;
using WarehouseManager.Core.Utility;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class WarehouseShipmentsReportWarehouseLogic
    {
        public static List<string> GetReportOptions()
        {
            List<string> options = new List<string>{
                "Inbound",
                "Outbound",
                "Product"
            };
            return options;
        }

        public static List<string> GetWarehouseList()
        {
            List<Warehouse> warehouses = GetWarehouses();
            return warehouses.Select(w => w.WarehouseName).ToList();
        }

        public static DateTime GetDefaultStartDate()
        {
            return new DateTime(1, 1, 1);
        }

        public static DateTime GetDefaultEndDate()
        {
            return DateTime.Now;
        }

        public static DataTable GetWarehouseExportData(string option, string warehouseName, DateTime startDate, DateTime endDate, bool includeStockTransfers = true)
        {
            DataTable dataTable;

            if (option == "Inbound")
            {
                dataTable = GetInboundWarehouseExportData(warehouseName, startDate, endDate, includeStockTransfers);
            }
            else
            {
                dataTable = GetOutboundWarehouseExportData(warehouseName, startDate, endDate, includeStockTransfers);
            }

            return dataTable;
        }

        public static DataTable GetWarehouseFileInformation(string option, string warehouseName, DateTime startDate, DateTime endDate)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add($"Warehouse Shipments Report: {option}", typeof(string));

            dataTable.Rows.Add($"Warehouse: {warehouseName}");
            dataTable.Rows.Add($"Date: {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}");

            return dataTable;
        }

        internal static List<Warehouse> GetWarehouses()
        {
            List<Warehouse> warehouses = Program.Warehouse.WarehouseTable.Warehouses ?? new List<Warehouse>();
            return warehouses;
        }

        private static DataTable GetInboundWarehouseExportData(string warehouseName, DateTime startDate, DateTime endDate, bool includeStockTransfers)
        {
            List<InboundShipment> inboundShipments = GetRelevantInboundShipments(GetWarehouseID(warehouseName), startDate, endDate);
            List<InboundShipmentDetail> inboundShipmentDetails = GetRelevantInboundShipmentDetails(inboundShipments);
            List<ProductVariant> productVariants = GetRelevantProductVariants(inboundShipmentDetails);
            List<Product> products = GetRelevantProducts(productVariants);
            List<Category> categories = GetRelevantCategories(products);
            List<Supplier> suppliers = GetRelevantSuppliers(inboundShipments);
            List<StockTransfer> stockTransfers = new List<StockTransfer>();
            List<StockTransferDetail> stockTransferDetails = new List<StockTransferDetail>();
            List<Warehouse> warehouses = new List<Warehouse>();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add($"Shipment Type");
            dataTable.Columns.Add($"PID-VID");
            dataTable.Columns.Add($"Product");
            dataTable.Columns.Add($"Category");
            dataTable.Columns.Add($"Date");
            dataTable.Columns.Add($"Supplier");
            dataTable.Columns.Add($"Quantity");

            foreach (InboundShipmentDetail inboundShipmentDetail in inboundShipmentDetails)
            {
                int productID = GetProductID(inboundShipmentDetail.ProductVariantID, productVariants);

                dataTable.Rows.Add(
                    $"Inbound",
                    $"P{productID}-V{inboundShipmentDetail.ProductVariantID}",
                    $"{GetProductName(productID, products)} {GetVariantColor(inboundShipmentDetail.ProductVariantID, productVariants)} {GetVariantSize(inboundShipmentDetail.ProductVariantID, productVariants)}",
                    $"{GetCategoryName(GetCategoryID(productID, products), categories)}",
                    $"{GetInboundShipmentDate(inboundShipmentDetail.InboundShipmentID, inboundShipments)}",
                    $"{GetSupplierName(GetSupplierID(inboundShipmentDetail.InboundShipmentID, inboundShipments), suppliers)}",
                    inboundShipmentDetail.InboundShipmentDetailAmount
                );
            }

            if (includeStockTransfers)
            {
                stockTransfers = GetRelevantInboundStockTransfers(GetWarehouseID(warehouseName), startDate, endDate);
                stockTransferDetails = GetRelevantStockTransferDetails(stockTransfers);
                warehouses = GetRelevantWarehouses(stockTransfers);

                dataTable.Columns[5].ColumnName = "From";
            }
            foreach (StockTransferDetail stockTransferDetail in stockTransferDetails)
            {

                int productID = GetProductID(stockTransferDetail.ProductVariantID, productVariants);

                dataTable.Rows.Add(
                    $"Stock Transfer",
                    $"P{productID}-V{stockTransferDetail.ProductVariantID}",
                    $"{GetProductName(productID, products)} {GetVariantColor(stockTransferDetail.ProductVariantID, productVariants)} {GetVariantSize(stockTransferDetail.ProductVariantID, productVariants)}",
                    $"{GetCategoryName(GetCategoryID(productID, products), categories)}",
                    $"{GetStockTransferDate(stockTransferDetail.StockTransferID, stockTransfers)}",
                    $"{GetWarehouseName(GetFromWarehouseID(stockTransferDetail.StockTransferID, stockTransfers), warehouses)}",
                    stockTransferDetail.StockTransferDetailAmount
                );
            }

            // sort by date from newest to oldest
            dataTable = SortDataTable.ByColumn(dataTable, 4, true);
            dataTable = SortDataTable.ClearDirectionArrow(dataTable);

            return dataTable;
        }

        private static DataTable GetOutboundWarehouseExportData(string warehouseName, DateTime startDate, DateTime endDate, bool includeStockTransfers)
        {
            List<OutboundShipment> outboundShipments = GetRelevantOutboundShipments(GetWarehouseID(warehouseName), startDate, endDate);
            List<OutboundShipmentDetail> outboundShipmentDetails = GetRelevantOutboundShipmentDetails(outboundShipments);
            List<ProductVariant> productVariants = GetRelevantProductVariants(outboundShipmentDetails);
            List<Product> products = GetRelevantProducts(productVariants);
            List<Category> categories = GetRelevantCategories(products);
            List<StockTransfer> stockTransfers = new List<StockTransfer>();
            List<StockTransferDetail> stockTransferDetails = new List<StockTransferDetail>();
            List<Warehouse> warehouses = new List<Warehouse>();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add($"Shipment Type");
            dataTable.Columns.Add($"PID-VID");
            dataTable.Columns.Add($"Product");
            dataTable.Columns.Add($"Category");
            dataTable.Columns.Add($"Date");
            dataTable.Columns.Add($"To Address");
            dataTable.Columns.Add($"Quantity");

            foreach (OutboundShipmentDetail outboundShipmentDetail in outboundShipmentDetails)
            {
                int productID = GetProductID(outboundShipmentDetail.ProductVariantID, productVariants);

                dataTable.Rows.Add(
                    $"Outbound",
                    $"P{productID}-V{outboundShipmentDetail.ProductVariantID}",
                    $"{GetProductName(productID, products)} {GetVariantColor(outboundShipmentDetail.ProductVariantID, productVariants)} {GetVariantSize(outboundShipmentDetail.ProductVariantID, productVariants)}",
                    $"{GetCategoryName(GetCategoryID(productID, products), categories)}",
                    $"{GetOutboundShipmentDate(outboundShipmentDetail.OutboundShipmentID, outboundShipments)}",
                    $"{GetOutboundShipmentAddress(outboundShipmentDetail.OutboundShipmentID, outboundShipments)}",
                    outboundShipmentDetail.OutboundShipmentDetailAmount
                );
            }

            if (includeStockTransfers)
            {
                stockTransfers = GetRelevantOutboundStockTransfers(GetWarehouseID(warehouseName), startDate, endDate);
                stockTransferDetails = GetRelevantStockTransferDetails(stockTransfers);
                warehouses = GetRelevantWarehouses(stockTransfers);

                dataTable.Columns[5].ColumnName = "To";
            }
            foreach (StockTransferDetail stockTransferDetail in stockTransferDetails)
            {

                int productID = GetProductID(stockTransferDetail.ProductVariantID, productVariants);

                dataTable.Rows.Add(
                    $"Stock Transfer",
                    $"P{productID}-V{stockTransferDetail.ProductVariantID}",
                    $"{GetProductName(productID, products)} {GetVariantColor(stockTransferDetail.ProductVariantID, productVariants)} {GetVariantSize(stockTransferDetail.ProductVariantID, productVariants)}",
                    $"{GetCategoryName(GetCategoryID(productID, products), categories)}",
                    $"{GetStockTransferDate(stockTransferDetail.StockTransferID, stockTransfers)}",
                    $"{GetWarehouseName(GetToWarehouseID(stockTransferDetail.StockTransferID, stockTransfers), warehouses)}",
                    stockTransferDetail.StockTransferDetailAmount
                );
            }

            // sort by date from newest to oldest
            dataTable = SortDataTable.ByColumn(dataTable, 4, true);
            dataTable = SortDataTable.ClearDirectionArrow(dataTable);

            return dataTable;
        }

        private static int GetWarehouseID(string warehouseName)
        {
            List<Warehouse> warehouses = GetWarehouses();
            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseName == warehouseName) ?? new Warehouse(0, "", 0);
            return warehouse.WarehouseID;
        }

        internal static List<InboundShipment> GetInboundShipments()
        {
            List<InboundShipment> inboundShipments = Program.Warehouse.InboundShipmentTable.InboundShipments ?? new List<InboundShipment>();
            return inboundShipments;
        }

        internal static List<InboundShipmentDetail> GetInboundShipmentDetails()
        {
            List<InboundShipmentDetail> inboundShipmentDetails = Program.Warehouse.InboundShipmentDetailTable.InboundShipmentDetails ?? new List<InboundShipmentDetail>();
            return inboundShipmentDetails;
        }

        internal static List<StockTransfer> GetStockTransfers()
        {
            List<StockTransfer> stockTransfers = Program.Warehouse.StockTransferTable.StockTransfers ?? new List<StockTransfer>();
            return stockTransfers;
        }

        internal static List<StockTransferDetail> GetStockTransferDetails()
        {
            List<StockTransferDetail> stockTransferDetails = Program.Warehouse.StockTransferDetailTable.StockTransferDetails ?? new List<StockTransferDetail>();
            return stockTransferDetails;
        }

        internal static List<Product> GetProducts()
        {
            List<Product> products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            return products;
        }

        internal static List<ProductVariant> GetProductVariants()
        {
            List<ProductVariant> productVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();
            return productVariants;
        }

        internal static List<Category> GetCategories()
        {
            List<Category> categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            return categories;
        }

        internal static List<Supplier> GetSuppliers()
        {
            List<Supplier> suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();
            return suppliers;
        }

        private static List<InboundShipment> GetRelevantInboundShipments(int warehouseID, DateTime startDate, DateTime endDate)
        {
            List<InboundShipment> inboundShipments = GetInboundShipments();

            var relevantInboundShipments = inboundShipments
                .Where(i => i.WarehouseID == warehouseID && i.InboundShipmentStartingDate >= startDate && i.InboundShipmentStartingDate <= endDate && i.InboundShipmentStatus == "Completed")
                .ToList();

            return relevantInboundShipments;
        }

        private static List<InboundShipmentDetail> GetRelevantInboundShipmentDetails(List<InboundShipment> relevantInboundShipments)
        {
            List<InboundShipmentDetail> inboundShipmentDetails = GetInboundShipmentDetails();
            List<int> relevantShipmentIDs = relevantInboundShipments.Select(i => i.InboundShipmentID).ToList();

            List<InboundShipmentDetail> relevantInboundShipmentDetails = inboundShipmentDetails
                .Where(isd => relevantShipmentIDs.Contains(isd.InboundShipmentID))
                .Distinct()
                .ToList();

            return relevantInboundShipmentDetails;
        }

        private static List<ProductVariant> GetRelevantProductVariants(List<InboundShipmentDetail> relevantInboundShipmentDetails)
        {
            List<ProductVariant> productVariants = GetProductVariants();
            List<int> relevantProductVariantIDs = relevantInboundShipmentDetails.Select(isd => isd.ProductVariantID).ToList();

            List<ProductVariant> relevantProductVariants = productVariants
                .Where(pv => relevantProductVariantIDs.Contains(pv.ProductVariantID))
                .Distinct()
                .ToList();

            return relevantProductVariants;
        }

        private static List<Product> GetRelevantProducts(List<ProductVariant> relevantProductVariants)
        {
            List<Product> products = GetProducts();
            List<int> relevantProductIDs = relevantProductVariants.Select(pv => pv.ProductID).ToList();

            var relevantProducts = products
                .Where(p => relevantProductIDs.Contains(p.ProductID))
                .Distinct()
                .ToList();

            return relevantProducts;
        }

        internal static List<Category> GetRelevantCategories(List<Product> relevantProducts)
        {
            List<Category> categories = GetCategories();
            List<int?> relevantCategoryIDs = relevantProducts.Select(p => p.CategoryID).ToList();

            List<Category> relevantCategories = categories
                .Where(c => relevantCategoryIDs.Contains(c.CategoryID))
                .Distinct()
                .ToList();

            return relevantCategories;
        }

        internal static List<Supplier> GetRelevantSuppliers(List<InboundShipment> relevantInboundShipments)
        {
            List<Supplier> suppliers = GetSuppliers();
            List<int> relevantSupplierIDs = relevantInboundShipments.Select(i => i.SupplierID).ToList();

            var relevantSuppliers = suppliers
                .Where(s => relevantSupplierIDs.Contains(s.SupplierID))
                .ToList();

            return relevantSuppliers;
        }

        private static List<StockTransfer> GetRelevantInboundStockTransfers(int warehouseID, DateTime startDate, DateTime endDate)
        {
            List<StockTransfer> stockTransfers = GetStockTransfers();
            List<StockTransfer> relevantStockTransfers = stockTransfers
                .Where(i => i.ToWarehouseID == warehouseID && i.StockTransferStartingDate >= startDate && i.StockTransferStartingDate <= endDate && i.StockTransferStatus == "Completed")
                .ToList();

            return relevantStockTransfers;
        }

        private static List<StockTransferDetail> GetRelevantStockTransferDetails(List<StockTransfer> relevantStockTransfers)
        {
            List<StockTransferDetail> stockTransferDetails = GetStockTransferDetails();
            List<int> relevantStockTransferIDs = relevantStockTransfers.Select(i => i.StockTransferID).ToList();

            List<StockTransferDetail> relevantStockTransferDetails = stockTransferDetails
                .Where(isd => relevantStockTransferIDs.Contains(isd.StockTransferID))
                .Distinct()
                .ToList();

            return relevantStockTransferDetails;
        }

        internal static int GetProductID(int variantID, List<ProductVariant> relevantProductVariants)
        {
            var variant = relevantProductVariants.FirstOrDefault(v => v.ProductVariantID == variantID);
            return variant?.ProductID ?? 0; // Assuming 0 is an invalid ProductID or default value
        }

        internal static string GetProductName(int productID, List<Product> relevantProducts)
        {
            var product = relevantProducts.FirstOrDefault(p => p.ProductID == productID);
            return product?.ProductName ?? "Unknown Product";
        }

        internal static string GetVariantColor(int variantID, List<ProductVariant> relevantProductVariants)
        {
            var variant = relevantProductVariants.FirstOrDefault(v => v.ProductVariantID == variantID);
            return variant?.ProductVariantColor ?? "Unknown Color";
        }

        internal static string GetVariantSize(int variantID, List<ProductVariant> relevantProductVariants)
        {
            var variant = relevantProductVariants.FirstOrDefault(v => v.ProductVariantID == variantID);
            return variant?.ProductVariantSize ?? "Unknown Size";
        }

        internal static int GetCategoryID(int productID, List<Product> relevantProducts)
        {
            var product = relevantProducts.FirstOrDefault(p => p.ProductID == productID);
            return product?.CategoryID ?? 0; // Assuming 0 is an invalid CategoryID or default value
        }

        internal static string GetCategoryName(int categoryID, List<Category> relevantCategories)
        {
            var category = relevantCategories.FirstOrDefault(c => c.CategoryID == categoryID);
            return category?.CategoryName ?? "Unknown Category";
        }

        internal static DateTime GetInboundShipmentDate(int inboundShipmentID, List<InboundShipment> inboundShipments)
        {
            InboundShipment shipment = inboundShipments.FirstOrDefault(s => s.InboundShipmentID == inboundShipmentID) ?? new InboundShipment(0, 0, 0, null, "", null, 0);
            return shipment.InboundShipmentStartingDate ?? GetDefaultStartDate();
        }

        internal static int GetSupplierID(int inboundShipmentID, List<InboundShipment> inboundShipments)
        {
            var shipment = inboundShipments.FirstOrDefault(s => s.InboundShipmentID == inboundShipmentID);
            return shipment?.SupplierID ?? 0; // Assuming 0 is an invalid SupplierID or default value
        }

        internal static string GetSupplierName(int supplierID, List<Supplier> suppliers)
        {
            var supplier = suppliers.FirstOrDefault(s => s.SupplierID == supplierID);
            return supplier?.SupplierName ?? "Unknown Supplier";
        }

        internal static DateTime GetStockTransferDate(int stockTransferID, List<StockTransfer> stockTransfers)
        {
            StockTransfer stockTransfer = stockTransfers.FirstOrDefault(s => s.StockTransferID == stockTransferID) ?? new StockTransfer(0, 0, 0, null, "", null, 0);
            return stockTransfer.StockTransferStartingDate ?? GetDefaultStartDate();
        }

        internal static int GetFromWarehouseID(int stockTransferID, List<StockTransfer> stockTransfers)
        {
            StockTransfer stockTransfer = stockTransfers.FirstOrDefault(s => s.StockTransferID == stockTransferID) ?? new StockTransfer(0, 0, 0, null, "", null, 0);
            return stockTransfer.FromWarehouseID;
        }

        internal static string GetWarehouseName(int warehouseID, List<Warehouse> warehouses)
        {
            Warehouse warehouse = warehouses.FirstOrDefault(w => w.WarehouseID == warehouseID) ?? new Warehouse(0, "", null);
            return warehouse.WarehouseName;
        }

        internal static List<Warehouse> GetRelevantWarehouses(List<StockTransfer> stockTransfers)
        {
            List<Warehouse> warehouses = GetWarehouses();
            List<int> relevantWarehouseIDs = stockTransfers.Select(w => w.FromWarehouseID).ToList();
            List<Warehouse> relevantWarehouses = warehouses
                .Where(w => relevantWarehouseIDs.Contains(w.WarehouseID))
                .Distinct()
                .ToList();

            return relevantWarehouses;
        }

        internal static List<OutboundShipment> GetOutboundShipments()
        {
            return Program.Warehouse.OutboundShipmentTable.OutboundShipments ?? new List<OutboundShipment>();
        }

        internal static List<OutboundShipmentDetail> GetOutboundShipmentDetails()
        {
            return Program.Warehouse.OutboundShipmentDetailTable.OutboundShipmentDetails ?? new List<OutboundShipmentDetail>();
        }

        private static List<OutboundShipment> GetRelevantOutboundShipments(int warehouseID, DateTime startDate, DateTime endDate)
        {
            return GetOutboundShipments()
                .Where(shipment => shipment.WarehouseID == warehouseID && shipment.OutboundShipmentStartingDate >= startDate && shipment.OutboundShipmentStartingDate <= endDate && shipment.OutboundShipmentStatus == "Completed")
                .ToList();
        }

        private static List<OutboundShipmentDetail> GetRelevantOutboundShipmentDetails(List<OutboundShipment> relevantOutboundShipments)
        {
            var relevantShipmentIDs = relevantOutboundShipments.Select(shipment => shipment.OutboundShipmentID).ToList();
            return GetOutboundShipmentDetails()
                .Where(detail => relevantShipmentIDs.Contains(detail.OutboundShipmentID))
                .ToList();
        }

        private static List<ProductVariant> GetRelevantProductVariants(List<OutboundShipmentDetail> relevantOutboundShipmentDetails)
        {
            var relevantVariantIDs = relevantOutboundShipmentDetails.Select(detail => detail.ProductVariantID).Distinct().ToList();
            List<ProductVariant> productVariants = GetProductVariants();
            List<ProductVariant> relevantProductVariants = productVariants
                .Where(variant => relevantVariantIDs.Contains(variant.ProductVariantID))
                .ToList();

            return relevantProductVariants;
        }

        private static List<StockTransfer> GetRelevantOutboundStockTransfers(int warehouseID, DateTime startDate, DateTime endDate)
        {
            List<StockTransfer> stockTransfers = GetStockTransfers();
            List<StockTransfer> relevantStockTransfers = stockTransfers
                .Where(transfer => transfer.FromWarehouseID == warehouseID && transfer.StockTransferStartingDate >= startDate && transfer.StockTransferStartingDate <= endDate && transfer.StockTransferStatus == "Completed")
                .ToList();
            return relevantStockTransfers;
        }

        internal static int GetToWarehouseID(int stockTransferID, List<StockTransfer> stockTransfers)
        {
            var transfer = stockTransfers.FirstOrDefault(t => t.ToWarehouseID == stockTransferID);
            return transfer?.ToWarehouseID ?? -1;
        }

        internal static DateTime GetOutboundShipmentDate(int outboundShipmentID, List<OutboundShipment> outboundShipments)
        {
            OutboundShipment shipment = outboundShipments.FirstOrDefault(s => s.OutboundShipmentID == outboundShipmentID) ?? new OutboundShipment(0, 0, "", null, "", null, 0);
            return shipment.OutboundShipmentStartingDate ?? GetDefaultStartDate();
        }

        internal static string GetOutboundShipmentAddress(int outboundShipmentID, List<OutboundShipment> outboundShipments)
        {
            OutboundShipment shipment = outboundShipments.FirstOrDefault(s => s.OutboundShipmentID == outboundShipmentID) ?? new OutboundShipment(0, 0, "", null, "", null, 0);
            return shipment.OutboundShipmentAddress;
        }

    }
}
