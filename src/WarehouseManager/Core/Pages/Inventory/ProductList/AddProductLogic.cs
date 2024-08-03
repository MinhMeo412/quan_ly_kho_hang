using System.Data;
using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddProductLogic
    {
        public static List<string> GetCategoryList()
        {
            return EditProductLogic.GetCategoryList();
        }

        public static DataTable GetDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Image Url", typeof(string));
            dataTable.Columns.Add("Color", typeof(string));
            dataTable.Columns.Add("Size", typeof(string));

            return dataTable;
        }

        public static DataTable AddVariantToDataTable(DataTable currentDataTable, string imageURL, string color, string size)
        {
            DataTable dataTable = currentDataTable.Copy();
            dataTable.Rows.Add(imageURL, color, size);
            return dataTable;
        }

        public static DataTable DeleteVariantFromDataTable(DataTable currentDataTable, int row)
        {
            return EditProductLogic.DeleteProductVariant(currentDataTable, row);
        }

        public static void Save(string productName, string productDescription, int productPrice, string categoryName, DataTable variantDataTable)
        {
            int productID = GetCurrentHighestProductID() + 1;
            AddProduct(productID, productName, productDescription, productPrice, categoryName);
            AddVariants(productID, variantDataTable);
        }

        private static int GetCurrentHighestProductID()
        {
            List<Product> products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            int highestProductID = products.Max(p => p.ProductID);
            return highestProductID;
        }

        private static void AddProduct(int productID, string productName, string productDescription, int productPrice, string categoryName)
        {
            List<Category>? categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            Category? category = categories.FirstOrDefault(c => c.CategoryName == categoryName);

            int? categoryID = null;
            if (category != null)
            {
                categoryID = category.CategoryID;
            }

            Program.Warehouse.ProductTable.Add(productID, productName, productDescription, productPrice, categoryID);
        }

        private static void AddVariants(int productID, DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                AddVariant(productID, (string?)row[0], (string?)row[1], (string?)row[2]);
            }
        }

        private static int GetCurrentHighestProductVariantID()
        {
            return EditProductLogic.GetCurrentHighestProductVariantID();
        }

        internal static void AddVariant(int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
        {
            int productVariantID = GetCurrentHighestProductVariantID() + 1;
            Program.Warehouse.ProductVariantTable.Add(productVariantID, productID, productVariantImageURL, productVariantColor, productVariantSize);
        }
    }
}