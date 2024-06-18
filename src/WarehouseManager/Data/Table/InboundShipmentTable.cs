using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class InboundShipmentTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;
        public List<InboundShipment>? InboundShipments { get; private set; }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawInboundShipments = Procedure.ExecuteReader(this.ConnectionString, "read_inbound_shipment", inParameters);

            List<InboundShipment> inboundShipments = new List<InboundShipment>();
            foreach (List<object?> rawShipment in rawInboundShipments)
            {
                InboundShipment shipment = new InboundShipment(
                    (int)(rawShipment[0] ?? 0),
                    (int?)rawShipment[1],
                    (int)(rawShipment[2] ?? 0),
                    (DateTime?)rawShipment[3],
                    (string)(rawShipment[4] ?? ""),
                    (string?)rawShipment[5],
                    (int?)rawShipment[6]
                );
                inboundShipments.Add(shipment);
            }

            this.InboundShipments = inboundShipments;
        }

        public void Add(int inboundShipmentID, int? supplierID, int warehouseID, DateTime? inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_supplier_id", supplierID},
                {"input_warehouse_id", warehouseID},
                {"input_inbound_shipment_starting_date", inboundShipmentStartingDate},
                {"input_inbound_shipment_status", inboundShipmentStatus},
                {"input_inbound_shipment_description", inboundShipmentDescription},
                {"input_user_id", userID}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_inbound_shipment", inParameters);
        }

        public void Update(int inboundShipmentID, int? supplierID, int warehouseID, DateTime? inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_supplier_id", supplierID},
                {"input_warehouse_id", warehouseID},
                {"input_inbound_shipment_starting_date", inboundShipmentStartingDate},
                {"input_inbound_shipment_status", inboundShipmentStatus},
                {"input_inbound_shipment_description", inboundShipmentDescription},
                {"input_user_id", userID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_inbound_shipment", inParameters);
        }

        public void Delete(int inboundShipmentID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_inbound_shipment_id", inboundShipmentID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_inbound_shipment", inParameters);
        }
    }
}
