using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddSupplierLogic
    {
        public static void AddSupplier(int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            Program.Warehouse.SupplierTable.Add(supplierID, supplierName, supplierDescription, supplierAddress, supplierEmail, supplierPhoneNumber, supplierWebsite);
        }


        public static void Save(string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            int supplierID = GetCurrentHighestSupplierID() + 1;
            AddSupplier(supplierID, supplierName, supplierDescription, supplierAddress, supplierEmail, supplierPhoneNumber, supplierWebsite);
        }

        private static int GetCurrentHighestSupplierID()
        {
            List<Supplier> suppliers = Program.Warehouse.SupplierTable.Suppliers ?? new List<Supplier>();
            int hightestSupplierID = suppliers.Max(s => s.SupplierID);
            return hightestSupplierID;
        }
    }
}