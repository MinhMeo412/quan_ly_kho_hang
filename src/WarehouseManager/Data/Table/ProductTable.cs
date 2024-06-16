using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class ProductTable
    {
        public List<Product>? Products { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawProducts = Procedure.ExecuteReader(connectionString, "read_product", inParameters);

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

        public void Add(string connectionString, string token, int productID, string productName, string? productDescription, int? productPrice, int? categoryID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_product_id", productID},
                {"input_product_name", productName},
                {"input_product_description", productDescription},
                {"input_product_price", productPrice},
                {"input_category_id", categoryID}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_product", inParameters);

            Product product = new Product(productID, productName, productDescription, productPrice, categoryID);

            this.Products ??= new List<Product>();
            this.Products.Add(product);
        }

        public void Update(string connectionString, string token, int productID, string productName, string? productDescription, int? productPrice, int? categoryID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_product_id", productID},
                {"input_product_name", productName},
                {"input_product_description", productDescription},
                {"input_product_price", productPrice},
                {"input_category_id", categoryID}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_product", inParameters);

            var product = this.Products?.FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                product.ProductName = productName;
                product.ProductDescription = productDescription;
                product.ProductPrice = productPrice;
                product.CategoryID = categoryID;
            }
        }

        public void Delete(string connectionString, string token, int productID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_product_id", productID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_product", inParameters);

            var product = this.Products?.FirstOrDefault(p => p.ProductID == productID);
            if (product != null)
            {
                this.Products ??= new List<Product>();
                this.Products.Remove(product);
            }
        }
    }
}
