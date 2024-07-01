using System.Data;
using WarehouseManager.Data.Entity;

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

        public static DataTable GetProductVariantData(int productID)
        {
            List<ProductVariant> productVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();
            List<ProductVariant> currentProductsVariants = productVariants.Where(pv => pv.ProductID == productID).ToList();

            return ConvertProductVariantDataToDataTable(currentProductsVariants);
        }

        private static DataTable ConvertProductVariantDataToDataTable(List<ProductVariant> productVariants)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Variant ID", typeof(object));
            dataTable.Columns.Add("Image Url", typeof(string));
            dataTable.Columns.Add("Color", typeof(string));
            dataTable.Columns.Add("Size", typeof(string));


            foreach (var productVariant in productVariants)
            {
                dataTable.Rows.Add(
                    productVariant.ProductVariantID,
                    productVariant.ProductVariantImageURL,
                    productVariant.ProductVariantColor,
                    productVariant.ProductVariantSize);
            }

            return dataTable;
        }

        public static DataTable DeleteProductVariant(DataTable currentDataTable, int row)
        {

            DataTable dataTable = currentDataTable.Copy();

            if (row < 0)
            {
                return dataTable;
            }

            DataRow rowToDelete = dataTable.Rows[row];

            if (rowToDelete != null)
            {
                // Mark the row for deletion
                rowToDelete.Delete();

                // Commit the deletion
                dataTable.AcceptChanges();
            }

            return dataTable;
        }

        public static DataTable AddProductVariant(DataTable currentDataTable, string imageURL, string color, string size)
        {
            DataTable dataTable = currentDataTable.Copy();
            dataTable.Rows.Add("Unsaved Variant", imageURL, color, size);
            return dataTable;
        }

        public static void Save(int productID, string productName, string productDescription, int productPrice, string categoryName, DataTable variantDataTable)
        {
            SaveProduct(productID, productName, productDescription, productPrice, categoryName);
            SaveVariants(productID, variantDataTable);
        }

        private static void SaveProduct(int productID, string productName, string productDescription, int productPrice, string categoryName)
        {
            List<Category>? categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            Category category = categories.FirstOrDefault(c => c.CategoryName == categoryName) ?? new Category(0, "", "");

            int categoryID = category.CategoryID;

            Program.Warehouse.ProductTable.Update(productID, productName, productDescription, productPrice, categoryID);
        }

        private static void SaveVariants(int productID, DataTable variantDataTable)
        {
            List<ProductVariant> allProductVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();
            List<ProductVariant> currentProductsVariants = allProductVariants.Where(pv => pv.ProductID == productID).ToList();
            List<int> undeletedVariantIDs = new List<int>();

            // Add new variants
            foreach (DataRow row in variantDataTable.Rows)
            {
                bool newVariant = row[0] == (object)"Unsaved Variant";
                bool modifiedVariant = !newVariant;

                if (newVariant)
                {
                    AddVariant(productID, (string?)row[1], (string?)row[2], (string?)row[3]);
                }

                if (modifiedVariant)
                {
                    UpdateVariant(
                        (int)row[0],
                        productID,
                        (string?)row[1],
                        (string?)row[2],
                        (string?)row[3]
                    );
                    undeletedVariantIDs.Add((int)row[0]);
                }
            }

            foreach (ProductVariant currentProductsVariant in currentProductsVariants)
            {
                bool deletedVariant = !undeletedVariantIDs.Contains(currentProductsVariant.ProductVariantID);

                if (deletedVariant)
                {
                    DeleteVariant(currentProductsVariant.ProductVariantID);
                }
            }
        }

        internal static int GetCurrentHighestProductVariantID()
        {
            List<ProductVariant> allProductVariants = Program.Warehouse.ProductVariantTable.ProductVariants ?? new List<ProductVariant>();
            int highestProductVariantID = allProductVariants.Max(v => v.ProductVariantID);
            return highestProductVariantID;
        }

        private static void AddVariant(int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
        {

            int productVariantID = GetCurrentHighestProductVariantID() + 1;

            Program.Warehouse.ProductVariantTable.Add(productVariantID, productID, productVariantImageURL, productVariantColor, productVariantSize);
        }

        private static void UpdateVariant(int productVariantID, int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
        {
            Program.Warehouse.ProductVariantTable.Update(productVariantID, productID, productVariantImageURL, productVariantColor, productVariantSize);
        }

        private static void DeleteVariant(int productVariantID)
        {
            Program.Warehouse.ProductVariantTable.Delete(productVariantID);
        }
    }
}