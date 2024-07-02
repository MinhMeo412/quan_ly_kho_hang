using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class OutboundShipmentTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<OutboundShipment>? _outboundShipments;
        public List<OutboundShipment>? OutboundShipments
        {
            get
            {
                this.Load();
                return this._outboundShipments;
            }
            private set
            {
                this._outboundShipments = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawOutboundShipments = Procedure.ExecuteReader(this.ConnectionString, "read_outbound_shipment", inParameters);

            List<OutboundShipment> outboundShipments = new List<OutboundShipment>();
            foreach (List<object?> rawShipment in rawOutboundShipments)
            {
                OutboundShipment shipment = new OutboundShipment(
                    (int)(rawShipment[0] ?? 0),
                    (int)(rawShipment[1] ?? 0),
                    (string)(rawShipment[2] ?? ""),
                    (DateTime?)rawShipment[3],
                    (string)(rawShipment[4] ?? ""),
                    (string?)rawShipment[5],
                    (int)(rawShipment[6] ?? 0)
                );
                outboundShipments.Add(shipment);
            }

            this.OutboundShipments = outboundShipments;
        }

        public void Add(int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_warehouse_id", warehouseID},
                {"input_outbound_shipment_address", outboundShipmentAddress},
                {"input_outbound_shipment_starting_date", outboundShipmentStartingDate},
                {"input_outbound_shipment_status", outboundShipmentStatus},
                {"input_outbound_shipment_description", outboundShipmentDescription},
                {"input_user_id", userID}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_outbound_shipment", inParameters);
        }

        public void Update(int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_warehouse_id", warehouseID},
                {"input_outbound_shipment_address", outboundShipmentAddress},
                {"input_outbound_shipment_starting_date", outboundShipmentStartingDate},
                {"input_outbound_shipment_status", outboundShipmentStatus},
                {"input_outbound_shipment_description", outboundShipmentDescription},
                {"input_user_id", userID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_outbound_shipment", inParameters);
        }

        public void Delete(int outboundShipmentID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_outbound_shipment_id", outboundShipmentID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_outbound_shipment", inParameters);
        }
    }
}
