using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class SupplierTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;
        public List<Supplier>? Suppliers { get; private set; }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawSuppliers = Procedure.ExecuteReader(this.ConnectionString, "read_supplier", inParameters);

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

        public void Add(int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_supplier_id", supplierID},
                {"input_supplier_name", supplierName},
                {"input_supplier_description", supplierDescription},
                {"input_supplier_address", supplierAddress},
                {"input_supplier_email", supplierEmail},
                {"input_supplier_phone_number", supplierPhoneNumber},
                {"input_supplier_website", supplierWebsite}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_supplier", inParameters);
        }

        public void Update(int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_supplier_id", supplierID},
                {"input_supplier_name", supplierName},
                {"input_supplier_description", supplierDescription},
                {"input_supplier_address", supplierAddress},
                {"input_supplier_email", supplierEmail},
                {"input_supplier_phone_number", supplierPhoneNumber},
                {"input_supplier_website", supplierWebsite}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_supplier", inParameters);
        }

        public void Delete(int supplierID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_supplier_id", supplierID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_supplier", inParameters);
        }
    }
}
