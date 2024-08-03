using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core.Pages
{
    public static class ProductListLogic
    {
        public static DataTable GetData()
        {
            List<Product>? products = Program.Warehouse.ProductTable.Products ?? new List<Product>();
            List<Category>? categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();

            List<(int, string, string?, int?, string?)> productMenuRows = new List<(int, string, string?, int?, string?)>();

            foreach (Product product in products)
            {
                Category category = categories.FirstOrDefault(c => c.CategoryID == product.CategoryID) ?? new Category(0, "", "");
                string categoryName = category.CategoryName;

                productMenuRows.Add((
                    product.ProductID,
                    product.ProductName,
                    product.ProductDescription,
                    product.ProductPrice,
                    categoryName
                ));
            }

            return ConvertProductMenuRowsToDataTable(productMenuRows);
        }

        private static DataTable ConvertProductMenuRowsToDataTable(List<(int, string, string?, int?, string?)> productMenuRows)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Product ID", typeof(int));
            dataTable.Columns.Add("Product Name", typeof(string));
            dataTable.Columns.Add("Product Description", typeof(string));
            dataTable.Columns.Add("Product Price", typeof(int));
            dataTable.Columns.Add("Category", typeof(string));

            foreach (var productMenuRow in productMenuRows)
            {
                dataTable.Rows.Add(productMenuRow.Item1, productMenuRow.Item2, productMenuRow.Item3, productMenuRow.Item4, productMenuRow.Item5);
            }

            return dataTable;
        }

        private static List<(int, string, string?, int?, string?)> ConvertDataTableToProductMenuRows(DataTable dataTable)
        {
            List<(int, string, string?, int?, string?)> productMenuRows = new List<(int, string, string?, int?, string?)>();

            foreach (DataRow row in dataTable.Rows)
            {
                productMenuRows.Add(((int)row[0], (string)row[1], (string?)row[2], (int?)row[3], (string?)row[4]));
            }

            return productMenuRows;
        }

        public static DataTable SortProductBySearchTerm(DataTable dataTable, string searchTerm)
        {
            return SortDataTable.BySearchTerm(dataTable, searchTerm);
        }

        public static DataTable SortProductByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            return SortDataTable.ByColumn(dataTable, columnToSortBy, sortColumnInDescendingOrder);
        }

        public static DataTable DeleteProduct(DataTable dataTable, int productID)
        {
            Program.Warehouse.ProductTable.Delete(productID);

            List<(int, string, string?, int?, string?)> products = ConvertDataTableToProductMenuRows(dataTable);

            products.RemoveAll(p => p.Item1 == productID);

            return ConvertProductMenuRowsToDataTable(products);
        }

        public static DataTable GetImportForm()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Product ID");
            dataTable.Columns.Add("Product Name");
            dataTable.Columns.Add("Price");
            dataTable.Columns.Add("Category");
            dataTable.Columns.Add("Description");
            dataTable.Columns.Add("Image URL");
            dataTable.Columns.Add("Color");
            dataTable.Columns.Add("Size");

            return dataTable;
        }

        public static DataTable GetFileInformation()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Product Import Form");
            dataTable.Rows.Add("");

            return dataTable;
        }

        public static void ImportFromExcel(string filePath)
        {
            List<Product> products = GetProducts();
            List<Category> categories = GetCategories();
            DataTable dataTable = Excel.ImportFromExcel(filePath);

            List<int> newProductIDs = new List<int>();
            List<int> createdNewProductIDs = new List<int>();

            foreach (DataRow row in dataTable.Rows)
            {
                int productID = int.Parse($"{row[0]}");
                if (!ProductAlreadyExists(productID, products))
                {
                    newProductIDs.Add(productID);
                }
            }

            foreach (DataRow row in dataTable.Rows)
            {
                int productID = int.Parse($"{row[0]}");
                string productName = $"{row[1]}";
                int productPrice = int.Parse($"{row[2]}");
                int categoryID = GetCategoryID($"{row[3]}", categories);
                string description = $"{row[4]}";
                string imageURL = $"{row[5]}";
                string color = $"{row[6]}";
                string size = $"{row[7]}";

                if (newProductIDs.Contains(productID) && !createdNewProductIDs.Contains(productID))
                {
                    AddProduct(productID, productName, productPrice, categoryID, description);
                    AddProductVariant(productID, imageURL, color, size);
                    createdNewProductIDs.Add(productID);
                }
                else if (newProductIDs.Contains(productID) && createdNewProductIDs.Contains(productID))
                {
                    AddProductVariant(productID, imageURL, color, size);
                }
            }
        }

        private static bool ProductAlreadyExists(int productID, List<Product> products)
        {
            List<int> productIDs = products.Select(p => p.ProductID).ToList();
            return productIDs.Contains(productID);
        }

        private static List<Product> GetProducts()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetProducts();
        }

        private static List<Category> GetCategories()
        {
            return WarehouseShipmentsReportWarehouseLogic.GetCategories();
        }

        private static int GetCategoryID(string categoryName, List<Category> categories)
        {
            Category category = categories.FirstOrDefault(c => c.CategoryName == categoryName) ?? new Category(0, "", null);
            return category.CategoryID;
        }

        private static void AddProduct(int productID, string productName, int productPrice, int categoryID, string description)
        {
            Program.Warehouse.ProductTable.Add(productID, productName, description, productPrice, categoryID);
        }

        private static void AddProductVariant(int productID, string imageURL, string color, string size)
        {
            AddProductLogic.AddVariant(
                productID: productID,
                productVariantImageURL: imageURL,
                productVariantColor: color,
                productVariantSize: size
            );
        }
    }
}