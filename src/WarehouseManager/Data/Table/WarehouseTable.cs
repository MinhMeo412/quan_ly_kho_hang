using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class WarehouseTable
    {
        public List<Warehouse>? Warehouses { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string,object>? inParameters = new Dictionary<string, object>
            {{"input_token",token}};
            
            List<List<object>> rawWarehouses = Procedure.ExecuteReader(connectionString,"read_warehouse",inParameters);

            List<Warehouses> warehouses = new List<Warehouses>();
            foreach (List<object> rawWarehouse in rawWarehouses)
            {
                Warehouse warehouse = new Warehouse((int)rawWarehouse[0], (string)rawWarehouse[1], (int)rawWarehouse[2]);
                warehouses.Add(warehouse);
            }

            this.Warehouses = warehouses;
        }

        public void Add(string connectionString, string token, int warehouseID, string warehouseName, int warehouseAddressID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"new_warehouse_name", warehouseName},
                {"new_warehouse_address_id", warehouseAddressID}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_warehouse", inParameters);

            Warehouse warehouse = new Warehouse(warehouseID, warehouseName, warehouseAddressID);

            this.Warehouses ??= new List<Warehouse>();
            this.Warehouses.Add(warehouse);
        }
}