using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class WarehouseAddressTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;
        public List<WarehouseAddress>? WarehouseAddresses { get; private set; }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawWarehouseAddresses = Procedure.ExecuteReader(this.ConnectionString, "read_warehouse_address", inParameters);

            List<WarehouseAddress> warehouseAddresses = new List<WarehouseAddress>();
            foreach (List<object?> rawWarehouseAddress in rawWarehouseAddresses)
            {
                WarehouseAddress warehouseAddress = new WarehouseAddress(
                    (int)(rawWarehouseAddress[0] ?? 0),
                    (string)(rawWarehouseAddress[1] ?? 0),
                    (string?)rawWarehouseAddress[2],
                    (string?)rawWarehouseAddress[3],
                    (string?)rawWarehouseAddress[4],
                    (string?)rawWarehouseAddress[5]
                );
                warehouseAddresses.Add(warehouseAddress);
            }

            this.WarehouseAddresses = warehouseAddresses;
        }

        public void Add(int warehouseAddressID, string warehouseAddressAddress, string? warehouseAddressDistrict, string? warehouseAddressPostalCode, string? warehouseAddressCity, string? warehouseAddressCountry)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_warehouse_address_id", warehouseAddressID},
                {"input_warehouse_address_address", warehouseAddressAddress},
                {"input_warehouse_address_district", warehouseAddressDistrict},
                {"input_warehouse_address_postal_code", warehouseAddressPostalCode},
                {"input_warehouse_address_city", warehouseAddressCity},
                {"input_warehouse_address_country", warehouseAddressCountry}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_warehouse_address", inParameters);
        }

        public void Update(int warehouseAddressID, string warehouseAddressAddress, string? warehouseAddressDistrict, string? warehouseAddressPostalCode, string? warehouseAddressCity, string? warehouseAddressCountry)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_warehouse_address_id", warehouseAddressID},
                {"input_warehouse_address_address", warehouseAddressAddress},
                {"input_warehouse_address_district", warehouseAddressDistrict},
                {"input_warehouse_address_postal_code", warehouseAddressPostalCode},
                {"input_warehouse_address_city", warehouseAddressCity},
                {"input_warehouse_address_country", warehouseAddressCountry}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_warehouse_address", inParameters);
        }

        public void Delete(int warehouseAddressID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_warehouse_address_id", warehouseAddressID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_warehouse_address", inParameters);
        }
    }
}
