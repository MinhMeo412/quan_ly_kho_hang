using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class WarehouseTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;
        public List<Warehouse>? Warehouses { get; private set; }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawWarehouses = Procedure.ExecuteReader(this.ConnectionString, "read_warehouse", inParameters);

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

        public void Add(int warehouseID, string warehouseName, int? warehouseAddressID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_warehouse_id", warehouseID},
                {"input_warehouse_name", warehouseName},
                {"input_warehouse_address_id", warehouseAddressID}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_warehouse", inParameters);
        }

        public void Update(int warehouseID, string warehouseName, int? warehouseAddressID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_warehouse_id", warehouseID},
                {"input_warehouse_name", warehouseName},
                {"input_warehouse_address_id", warehouseAddressID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_warehouse", inParameters);
        }

        public void Delete(int warehouseID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_warehouse_id", warehouseID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_warehouse", inParameters);
        }
    }
}
