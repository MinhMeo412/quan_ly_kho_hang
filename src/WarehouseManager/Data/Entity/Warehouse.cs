namespace WarehouseManager.Data.Entity
{
    class Warehouse(int warehouseID, string warehouseName, int? warehouseAddressID)
    {
        public int WarehouseID { get; set; } = warehouseID;
        public string WarehouseName { get; set; } = warehouseName;
        public int? WarehouseAddressID { get; set; } = warehouseAddressID;
    }
}