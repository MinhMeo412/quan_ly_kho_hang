namespace WarehouseManager.Data.Entity
{
    class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; } = "";
        public string? SupplierDescription { get; set; }
        public string? SupplierAddress { get; set; }
        public string? SupplierEmail { get; set; }
        public string? SupplierPhoneNumber { get; set; }
        public string? SupplierWebsite { get; set; }
    }
}