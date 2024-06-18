using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class ProductVariantTable(string connectionString, string? token)
    {
        private string ConnectionString = connectionString;
        private string? Token = token;

        private List<ProductVariant>? _productVariants;
        public List<ProductVariant>? ProductVariants
        {
            get
            {
                this.Load();
                return this._productVariants;
            }
            private set
            {
                this._productVariants = value;
            }
        }

        private void Load()
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token}
            };

            List<List<object?>> rawProductVariants = Procedure.ExecuteReader(this.ConnectionString, "read_product_variant", inParameters);

            List<ProductVariant> productVariants = new List<ProductVariant>();
            foreach (List<object?> rawProductVariant in rawProductVariants)
            {
                ProductVariant productVariant = new ProductVariant(
                    (int)(rawProductVariant[0] ?? 0),
                    (int)(rawProductVariant[1] ?? 0),
                    (string?)rawProductVariant[2],
                    (string?)rawProductVariant[3],
                    (string?)rawProductVariant[4]
                );
                productVariants.Add(productVariant);
            }

            this.ProductVariants = productVariants;
        }

        public void Add(int productVariantID, int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_product_variant_id", productVariantID},
                {"input_product_id", productID},
                {"input_product_variant_image_url", productVariantImageURL},
                {"input_product_variant_color", productVariantColor},
                {"input_product_variant_size", productVariantSize}
            };
            Procedure.ExecuteNonQuery(this.ConnectionString, "create_product_variant", inParameters);
        }

        public void Update(int productVariantID, int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_product_variant_id", productVariantID},
                {"input_product_id", productID},
                {"input_product_variant_image_url", productVariantImageURL},
                {"input_product_variant_color", productVariantColor},
                {"input_product_variant_size", productVariantSize}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "update_product_variant", inParameters);
        }

        public void Delete(int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", this.Token},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(this.ConnectionString, "delete_product_variant", inParameters);
        }
    }
}
