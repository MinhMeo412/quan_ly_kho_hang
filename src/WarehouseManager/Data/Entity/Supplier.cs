namespace WarehouseManager.Data.Entity
{
    class Supplier(int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
    {
        public int SupplierID { get; set; } = supplierID;
        public string SupplierName { get; set; } = supplierName;
        public string? SupplierDescription { get; set; } = supplierDescription;
        public string? SupplierAddress { get; set; } = supplierAddress;
        public string? SupplierEmail { get; set; } = supplierEmail;
        public string? SupplierPhoneNumber { get; set; } = supplierPhoneNumber;
        public string? SupplierWebsite { get; set; } = supplierWebsite;
    }
}