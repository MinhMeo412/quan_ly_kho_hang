using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class ProductListReportLogic
    {
        public static DataTable GetFileInformation()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Product List Report", typeof(string));

            dataTable.Rows.Add($"Products: {GetProductCount()}");
            dataTable.Rows.Add($"Variants: {GetProductVariantCount()}");
            dataTable.Rows.Add($"Date: {GetCurrentTime()}");

            return dataTable;
        }

        public static DataTable GetProductAndVariantList()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Product ID", typeof(string));
            dataTable.Columns.Add("Product Name", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Category", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Image URL", typeof(string));
            dataTable.Columns.Add("Color", typeof(string));
            dataTable.Columns.Add("Size", typeof(string));

            List<Product> products = GetProducts();
            List<ProductVariant> productVariants = GetProductVariants();
            List<Category> categories = GetCategories();
            foreach (Product product in products)
            {
                foreach (ProductVariant variant in GetProductVariantsByID(product.ProductID, productVariants))
                {

                    dataTable.Rows.Add(
                        product.ProductID,
                        product.ProductName,
                        product.ProductPrice,
                        GetCategoryName(product.CategoryID ?? 0, categories),
                        product.ProductDescription,
                        variant.ProductVariantImageURL,
                        variant.ProductVariantColor,
                        variant.ProductVariantSize
                    );
                }
            }

            return dataTable;
        }

        private static List<Product> GetProducts()
        {
            List<Product> products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            return products;
        }

        private static List<Category> GetCategories()
        {
            List<Category> categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            return categories;
        }

        private static List<ProductVariant> GetProductVariants()
        {
            List<ProductVariant> productVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();
            return productVariants;
        }

        private static int GetProductCount()
        {
            List<Product> products = GetProducts();
            return products.Count;
        }

        private static int GetProductVariantCount()
        {
            List<ProductVariant> productVariants = GetProductVariants();
            return productVariants.Count;
        }

        private static string GetCurrentTime()
        {
            return WarehouseStockReportLogic.GetCurrentTime();
        }

        private static string GetCategoryName(int categoryID, List<Category> categories)
        {
            Category category = categories.FirstOrDefault(c => c.CategoryID == categoryID) ?? new Category(1, "", "");
            string categoryName = category.CategoryName;
            return categoryName;
        }

        private static List<ProductVariant> GetProductVariantsByID(int productID, List<ProductVariant> productVariants)
        {
            return productVariants.Where(v => v.ProductID == productID).ToList();
        }
    }
}