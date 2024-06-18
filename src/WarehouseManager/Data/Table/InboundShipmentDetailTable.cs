using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class InboundShipmentDetailTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<InboundShipmentDetail>? _inboundShipmentDetails;
        public List<InboundShipmentDetail>? InboundShipmentDetails
        {
            get
            {
                this.Load();
                return _inboundShipmentDetails;
            }
            private set
            {
                _inboundShipmentDetails = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawInboundShipmentDetails = Procedure.ExecuteReader(this.ConnectionString, "read_inbound_shipment_detail", inParameters);

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

        public void Add(int inboundShipmentID, int productVariantID, int inboundShipmentDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_product_variant_id", productVariantID},
                {"input_inbound_shipment_detail_amount", inboundShipmentDetailAmount}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_inbound_shipment_detail", inParameters);
        }

        public void Update(int inboundShipmentID, int productVariantID, int inboundShipmentDetailAmount)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_product_variant_id", productVariantID},
                {"input_inbound_shipment_detail_amount", inboundShipmentDetailAmount}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_inbound_shipment_detail", inParameters);
        }

        public void Delete(int inboundShipmentID, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_inbound_shipment_detail", inParameters);
        }
    }
}
