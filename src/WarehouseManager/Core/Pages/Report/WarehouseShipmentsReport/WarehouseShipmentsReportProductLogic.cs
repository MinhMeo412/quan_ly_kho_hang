using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core.Pages
{
    public static class WarehouseShipmentsReportProductLogic
    {
        public static List<string> GetReportOptions()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetReportOptions();
        }

        public static Dictionary<int, string> GetProductList()
        {
            List<Product> products = GetProducts();
            Dictionary<int, string> productDictionary = new Dictionary<int, string>();

            foreach (var product in products)
            {
                productDictionary[product.ProductID] = product.ProductName;
            }

            return productDictionary;
        }

        public static DateTime GetDefaultStartDate()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetDefaultStartDate();
        }

        public static DateTime GetDefaultEndDate()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetDefaultEndDate();
        }

        public static DataTable GetProductExportData(string id, DateTime startDate, DateTime endDate)
        {
            List<Product> products = GetRelevantProducts(int.Parse(id), GetProducts());
            List<ProductVariant> productVariants = GetRelevantProductVariants(products);
            List<Category> categories = GetRelevantCategories(products);
            List<InboundShipmentDetail> inboundShipmentDetails = GetRelevantInboundShipmentDetails(productVariants, startDate, endDate);
            List<OutboundShipmentDetail> outboundShipmentDetails = GetRelevantOutboundShipmentDetails(productVariants, startDate, endDate);
            List<StockTransferDetail> stockTransferDetails = GetRelevantStockTransferDetails(productVariants, startDate, endDate);
            List<InboundShipment> inboundShipments = GetRelevantInboundShipments(inboundShipmentDetails);
            List<OutboundShipment> outboundShipments = GetRelevantOutboundShipments(outboundShipmentDetails);
            List<StockTransfer> stockTransfers = GetRelevantStockTranfers(stockTransferDetails);
            List<Supplier> suppliers = GetRelevantSuppliers(inboundShipments);
            List<Warehouse> warehouses = GetRelevantWarehouses(inboundShipments, outboundShipments, stockTransfers);

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add($"Shipment Type");
            dataTable.Columns.Add($"PID-VID");
            dataTable.Columns.Add($"Product");
            dataTable.Columns.Add($"Category");
            dataTable.Columns.Add($"Date");
            dataTable.Columns.Add($"From");
            dataTable.Columns.Add($"To");
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
                    $"{GetWarehouseName(GetWarehouseID(inboundShipmentDetail.InboundShipmentID, inboundShipments), warehouses)}",
                    inboundShipmentDetail.InboundShipmentDetailAmount
                );
            }

            foreach (OutboundShipmentDetail outboundShipmentDetail in outboundShipmentDetails)
            {
                int productID = GetProductID(outboundShipmentDetail.ProductVariantID, productVariants);

                dataTable.Rows.Add(
                    $"Outbound",
                    $"P{productID}-V{outboundShipmentDetail.ProductVariantID}",
                    $"{GetProductName(productID, products)} {GetVariantColor(outboundShipmentDetail.ProductVariantID, productVariants)} {GetVariantSize(outboundShipmentDetail.ProductVariantID, productVariants)}",
                    $"{GetCategoryName(GetCategoryID(productID, products), categories)}",
                    $"{GetOutboundShipmentDate(outboundShipmentDetail.OutboundShipmentID, outboundShipments)}",
                    $"{GetWarehouseName(GetWarehouseID(outboundShipmentDetail.OutboundShipmentID, outboundShipments), warehouses)}",
                    $"{GetOutboundShipmentAddress(outboundShipmentDetail.OutboundShipmentID, outboundShipments)}",
                    outboundShipmentDetail.OutboundShipmentDetailAmount
                );
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
                    $"{GetWarehouseName(GetToWarehouseID(stockTransferDetail.StockTransferID, stockTransfers), warehouses)}",
                    stockTransferDetail.StockTransferDetailAmount
                );
            }

            // sort by date from newest to oldest
            dataTable = SortDataTable.ByColumn(dataTable, 4, true);
            dataTable = SortDataTable.ClearDirectionArrow(dataTable);

            return dataTable;
        }

        public static DataTable GetProductFileInformation(string id, DateTime startDate, DateTime endDate)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add($"Warehouse Shipments Report: Product", typeof(string));

            dataTable.Rows.Add($"Product: {GetProductName(int.Parse(id), GetProducts())}");
            dataTable.Rows.Add($"Date: {startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}");

            return dataTable;
        }

        public static int GetProductID(string productName, Dictionary<int, string> productDictionary)
        {
            foreach (var kvp in productDictionary)
            {
                if (kvp.Value.Equals(productName, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Key;
                }
            }

            return -1;
        }

        public static int GetProductIndex(string productIDString, Dictionary<int, string> productDictionary)
        {
            int index = 0;
            int productID = int.TryParse(productIDString, out int parsedID) ? parsedID : -1;

            foreach (var kvp in productDictionary)
            {
                if (kvp.Key == productID)
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        private static List<Product> GetProducts()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetProducts();
        }

        private static string GetProductName(int productID, List<Product> relevantProducts)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetProductName(productID, relevantProducts);
        }

        private static List<ProductVariant> GetProductVariants()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetProductVariants();
        }

        private static List<InboundShipmentDetail> GetInboundShipmentDetails()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetInboundShipmentDetails();
        }

        private static List<OutboundShipmentDetail> GetOutboundShipmentDetails()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetOutboundShipmentDetails();
        }

        private static List<StockTransferDetail> GetStockTransferDetails()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetStockTransferDetails();
        }

        private static List<InboundShipment> GetInboundShipments()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetInboundShipments();
        }

        private static List<OutboundShipment> GetOutboundShipments()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetOutboundShipments();
        }

        private static List<StockTransfer> GetStockTransfers()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetStockTransfers();
        }

        private static List<Warehouse> GetWarehouses()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetWarehouses();
        }

        private static List<Product> GetRelevantProducts(int productID, List<Product> products)
        {
            Product product = products.FirstOrDefault(p => p.ProductID == productID) ?? new Product(0, "", null, null, null);
            List<Product> relevantProducts = new List<Product> { product };
            return relevantProducts;
        }

        private static List<ProductVariant> GetRelevantProductVariants(List<Product> products)
        {
            List<int> productIDs = products.Select(p => p.ProductID).ToList();
            List<ProductVariant> productVariants = GetProductVariants();
            return productVariants.Where(variant => productIDs.Contains(variant.ProductID)).ToList();
        }

        private static List<Category> GetRelevantCategories(List<Product> products)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetRelevantCategories(products);
        }

        private static List<InboundShipmentDetail> GetRelevantInboundShipmentDetails(List<ProductVariant> productVariants, DateTime startDate, DateTime endDate)
        {
            List<int> relevantVariantIDs = productVariants.Select(v => v.ProductVariantID).ToList();

            List<InboundShipmentDetail> inboundShipmentDetails = GetInboundShipmentDetails()
                .Where(detail => relevantVariantIDs.Contains(detail.ProductVariantID))
                .ToList();

            List<int> inboundShipmentIDs = inboundShipmentDetails.Select(i => i.InboundShipmentID).ToList();

            List<InboundShipment> inboundShipments = GetInboundShipments().Where(i => inboundShipmentIDs.Contains(i.InboundShipmentID)).ToList();
            List<InboundShipment> relevantInboundShipments = inboundShipments
                .Where(i => i.InboundShipmentStartingDate >= startDate && i.InboundShipmentStartingDate <= endDate && i.InboundShipmentStatus == "Completed")
                .ToList();

            List<int> relevantInboundShipmentIDs = relevantInboundShipments.Select(i => i.InboundShipmentID).ToList();

            List<InboundShipmentDetail> relevantInboundShipmentDetails = inboundShipmentDetails
                .Where(isd => relevantInboundShipmentIDs.Contains(isd.InboundShipmentID))
                .ToList();

            return relevantInboundShipmentDetails;
        }

        private static List<OutboundShipmentDetail> GetRelevantOutboundShipmentDetails(List<ProductVariant> productVariants, DateTime startDate, DateTime endDate)
        {
            List<int> relevantVariantIDs = productVariants.Select(v => v.ProductVariantID).ToList();

            List<OutboundShipmentDetail> outboundShipmentDetails = GetOutboundShipmentDetails()
                .Where(detail => relevantVariantIDs.Contains(detail.ProductVariantID))
                .ToList();

            List<int> outboundShipmentIDs = outboundShipmentDetails.Select(i => i.OutboundShipmentID).ToList();

            List<OutboundShipment> outboundShipments = GetOutboundShipments().Where(i => outboundShipmentIDs.Contains(i.OutboundShipmentID)).ToList();
            List<OutboundShipment> relevantOutboundShipments = outboundShipments
                .Where(i => i.OutboundShipmentStartingDate >= startDate && i.OutboundShipmentStartingDate <= endDate && i.OutboundShipmentStatus == "Completed")
                .ToList();

            List<int> relevantOutboundShipmentIDs = relevantOutboundShipments.Select(i => i.OutboundShipmentID).ToList();

            List<OutboundShipmentDetail> relevantOutboundShipmentDetails = outboundShipmentDetails
                .Where(osd => relevantOutboundShipmentIDs.Contains(osd.OutboundShipmentID))
                .ToList();

            return relevantOutboundShipmentDetails;
        }


        private static List<StockTransferDetail> GetRelevantStockTransferDetails(List<ProductVariant> productVariants, DateTime startDate, DateTime endDate)
        {
            List<int> relevantVariantIDs = productVariants.Select(v => v.ProductVariantID).ToList();

            List<StockTransferDetail> stockTransferDetails = GetStockTransferDetails()
                .Where(detail => relevantVariantIDs.Contains(detail.ProductVariantID))
                .ToList();

            List<int> stockTransferIDs = stockTransferDetails.Select(i => i.StockTransferID).ToList();

            List<StockTransfer> stockTransfers = GetStockTransfers().Where(i => stockTransferIDs.Contains(i.StockTransferID)).ToList();
            List<StockTransfer> relevantStockTransfers = stockTransfers
                .Where(i => i.StockTransferStartingDate >= startDate && i.StockTransferStartingDate <= endDate && i.StockTransferStatus == "Completed")
                .ToList();

            List<int> relevantStockTransferIDs = relevantStockTransfers.Select(i => i.StockTransferID).ToList();

            List<StockTransferDetail> relevantStockTransferDetails = stockTransferDetails
                .Where(std => relevantStockTransferIDs.Contains(std.StockTransferID))
                .ToList();

            return relevantStockTransferDetails;
        }


        private static List<InboundShipment> GetRelevantInboundShipments(List<InboundShipmentDetail> inboundShipmentDetails)
        {
            List<int> shipmentIDs = inboundShipmentDetails.Select(detail => detail.InboundShipmentID).Distinct().ToList();
            List<InboundShipment> inboundShipments = GetInboundShipments();

            return inboundShipments
                .Where(shipment => shipmentIDs.Contains(shipment.InboundShipmentID))
                .ToList();
        }

        private static List<OutboundShipment> GetRelevantOutboundShipments(List<OutboundShipmentDetail> outboundShipmentDetails)
        {
            List<int> shipmentIDs = outboundShipmentDetails.Select(detail => detail.OutboundShipmentID).Distinct().ToList();
            List<OutboundShipment> outboundShipments = GetOutboundShipments();

            return outboundShipments
                .Where(shipment => shipmentIDs.Contains(shipment.OutboundShipmentID))
                .ToList();

        }

        private static List<StockTransfer> GetRelevantStockTranfers(List<StockTransferDetail> stockTransferDetails)
        {
            List<int> transferIDs = stockTransferDetails.Select(detail => detail.StockTransferID).Distinct().ToList();
            List<StockTransfer> stockTransfers = GetStockTransfers();

            return stockTransfers
                .Where(transfer => transferIDs.Contains(transfer.StockTransferID))
                .ToList();
        }

        private static List<Supplier> GetRelevantSuppliers(List<InboundShipment> inboundShipments)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetRelevantSuppliers(inboundShipments);
        }

        private static List<Warehouse> GetRelevantWarehouses(List<InboundShipment> inboundShipments, List<OutboundShipment> outboundShipments, List<StockTransfer> stockTransfers)
        {
            List<int> relevantWarehousesIDs = inboundShipments.Select(i => i.WarehouseID).ToList();
            relevantWarehousesIDs.AddRange(outboundShipments.Select(o => o.WarehouseID).ToList());

            List<Warehouse> warehouses = GetWarehouses();
            List<Warehouse> relevantWarehouses = warehouses.Where(warehouse => relevantWarehousesIDs.Contains(warehouse.WarehouseID)).ToList();
            relevantWarehouses.AddRange(WarehouseShipmentsReportWarehouseLogic.GetRelevantWarehouses(stockTransfers));

            relevantWarehouses = relevantWarehouses.Distinct().ToList();

            return relevantWarehouses;
        }

        private static int GetProductID(int variantID, List<ProductVariant> relevantProductVariants)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetProductID(variantID, relevantProductVariants);
        }

        private static string GetVariantColor(int variantID, List<ProductVariant> relevantProductVariants)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetVariantColor(variantID, relevantProductVariants);
        }

        private static string GetVariantSize(int variantID, List<ProductVariant> relevantProductVariants)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetVariantSize(variantID, relevantProductVariants);
        }

        private static int GetCategoryID(int productID, List<Product> relevantProducts)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetCategoryID(productID, relevantProducts);
        }

        private static string GetCategoryName(int categoryID, List<Category> relevantCategories)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetCategoryName(categoryID, relevantCategories);
        }

        private static DateTime GetInboundShipmentDate(int inboundShipmentID, List<InboundShipment> inboundShipments)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetInboundShipmentDate(inboundShipmentID, inboundShipments);
        }

        private static int GetSupplierID(int inboundShipmentID, List<InboundShipment> inboundShipments)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetSupplierID(inboundShipmentID, inboundShipments);
        }

        private static string GetSupplierName(int supplierID, List<Supplier> suppliers)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetSupplierName(supplierID, suppliers);
        }

        private static string GetWarehouseName(int warehouseID, List<Warehouse> warehouses)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetWarehouseName(warehouseID, warehouses);
        }

        private static DateTime GetOutboundShipmentDate(int outboundShipmentID, List<OutboundShipment> outboundShipments)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetOutboundShipmentDate(outboundShipmentID, outboundShipments);
        }

        private static string GetOutboundShipmentAddress(int outboundShipmentID, List<OutboundShipment> outboundShipments)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetOutboundShipmentAddress(outboundShipmentID, outboundShipments);
        }

        private static DateTime GetStockTransferDate(int stockTransferID, List<StockTransfer> stockTransfers)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetStockTransferDate(stockTransferID, stockTransfers);
        }

        private static int GetFromWarehouseID(int stockTransferID, List<StockTransfer> stockTransfers)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetFromWarehouseID(stockTransferID, stockTransfers);
        }

        private static int GetToWarehouseID(int stockTransferID, List<StockTransfer> stockTransfers)
        {
            return WarehouseShipmentsReportWarehouseLogic.GetToWarehouseID(stockTransferID, stockTransfers);
        }

        private static int GetWarehouseID(int inboundShipmentID, List<InboundShipment> inboundShipments)
        {
            InboundShipment inboundShipment = inboundShipments.FirstOrDefault(i => i.InboundShipmentID == inboundShipmentID) ?? new InboundShipment(0, 0, 0, null, "", null, 0);
            return inboundShipment.WarehouseID;
        }

        private static int GetWarehouseID(int outboundShipmentID, List<OutboundShipment> outboundShipments)
        {
            OutboundShipment inboundShipment = outboundShipments.FirstOrDefault(i => i.OutboundShipmentID == outboundShipmentID) ?? new OutboundShipment(0, 0, "", null, "", null, 0);
            return inboundShipment.WarehouseID;
        }

    }
}
