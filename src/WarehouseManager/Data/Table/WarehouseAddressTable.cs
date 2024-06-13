using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class WarehouseAddressTable
    {
        public List<WarehouseAddress>? WarehouseAddresses { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {{"input_token", token}};
            
            List<List<object>> rawWarehouseAddresses = Procedure.ExecuteReader(connectionString, "read_warehouse_address", inParameters);

            List<WarehouseAddress> warehouseAddresses = new List<WarehouseAddress>();
            foreach (List<object> rawWarehouseAddress in rawWarehouseAddresses)
            {
                WarehouseAddress warehouseAddress = new WarehouseAddress(
                    (int)rawWarehouseAddress[0], 
                    (string)rawWarehouseAddress[1], 
                    (string)rawWarehouseAddress[2],
                    (string)rawWarehouseAddress[3],
                    (string)rawWarehouseAddress[4],
                    (string)rawWarehouseAddress[5]
                );
                warehouseAddresses.Add(warehouseAddress);
            }

            this.WarehouseAddresses = warehouseAddresses;
        }

        public void Add(string connectionString, string token, int warehouseAddressID, string warehouseAddressAddress, string warehouseAddressDistrict, string warehouseAddressPostalCode, string warehouseAddressCity, string warehouseAddressCountry)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"new_warehouse_address_address", warehouseAddressAddress},
                {"new_warehouse_address_district", warehouseAddressDistrict},
                {"new_warehouse_address_postal_code", warehouseAddressPostalCode},
                {"new_warehouse_address_city", warehouseAddressCity},
                {"new_warehouse_address_country", warehouseAddressCountry}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_warehouse_address", inParameters);

            WarehouseAddress warehouseAddress = new WarehouseAddress(warehouseAddressID, warehouseAddressAddress, warehouseAddressDistrict, warehouseAddressPostalCode, warehouseAddressCity, warehouseAddressCountry);

            this.WarehouseAddresses ??= new List<WarehouseAddress>();
            this.WarehouseAddresses.Add(warehouseAddress);
        }

        
    }
}