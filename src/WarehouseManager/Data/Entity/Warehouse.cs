namespace WarehouseManager.Data.Entity
{
    class Warehouse
    {
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; } = "";
        public int? WarehouseAddressID { get; set; }
    }
}