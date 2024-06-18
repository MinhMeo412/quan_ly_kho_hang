using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class ProductTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<Product>? _products;
        public List<Product>? Products
        {
            get
            {
                this.Load();
                return this._products;
            }
            private set
            {
                this._products = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawProducts = Procedure.ExecuteReader(this.ConnectionString, "read_product", inParameters);

            List<Product> products = new List<Product>();
            foreach (List<object?> rawProduct in rawProducts)
            {
                Product product = new Product(
                    (int)(rawProduct[0] ?? 0),
                    (string)(rawProduct[1] ?? ""),
                    (string?)rawProduct[2],
                    (int?)rawProduct[3],
                    (int?)rawProduct[4]);
                products.Add(product);
            }

            this.Products = products;
        }

        public void Add(int productID, string productName, string? productDescription, int? productPrice, int? categoryID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_product_id", productID},
                {"input_product_name", productName},
                {"input_product_description", productDescription},
                {"input_product_price", productPrice},
                {"input_category_id", categoryID}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_product", inParameters);
        }

        public void Update(int productID, string productName, string? productDescription, int? productPrice, int? categoryID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_product_id", productID},
                {"input_product_name", productName},
                {"input_product_description", productDescription},
                {"input_product_price", productPrice},
                {"input_category_id", categoryID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_product", inParameters);
        }

        public void Delete(int productID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_product_id", productID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_product", inParameters);
        }
    }
}
