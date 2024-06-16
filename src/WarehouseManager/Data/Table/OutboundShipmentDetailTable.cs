using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class OutboundShipmentDetailTable
    {
        public List<OutboundShipmentDetail>? OutboundShipmentDetails { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawOutboundShipmentDetails = Procedure.ExecuteReader(connectionString, "read_outbound_shipment_detail", inParameters);

            List<OutboundShipmentDetail> outboundShipmentDetails = new List<OutboundShipmentDetail>();
            foreach (List<object?> rawDetail in rawOutboundShipmentDetails)
            {
                OutboundShipmentDetail detail = new OutboundShipmentDetail(
                    (int)(rawDetail[0] ?? 0),
                    (int)(rawDetail[1] ?? 0),
                    (int)(rawDetail[2] ?? 0)
                );
                outboundShipmentDetails.Add(detail);
            }

            this.OutboundShipmentDetails = outboundShipmentDetails;
        }

        public void Add(string connectionString, string token, int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_product_variant_id", productVariantID},
                {"input_outbound_shipment_detail_amount", outboundShipmentDetailAmount}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_outbound_shipment_detail", inParameters);

            OutboundShipmentDetail detail = new OutboundShipmentDetail(
                outboundShipmentID, productVariantID, outboundShipmentDetailAmount);

            this.OutboundShipmentDetails ??= new List<OutboundShipmentDetail>();
            this.OutboundShipmentDetails.Add(detail);
        }

        public void Update(string connectionString, string token, int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_product_variant_id", productVariantID},
                {"input_outbound_shipment_detail_amount", outboundShipmentDetailAmount}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_outbound_shipment_detail", inParameters);

            var detail = this.OutboundShipmentDetails?.FirstOrDefault(d => d.OutboundShipmentID == outboundShipmentID && d.ProductVariantID == productVariantID);
            if (detail != null)
            {
                detail.OutboundShipmentDetailAmount = outboundShipmentDetailAmount;
            }
        }

        public void Delete(string connectionString, string token, int outboundShipmentID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_outbound_shipment_detail", inParameters);

            var detail = this.OutboundShipmentDetails?.FirstOrDefault(d => d.OutboundShipmentID == outboundShipmentID && d.ProductVariantID == productVariantID);
            if (detail != null)
            {
                this.OutboundShipmentDetails ??= new List<OutboundShipmentDetail>();
                this.OutboundShipmentDetails.Remove(detail);
            }
        }
    }
}
