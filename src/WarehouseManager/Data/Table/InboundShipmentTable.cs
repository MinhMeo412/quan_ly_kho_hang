using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class InboundShipmentTable
    {
        public List<InboundShipment>? InboundShipments { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {{"input_token", token}};
            
            List<List<object>> rawInboundShipments = Procedure.ExecuteReader(connectionString, "read_inbound_shipment", inParameters);

            List<InboundShipment> inboundShipments = new List<InboundShipment>();
            foreach (List<object> rawInboundShipment in rawInboundShipments)
            {
                InboundShipment inboundShipment = new InboundShipment(
                    (int)rawInboundShipment[0],    // inbound_shipment_id
                    (int?)rawInboundShipment[1],   // supplier_id (nullable)
                    (int)rawInboundShipment[2],    // warehouse_id
                    (DateTime)rawInboundShipment[3],// inbound_shipment_starting_date
                    (string)rawInboundShipment[4], // inbound_shipment_status
                    (string)rawInboundShipment[5], // inbound_shipment_description
                    (int?)rawInboundShipment[6]    // user_id (nullable)
                );
                inboundShipments.Add(inboundShipment);
            }

            this.InboundShipments = inboundShipments;
        }

        public void Add(string connectionString, string token, int inboundShipmentID, int supplierID, int warehouseID, DateTime inboundShipmentStartingDate, string inboundShipmentStatus, string inboundShipmentDescription, int userID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"supplier_id", supplierID},
                {"warehouse_id", warehouseID},
                {"inbound_shipment_description", inboundShipmentDescription},
                {"user_id", userID}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_inbound_shipment", inParameters);

            InboundShipment inboundShipment = new InboundShipment(inboundShipmentID, supplierID, warehouseID, inboundShipmentStartingDate, inboundShipmentStatus, inboundShipmentDescription, userID);

            this.InboundShipments ??= new List<InboundShipment>();
            this.InboundShipments.Add(inboundShipment);
        }
    }
}