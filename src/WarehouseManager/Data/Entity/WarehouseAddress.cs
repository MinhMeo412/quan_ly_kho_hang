namespace WarehouseManager.Data.Entity
{
    class WarehouseAddress(int warehouseAddressID, string warehouseAddressAddress, string? warehouseAddressDistrict, string? warehouseAddressPostalCode, string? warehouseAddressCity, string? warehouseAddressCountry)
    {
        public int WarehouseAddressID { get; set; } = warehouseAddressID;
        public string WarehouseAddressAddress { get; set; } = warehouseAddressAddress;
        public string? WarehouseAddressDistrict { get; set; } = warehouseAddressDistrict;
        public string? WarehouseAddressPostalCode { get; set; } = warehouseAddressPostalCode;
        public string? WarehouseAddressCity { get; set; } = warehouseAddressCity;
        public string? WarehouseAddressCountry { get; set; } = warehouseAddressCountry;
    }
}