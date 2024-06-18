using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class OutboundShipmentDetailTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;
        public List<OutboundShipmentDetail>? OutboundShipmentDetails { get; private set; }

        private void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawOutboundShipmentDetails = Procedure.ExecuteReader(this.ConnectionString, "read_outbound_shipment_detail", inParameters);

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

        public void Add(int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_product_variant_id", productVariantID},
                {"input_outbound_shipment_detail_amount", outboundShipmentDetailAmount}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_outbound_shipment_detail", inParameters);
        }

        public void Update(int outboundShipmentID, int productVariantID, int outboundShipmentDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_product_variant_id", productVariantID},
                {"input_outbound_shipment_detail_amount", outboundShipmentDetailAmount}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_outbound_shipment_detail", inParameters);
        }

        public void Delete(int outboundShipmentID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_outbound_shipment_detail", inParameters);
        }
    }
}
