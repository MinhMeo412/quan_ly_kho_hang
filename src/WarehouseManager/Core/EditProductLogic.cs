using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core
{
    public static class EditProductLogic
    {
        private static Product GetProduct(int productID)
        {
            List<Product> products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            Product product = products.FirstOrDefault(p => p.ProductID == productID) ?? new Product(0, "", "", 0, 0);
            return product;
        }

        public static string GetProductName(int productID)
        {
            return GetProduct(productID).ProductName;
        }

        public static int GetProductPrice(int productID)
        {
            return GetProduct(productID).ProductPrice ?? 0;
        }

        public static int GetProductCategory(int productID)
        {
            int categoryID = GetProduct(productID).CategoryID ?? 0;

            List<Category> categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();

            Category category = categories.FirstOrDefault(c => c.CategoryID == categoryID) ?? new Category(0, "", "");
            string categoryName = category.CategoryName;

            List<string> categoryNames = GetCategoryList();
            return categoryNames.IndexOf(categoryName);
        }

        public static List<string> GetCategoryList()
        {
            List<Category> categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            List<string> categoryNames = new List<string>();

            foreach (Category category in categories)
            {
                categoryNames.Add(category.CategoryName);
            }

            return categoryNames;
        }

        public static string GetProductDescription(int productID)
        {
            return $"{GetProduct(productID).ProductDescription}";
        }

        public static DataTable GetProductVariants(int productID)
        {
            List<ProductVariant> productVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();
            List<ProductVariant> currentProductsVariants = productVariants.Where(pv => pv.ProductID == productID).ToList();

            return ConvertProductVariantsToDataTable(currentProductsVariants);
        }

        private static DataTable ConvertProductVariantsToDataTable(List<ProductVariant> productVariants)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Product Variant ID", typeof(int));
            dataTable.Columns.Add("Product ID", typeof(int));
            dataTable.Columns.Add("Image Url", typeof(string));
            dataTable.Columns.Add("Color", typeof(string));
            dataTable.Columns.Add("Size", typeof(string));


            foreach (var productVariant in productVariants)
            {
                dataTable.Rows.Add(
                    productVariant.ProductVariantID,
                    productVariant.ProductID,
                    productVariant.ProductVariantImageURL,
                    productVariant.ProductVariantColor,
                    productVariant.ProductVariantSize);
            }

            return dataTable;
        }

        private static List<ProductVariant> ConvertDataTableToProductVariants(DataTable dataTable)
        {
            List<ProductVariant> productVariants = new List<ProductVariant>();

            foreach (DataRow row in dataTable.Rows)
            {
                ProductVariant productVariant = new ProductVariant(
                    (int)row[0],
                    (int)row[1],
                    (string?)row[2],
                    (string?)row[3],
                    (string?)row[4]
                );
                productVariants.Add(productVariant);
            }

            return productVariants;
        }


    }
}