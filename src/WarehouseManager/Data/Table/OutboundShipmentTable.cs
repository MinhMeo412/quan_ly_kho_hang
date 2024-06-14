using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class OutboundShipmentTable
    {
        public List<OutboundShipment>? OutboundShipments { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {{"input_token", token}};
            
            List<List<object>> rawOutboundShipments = Procedure.ExecuteReader(connectionString, "read_outbound_shipment", inParameters);

            List<OutboundShipment> outboundShipments = new List<OutboundShipment>();
            foreach (List<object> rawOutboundShipment in rawOutboundShipments)
            {
                OutboundShipment outboundShipment = new OutboundShipment(
                    (int)rawOutboundShipment[0],    // outbound_shipment_id
                    (int)rawOutboundShipment[1],    // warehouse_id
                    (string)rawOutboundShipment[2], // outbound_shipment_address
                    (DateTime)rawOutboundShipment[3],// outbound_shipment_starting_date
                    (string)rawOutboundShipment[4], // outbound_shipment_status
                    (string)rawOutboundShipment[5], // outbound_shipment_description
                    (int?)rawOutboundShipment[6]    // user_id (nullable)
                );
                outboundShipments.Add(outboundShipment);
            }

            this.OutboundShipments = outboundShipments;
        }

        public void Add(string connectionString, string token, int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int? userID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"warehouse_id", warehouseID},
                {"outbound_shipment_address", outboundShipmentAddress},
                {"outbound_shipment_description", outboundShipmentDescription},
                {"user_id", userID}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_outbound_shipment", inParameters);

            OutboundShipment outboundShipment = new OutboundShipment(outboundShipmentID, warehouseID, outboundShipmentAddress, outboundShipmentStartingDate, outboundShipmentStatus, outboundShipmentDescription, userID);

            this.OutboundShipments ??= new List<OutboundShipment>();
            this.OutboundShipments.Add(outboundShipment);
        }

        public void Update(string connectionString, string token, int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int? userID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"outbound_shipment_id", outboundShipmentID},
                {"new_warehouse_id", warehouseID},
                {"new_outbound_shipment_address", outboundShipmentAddress},
                {"new_outbound_shipment_starting_date", outboundShipmentStartingDate},
                {"new_outbound_shipment_status", outboundShipmentStatus},
                {"new_outbound_shipment_description", outboundShipmentDescription},
                {"new_user_id", userID}
            };
            Procedure.ExecuteNonQuery(connectionString, "update_outbound_shipment", inParameters);

            var outboundShipment = this.OutboundShipments?.FirstOrDefault(temp => temp.OutboundShipmentID == outboundShipmentID);
            if (outboundShipment != null)
            {
                outboundShipment.WarehouseID = warehouseID;
                outboundShipment.OutboundShipmentAddress = outboundShipmentAddress;
                outboundShipment.OutboundShipmentStartingDate = outboundShipmentStartingDate;
                outboundShipment.OutboundShipmentStatus = outboundShipmentStatus;
                outboundShipment.OutboundShipmentDescription = outboundShipmentDescription;
                outboundShipment.UserID = userID;
            }
        }

        public void Delete(string connectionString, string token, int outboundShipmentID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"target_shipment_id", outboundShipmentID}
            };
            Procedure.ExecuteNonQuery(connectionString, "delete_outbound_shipment", inParameters);

            var outboundShipment = this.OutboundShipments?.FirstOrDefault(temp => temp.OutboundShipmentID == outboundShipmentID);
            if (outboundShipment != null)
            {
                this.OutboundShipments ??= new List<OutboundShipment>();
                this.OutboundShipments.Remove(outboundShipment);
            }
        }
    }
}