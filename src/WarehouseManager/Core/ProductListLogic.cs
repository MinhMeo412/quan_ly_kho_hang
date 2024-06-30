using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core
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
    }
}