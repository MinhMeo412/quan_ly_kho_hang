using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class OutboundShipmentDetailTable
    {
        public List<OutboundShipmentDetail>? OutboundShipmentDetails { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {{"input_token", token}};
            
            List<List<object>> rawOutboundShipmentDetails = Procedure.ExecuteReader(connectionString, "read_outbound_shipment_detail", inParameters);

            List<OutboundShipmentDetail> outboundShipmentDetails = new List<OutboundShipmentDetail>();
            foreach (List<object> rawOutboundShipmentDetail in rawOutboundShipmentDetails)
            {
                OutboundShipmentDetail outboundShipmentDetail = new OutboundShipmentDetail(
                    (int)rawOutboundShipmentDetail[0], // outbound_shipment_id
                    (int)rawOutboundShipmentDetail[1], // product_variant_id
                    (int)rawOutboundShipmentDetail[2]  // outbound_shipment_detail_amount
                );
                outboundShipmentDetails.Add(outboundShipmentDetail);
            }

            this.OutboundShipmentDetails = outboundShipmentDetails;
        }

        public void Add(string connectionString, string token, int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"outbound_shipment_id", outboundShipmentID},
                {"product_variant_id", productVariantID},
                {"outbound_shipment_detail_amount", outboundShipmentDetailAmount}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_outbound_shipment_detail", inParameters);

            OutboundShipmentDetail outboundShipmentDetail = new OutboundShipmentDetail(outboundShipmentID, productVariantID, outboundShipmentDetailAmount);

            this.OutboundShipmentDetails ??= new List<OutboundShipmentDetail>();
            this.OutboundShipmentDetails.Add(outboundShipmentDetail);
        }

        public void Update(string connectionString, string token, int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"outbound_shipment_id", outboundShipmentID},
                {"product_variant_id", productVariantID},
                {"new_outbound_shipment_detail_amount", outboundShipmentDetailAmount}
            };
            Procedure.ExecuteNonQuery(connectionString, "update_outbound_shipment_detail", inParameters);

            var outboundShipmentDetail = this.OutboundShipmentDetails?.FirstOrDefault(temp => temp.OutboundShipmentID == outboundShipmentID && temp.ProductVariantID == productVariantID);
            if (outboundShipmentDetail != null)
            {
                outboundShipmentDetail.OutboundShipmentDetailAmount = outboundShipmentDetailAmount;
            }
        }

        public void Delete(string connectionString, string token, int outboundShipmentID, int productVariantID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"target_shipment_id", outboundShipmentID},
                {"target_variant_id", productVariantID}
            };
            Procedure.ExecuteNonQuery(connectionString, "delete_outbound_shipment_detail", inParameters);

            var outboundShipmentDetail = this.OutboundShipmentDetails?.FirstOrDefault(temp => temp.OutboundShipmentID == outboundShipmentID && temp.ProductVariantID == productVariantID);
            if (outboundShipmentDetail != null)
            {
                this.OutboundShipmentDetails ??= new List<OutboundShipmentDetail>();
                this.OutboundShipmentDetails.Remove(outboundShipmentDetail);
            }
        }
    }
}