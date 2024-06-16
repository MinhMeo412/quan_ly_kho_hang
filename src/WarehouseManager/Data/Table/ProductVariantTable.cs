using WarehouseManager.Data.Entity;
using WarehouseManager.Data.Utility;

namespace WarehouseManager.Data.Table
{
    class ProductVariantTable
    {
        public List<ProductVariant>? ProductVariants { get; private set; }

        public void Load(string connectionString, string token)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token}
            };

            List<List<object?>> rawProductVariants = Procedure.ExecuteReader(connectionString, "read_product_variant", inParameters);

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

        public void Add(string connectionString, string token, int productVariantID, int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_product_variant_id", productVariantID},
                {"input_product_id", productID},
                {"input_product_variant_image_url", productVariantImageURL},
                {"input_product_variant_color", productVariantColor},
                {"input_product_variant_size", productVariantSize}
            };
            Procedure.ExecuteNonQuery(connectionString, "create_product_variant", inParameters);

            ProductVariant productVariant = new ProductVariant(productVariantID, productID, productVariantImageURL, productVariantColor, productVariantSize);

            this.ProductVariants ??= new List<ProductVariant>();
            this.ProductVariants.Add(productVariant);
        }

        public void Update(string connectionString, string token, int productVariantID, int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_product_variant_id", productVariantID},
                {"input_product_id", productID},
                {"input_product_variant_image_url", productVariantImageURL},
                {"input_product_variant_color", productVariantColor},
                {"input_product_variant_size", productVariantSize}
            };

            Procedure.ExecuteNonQuery(connectionString, "update_product_variant", inParameters);

            var productVariant = this.ProductVariants?.FirstOrDefault(pv => pv.ProductVariantID == productVariantID);
            if (productVariant != null)
            {
                productVariant.ProductID = productID;
                productVariant.ProductVariantImageURL = productVariantImageURL;
                productVariant.ProductVariantColor = productVariantColor;
                productVariant.ProductVariantSize = productVariantSize;
            }
        }

        public void Delete(string connectionString, string token, int productVariantID)
        {
            Dictionary<string, object?> inParameters = new Dictionary<string, object?>{
                {"input_token", token},
                {"input_product_variant_id", productVariantID}
            };

            Procedure.ExecuteNonQuery(connectionString, "delete_product_variant", inParameters);

            var productVariant = this.ProductVariants?.FirstOrDefault(pv => pv.ProductVariantID == productVariantID);
            if (productVariant != null)
            {
                this.ProductVariants ??= new List<ProductVariant>();
                this.ProductVariants.Remove(productVariant);
            }
        }
    }
}
