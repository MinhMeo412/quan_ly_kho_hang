using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class WarehouseAddressTable
    {
        public List<WarehouseAddress>? WarehouseAddresses { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawWarehouseAddresses = Procedure.ExecuteReader(connectionString, "read_warehouse_address", inParameters);

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

        public void Add(string connectionString, string token, int warehouseAddressID, string warehouseAddressAddress, string? warehouseAddressDistrict, string? warehouseAddressPostalCode, string? warehouseAddressCity, string? warehouseAddressCountry)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_warehouse_address_id", warehouseAddressID},
                {"input_warehouse_address_address", warehouseAddressAddress},
                {"input_warehouse_address_district", warehouseAddressDistrict},
                {"input_warehouse_address_postal_code", warehouseAddressPostalCode},
                {"input_warehouse_address_city", warehouseAddressCity},
                {"input_warehouse_address_country", warehouseAddressCountry}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_warehouse_address", inParameters);

            WarehouseAddress warehouseAddress = new WarehouseAddress(warehouseAddressID, warehouseAddressAddress, warehouseAddressDistrict, warehouseAddressPostalCode, warehouseAddressCity, warehouseAddressCountry);

            this.WarehouseAddresses ??= new List<WarehouseAddress>();
            this.WarehouseAddresses.Add(warehouseAddress);
        }

        public void Update(string connectionString, string token, int warehouseAddressID, string warehouseAddressAddress, string? warehouseAddressDistrict, string? warehouseAddressPostalCode, string? warehouseAddressCity, string? warehouseAddressCountry)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_warehouse_address_id", warehouseAddressID},
                {"input_warehouse_address_address", warehouseAddressAddress},
                {"input_warehouse_address_district", warehouseAddressDistrict},
                {"input_warehouse_address_postal_code", warehouseAddressPostalCode},
                {"input_warehouse_address_city", warehouseAddressCity},
                {"input_warehouse_address_country", warehouseAddressCountry}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_warehouse_address", inParameters);

            var warehouseAddress = this.WarehouseAddresses?.FirstOrDefault(wa => wa.WarehouseAddressID == warehouseAddressID);
            if (warehouseAddress != null)
            {
                warehouseAddress.WarehouseAddressAddress = warehouseAddressAddress;
                warehouseAddress.WarehouseAddressDistrict = warehouseAddressDistrict;
                warehouseAddress.WarehouseAddressPostalCode = warehouseAddressPostalCode;
                warehouseAddress.WarehouseAddressCity = warehouseAddressCity;
                warehouseAddress.WarehouseAddressCountry = warehouseAddressCountry;
            }
        }

        public void Delete(string connectionString, string token, int warehouseAddressID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_warehouse_address_id", warehouseAddressID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_warehouse_address", inParameters);

            var warehouseAddress = this.WarehouseAddresses?.FirstOrDefault(wa => wa.WarehouseAddressID == warehouseAddressID);
            if (warehouseAddress != null)
            {
                this.WarehouseAddresses ??= new List<WarehouseAddress>();
                this.WarehouseAddresses.Remove(warehouseAddress);
            }
        }
    }
}
