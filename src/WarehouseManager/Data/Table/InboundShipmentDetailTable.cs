using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class InboundShipmentDetailTable
    {
        public List<InboundShipmentDetail>? InboundShipmentDetails { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawInboundShipmentDetails = Procedure.ExecuteReader(connectionString, "read_inbound_shipment_detail", inParameters);

            List<InboundShipmentDetail> inboundShipmentDetails = new List<InboundShipmentDetail>();
            foreach (List<object?> rawDetail in rawInboundShipmentDetails)
            {
                InboundShipmentDetail detail = new InboundShipmentDetail(
                    (int)(rawDetail[0] ?? 0),
                    (int)(rawDetail[1] ?? 0),
                    (int)(rawDetail[2] ?? 0)
                );
                inboundShipmentDetails.Add(detail);
            }

            this.InboundShipmentDetails = inboundShipmentDetails;
        }

        public void Add(string connectionString, string token, int inboundShipmentID, int productVariantID, int inboundShipmentDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_product_variant_id", productVariantID},
                {"input_inbound_shipment_detail_amount", inboundShipmentDetailAmount}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_inbound_shipment_detail", inParameters);

            InboundShipmentDetail detail = new InboundShipmentDetail(
                inboundShipmentID, productVariantID, inboundShipmentDetailAmount);

            this.InboundShipmentDetails ??= new List<InboundShipmentDetail>();
            this.InboundShipmentDetails.Add(detail);
        }

        public void Update(string connectionString, string token, int inboundShipmentID, int productVariantID, int inboundShipmentDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_product_variant_id", productVariantID},
                {"input_inbound_shipment_detail_amount", inboundShipmentDetailAmount}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_inbound_shipment_detail", inParameters);

            var detail = this.InboundShipmentDetails?.FirstOrDefault(d => d.InboundShipmentID == inboundShipmentID && d.ProductVariantID == productVariantID);
            if (detail != null)
            {
                detail.InboundShipmentDetailAmount = inboundShipmentDetailAmount;
            }
        }

        public void Delete(string connectionString, string token, int inboundShipmentID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_inbound_shipment_detail", inParameters);

            var detail = this.InboundShipmentDetails?.FirstOrDefault(d => d.InboundShipmentID == inboundShipmentID && d.ProductVariantID == productVariantID);
            if (detail != null)
            {
                this.InboundShipmentDetails ??= new List<InboundShipmentDetail>();
                this.InboundShipmentDetails.Remove(detail);
            }
        }
    }
}
