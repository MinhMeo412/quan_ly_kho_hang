using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class OutboundShipmentTable
    {
        public List<OutboundShipment>? OutboundShipments { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawOutboundShipments = Procedure.ExecuteReader(connectionString, "read_outbound_shipment", inParameters);

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
                    (int?)rawShipment[6]
                );
                outboundShipments.Add(shipment);
            }

            this.OutboundShipments = outboundShipments;
        }

        public void Add(string connectionString, string token, int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_warehouse_id", warehouseID},
                {"input_outbound_shipment_address", outboundShipmentAddress},
                {"input_outbound_shipment_starting_date", outboundShipmentStartingDate},
                {"input_outbound_shipment_status", outboundShipmentStatus},
                {"input_outbound_shipment_description", outboundShipmentDescription},
                {"input_user_id", userID}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_outbound_shipment", inParameters);

            OutboundShipment shipment = new OutboundShipment(
                outboundShipmentID,
                warehouseID,
                outboundShipmentAddress,
                outboundShipmentStartingDate,
                outboundShipmentStatus,
                outboundShipmentDescription,
                userID
            );

            this.OutboundShipments ??= new List<OutboundShipment>();
            this.OutboundShipments.Add(shipment);
        }

        public void Update(string connectionString, string token, int outboundShipmentID, int warehouseID, string outboundShipmentAddress, DateTime? outboundShipmentStartingDate, string outboundShipmentStatus, string? outboundShipmentDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_outbound_shipment_id", outboundShipmentID},
                {"input_warehouse_id", warehouseID},
                {"input_outbound_shipment_address", outboundShipmentAddress},
                {"input_outbound_shipment_starting_date", outboundShipmentStartingDate},
                {"input_outbound_shipment_status", outboundShipmentStatus},
                {"input_outbound_shipment_description", outboundShipmentDescription},
                {"input_user_id", userID}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_outbound_shipment", inParameters);

            var shipment = this.OutboundShipments?.FirstOrDefault(s => s.OutboundShipmentID == outboundShipmentID);
            if (shipment != null)
            {
                shipment.WarehouseID = warehouseID;
                shipment.OutboundShipmentAddress = outboundShipmentAddress;
                shipment.OutboundShipmentStartingDate = outboundShipmentStartingDate;
                shipment.OutboundShipmentStatus = outboundShipmentStatus;
                shipment.OutboundShipmentDescription = outboundShipmentDescription;
                shipment.UserID = userID;
            }
        }

        public void Delete(string connectionString, string token, int outboundShipmentID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_outbound_shipment_id", outboundShipmentID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_outbound_shipment", inParameters);

            var shipment = this.OutboundShipments?.FirstOrDefault(s => s.OutboundShipmentID == outboundShipmentID);
            if (shipment != null)
            {
                this.OutboundShipments ??= new List<OutboundShipment>();
                this.OutboundShipments.Remove(shipment);
            }
        }
    }
}
