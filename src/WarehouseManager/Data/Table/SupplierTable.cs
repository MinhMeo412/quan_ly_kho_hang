using WarehouseManager.Data.Entity;

namespace WarehouseManager.Data.Table
{
    class SupplierTable
    {
        public List<Supplier>? Suppliers { get; private set; }
        
        private void Load(string connectionString,string token)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token",token }
            };

            List<List<object>> rawSuppliers = Procedure.ExecuteReader(connectionString, "read_supplier", inParameters);

            List<Supplier> suppliers = new List<Supplier>();
            foreach(List<object> rawSupplier in rawSuppliers)
            {
                Supplier supplier = new Supplier((int)supplierID[0], (string)supplierName[1], (string)supplierDescription[2], (string)supplierAddress[3], (string)supplierEmail[4], (string)supplierPhoneNumber[5], (string)supplierWebsite[6]);
                suppliers.Add(supplier);
            }
            this.Add(permission);
        }

        public void Add(string connectionString, string token, int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"new_supplier_name", supplierName},
                {"new_supplier_description", supplierDescription},
                {"new_supplier_address", permissionDescription},
                {"new_supplier_email", supplierEmail},
                {"new_supplier_phone_number", supplierPhoneNumber},
                {"new_supplier_website", supplierWebsite}
            };

            Procedure.ExecuteNonQuery(connectionString, "create_supplier", inParameters);

            Supplier supplier = new Supplier(supplierName, supplierDescription, permissionDescription, supplierEmail, supplierPhoneNumber, supplierWebsite);


            this.Suppliers ??= new List<Supplier>();
            this.Suppliers.Add(supplier);
        }
        
        public void Update(string connectionString, string token, int supplierID, string supplierName, string? supplierDescription, string? supplierAddress, string? supplierEmail, string? supplierPhoneNumber, string? supplierWebsite)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"new_supplier_name", supplierName},
                {"new_supplier_description", supplierDescription},
                {"new_supplier_address", supplierAddress},
                {"new_supplier_email", supplierEmail},
                {"new_supplier_phone_number", supplierPhoneNumber},
                {"new_supplier_website", supplierWebsite}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_supplier", inParameters);

            var supplier=this.Suppliers?.FirstOrDefault(s => s.SupplierID == supplierID);

            if (supplier != null)
            {
                supplier.SupplierName = supplierName;
                supplier.SupplierDescription = supplierDescription;
                supplier.SupplierAddress = supplierName;
                supplier.SupplierEmail = supplierEmail;
                supplier.SupplierPhoneNumber = supplierPhoneNumber;
                supplier.SupplierWebsite = supplierWebsite;
            }
        }

        public void Delete(string connectionString, string token, int supplierID)
        {
            Dictionary<string, object>? inParameters = new Dictionary<string, object>
            {
                {"input_token", token},
                {"target_supplier_id", supplierID},
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