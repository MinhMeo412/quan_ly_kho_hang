using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddSupplierLogic
    {
        public static void Save(string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            AddSupplier(supplierName, supplierDescription, supplierAddress, supplierEmail, supplierPhoneNumber, supplierWebsite);
        }

        private static void AddSupplier(string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            int supplierID = GetCurrentHighestSupplierID() + 1;
            Program.Warehouse.SupplierTable.Add(supplierID, supplierName, supplierDescription, supplierAddress, supplierEmail, supplierPhoneNumber, supplierWebsite);
        }

        private static int GetCurrentHighestSupplierID()
        {
            List<Supplier> suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();
            int hightestSupplierID = suppliers.Max(s => s.SupplierID);
            return hightestSupplierID;
        }
    }
}