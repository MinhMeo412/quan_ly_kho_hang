using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class InboundShipmentTable
    {
        public List<InboundShipment>? InboundShipments { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawInboundShipments = Procedure.ExecuteReader(connectionString, "read_inbound_shipment", inParameters);

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

        public void Add(string connectionString, string token, int inboundShipmentID, int? supplierID, int warehouseID, DateTime? inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_supplier_id", supplierID},
                {"input_warehouse_id", warehouseID},
                {"input_inbound_shipment_starting_date", inboundShipmentStartingDate},
                {"input_inbound_shipment_status", inboundShipmentStatus},
                {"input_inbound_shipment_description", inboundShipmentDescription},
                {"input_user_id", userID}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_inbound_shipment", inParameters);

            InboundShipment shipment = new InboundShipment(
                inboundShipmentID, supplierID, warehouseID, inboundShipmentStartingDate, inboundShipmentStatus, inboundShipmentDescription, userID);

            this.InboundShipments ??= new List<InboundShipment>();
            this.InboundShipments.Add(shipment);
        }

        public void Update(string connectionString, string token, int inboundShipmentID, int? supplierID, int warehouseID, DateTime? inboundShipmentStartingDate, string inboundShipmentStatus, string? inboundShipmentDescription, int? userID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inbound_shipment_id", inboundShipmentID},
                {"input_supplier_id", supplierID},
                {"input_warehouse_id", warehouseID},
                {"input_inbound_shipment_starting_date", inboundShipmentStartingDate},
                {"input_inbound_shipment_status", inboundShipmentStatus},
                {"input_inbound_shipment_description", inboundShipmentDescription},
                {"input_user_id", userID}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_inbound_shipment", inParameters);

            var shipment = this.InboundShipments?.FirstOrDefault(s => s.InboundShipmentID == inboundShipmentID);
            if (shipment != null)
            {
                shipment.SupplierID = supplierID;
                shipment.WarehouseID = warehouseID;
                shipment.InboundShipmentStartingDate = inboundShipmentStartingDate;
                shipment.InboundShipmentStatus = inboundShipmentStatus;
                shipment.InboundShipmentDescription = inboundShipmentDescription;
                shipment.UserID = userID;
            }
        }

        public void Delete(string connectionString, string token, int inboundShipmentID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_inbound_shipment_id", inboundShipmentID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_inbound_shipment", inParameters);

            var shipment = this.InboundShipments?.FirstOrDefault(s => s.InboundShipmentID == inboundShipmentID);
            if (shipment != null)
            {
                this.InboundShipments ??= new List<InboundShipment>();
                this.InboundShipments.Remove(shipment);
            }
        }
    }
}
