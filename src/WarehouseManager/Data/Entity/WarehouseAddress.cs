namespace WarehouseManager.Data.Entity
{
    class WarehouseAddress
    {
        public int WarehouseAddressID { get; set; }
        public string WarehouseAddressAddress { get; set; } = "";
        public string? WarehouseAddressDistrict { get; set; }
        public string? WarehouseAddressPostalCode { get; set; }
        public string? WarehouseAddressCity { get; set; }
        public string? WarehouseAddressCountry { get; set; }
    }
}