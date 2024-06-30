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

        public static DataTable SortProductBySearchTerm(DataTable dataTable, string searchTerm)
        {
            List<(int, string, string?, int?, string?)> products = ConvertDataTableToProductMenuRows(dataTable);
            List<((int, string, string?, int?, string?), double)> productSimilarities = new List<((int, string, string?, int?, string?), double)>();

            foreach ((int, string, string?, int?, string?) product in products)
            {
                double maxSimilarity = Misc.MaxDouble(
                    Misc.JaccardSimilarity($"{product.Item1}", searchTerm),
                    Misc.JaccardSimilarity(product.Item2, searchTerm),
                    Misc.JaccardSimilarity($"{product.Item3}", searchTerm),
                    Misc.JaccardSimilarity($"{product.Item4}", searchTerm),
                    Misc.JaccardSimilarity($"{product.Item5}", searchTerm)
                );

                productSimilarities.Add((product, maxSimilarity));
            }

            List<(int, string, string?, int?, string?)> sortedProducts = productSimilarities
                .OrderByDescending(p => p.Item2)
                .Select(p => p.Item1)
                .ToList();

            return ConvertProductMenuRowsToDataTable(sortedProducts);
        }

        public static DataTable SortProductByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            List<(int, string, string?, int?, string?)> products = ConvertDataTableToProductMenuRows(dataTable);

            switch (columnToSortBy)
            {
                case 0:
                    products = products.OrderBy(p => p.Item1).ToList();
                    break;
                case 1:
                    products = products.OrderBy(p => p.Item2).ToList();
                    break;
                case 2:
                    products = products.OrderBy(p => p.Item3).ToList();
                    break;
                case 3:
                    products = products.OrderBy(p => p.Item4).ToList();
                    break;
                case 4:
                    products = products.OrderBy(p => p.Item5).ToList();
                    break;
                default:
                    break;
            }

            if (sortColumnInDescendingOrder)
            {
                products.Reverse();
            }

            DataTable sortedDataTable = ConvertProductMenuRowsToDataTable(products);

            sortedDataTable.Columns[columnToSortBy].ColumnName = Misc.ShowCurrentSortingDirection(sortedDataTable.Columns[columnToSortBy].ColumnName, sortColumnInDescendingOrder);

            return sortedDataTable;
        }

        public static DataTable DeleteProduct(DataTable dataTable, int productID)
        {
            Program.Warehouse.ProductTable.Delete(productID);

            List<(int, string, string?, int?, string?)> products = ConvertDataTableToProductMenuRows(dataTable);

            products.RemoveAll(p => p.Item1 == productID);

            return ConvertProductMenuRowsToDataTable(products);
        }

    }
}