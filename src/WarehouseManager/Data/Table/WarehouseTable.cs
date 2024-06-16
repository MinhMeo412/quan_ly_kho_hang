using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class WarehouseTable
    {
        public List<Warehouse>? Warehouses { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawWarehouses = Procedure.ExecuteReader(connectionString, "read_warehouse", inParameters);

            List<Warehouse> warehouses = new List<Warehouse>();
            foreach (List<object?> rawWarehouse in rawWarehouses)
            {
                Warehouse warehouse = new Warehouse(
                    (int)(rawWarehouse[0] ?? 0),
                    (string)(rawWarehouse[1] ?? ""),
                    (int?)rawWarehouse[2]
                );
                warehouses.Add(warehouse);
            }

            this.Warehouses = warehouses;
        }

        public void Add(string connectionString, string token, int warehouseID, string warehouseName, int? warehouseAddressID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_warehouse_id", warehouseID},
                {"input_warehouse_name", warehouseName},
                {"input_warehouse_address_id", warehouseAddressID}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_warehouse", inParameters);

            Warehouse warehouse = new Warehouse(warehouseID, warehouseName, warehouseAddressID);

            this.Warehouses ??= new List<Warehouse>();
            this.Warehouses.Add(warehouse);
        }

        public void Update(string connectionString, string token, int warehouseID, string warehouseName, int? warehouseAddressID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_warehouse_id", warehouseID},
                {"input_warehouse_name", warehouseName},
                {"input_warehouse_address_id", warehouseAddressID}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_warehouse", inParameters);

            var warehouse = this.Warehouses?.FirstOrDefault(w => w.WarehouseID == warehouseID);
            if (warehouse != null)
            {
                warehouse.WarehouseName = warehouseName;
                warehouse.WarehouseAddressID = warehouseAddressID;
            }
        }

        public void Delete(string connectionString, string token, int warehouseID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_warehouse_id", warehouseID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_warehouse", inParameters);

            var warehouse = this.Warehouses?.FirstOrDefault(w => w.WarehouseID == warehouseID);
            if (warehouse != null)
            {
                this.Warehouses ??= new List<Warehouse>();
                this.Warehouses.Remove(warehouse);
            }
        }
    }
}
