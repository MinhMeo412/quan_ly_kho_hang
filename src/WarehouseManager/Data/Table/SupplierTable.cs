using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class SupplierTable
    {
        public List<Supplier>? Suppliers { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawSuppliers = Procedure.ExecuteReader(connectionString, "read_supplier", inParameters);

            List<Supplier> suppliers = new List<Supplier>();
            foreach (List<object?> rawSupplier in rawSuppliers)
            {
                Supplier supplier = new Supplier(
                    (int)(rawSupplier[0] ?? 0),
                    (string)(rawSupplier[1] ?? ""),
                    (string?)rawSupplier[2],
                    (string?)rawSupplier[3],
                    (string?)rawSupplier[4],
                    (string?)rawSupplier[5],
                    (string?)rawSupplier[6]
                );
                suppliers.Add(supplier);
            }

            this.Suppliers = suppliers;
        }

        public void Add(string connectionString, string token, int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_supplier_id", supplierID},
                {"input_supplier_name", supplierName},
                {"input_supplier_description", supplierDescription},
                {"input_supplier_address", supplierAddress},
                {"input_supplier_email", supplierEmail},
                {"input_supplier_phone_number", supplierPhoneNumber},
                {"input_supplier_website", supplierWebsite}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_supplier", inParameters);

            Supplier supplier = new Supplier(supplierID, supplierName, supplierDescription, supplierAddress, supplierEmail, supplierPhoneNumber, supplierWebsite);

            this.Suppliers ??= new List<Supplier>();
            this.Suppliers.Add(supplier);
        }

        public void Update(string connectionString, string token, int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_supplier_id", supplierID},
                {"input_supplier_name", supplierName},
                {"input_supplier_description", supplierDescription},
                {"input_supplier_address", supplierAddress},
                {"input_supplier_email", supplierEmail},
                {"input_supplier_phone_number", supplierPhoneNumber},
                {"input_supplier_website", supplierWebsite}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_supplier", inParameters);

            var supplier = this.Suppliers?.FirstOrDefault(s => s.SupplierID == supplierID);
            if (supplier != null)
            {
                supplier.SupplierName = supplierName;
                supplier.SupplierDescription = supplierDescription;
                supplier.SupplierAddress = supplierAddress;
                supplier.SupplierEmail = supplierEmail;
                supplier.SupplierPhoneNumber = supplierPhoneNumber;
                supplier.SupplierWebsite = supplierWebsite;
            }
        }

        public void Delete(string connectionString, string token, int supplierID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_supplier_id", supplierID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_supplier", inParameters);

            var supplier = this.Suppliers?.FirstOrDefault(s => s.SupplierID == supplierID);
            if (supplier != null)
            {
                this.Suppliers ??= new List<Supplier>();
                this.Suppliers.Remove(supplier);
            }
        }
    }
}
