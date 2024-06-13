using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class InboundShipmentDetailTable
    {
        public List<InboundShipmentDetail>? InboundShipmentDetails { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {{"input_token", token}};
            
            List<List<object>> rawInboundShipmentDetails = Procedure.ExecuteReader(connectionString, "read_inbound_shipment_detail", inParameters);

            List<InboundShipmentDetail> inboundShipmentDetails = new List<InboundShipmentDetail>();
            foreach (List<object> rawInboundShipmentDetail in rawInboundShipmentDetails)
            {
                InboundShipmentDetail inboundShipmentDetail = new InboundShipmentDetail(
                    (int)rawInboundShipmentDetail[0], // inbound_shipment_id
                    (int)rawInboundShipmentDetail[1], // product_variant_id
                    (int)rawInboundShipmentDetail[2]  // inbound_shipment_detail_amount
                );
                inboundShipmentDetails.Add(inboundShipmentDetail);
            }

            this.InboundShipmentDetails = inboundShipmentDetails;
        }

        public void Add(string connectionString, string token, int inboundShipmentID, int productVariantID, int inboundShipmentDetailAmount)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"inbound_shipment_id", inboundShipmentID},
                {"product_variant_id", productVariantID},
                {"inbound_shipment_detail_amount", inboundShipmentDetailAmount}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_inbound_shipment_detail", inParameters);

            InboundShipmentDetail inboundShipmentDetail = new InboundShipmentDetail(inboundShipmentID, productVariantID, inboundShipmentDetailAmount);

            this.InboundShipmentDetails ??= new List<InboundShipmentDetail>();
            this.InboundShipmentDetails.Add(inboundShipmentDetail);
        }
    }
}