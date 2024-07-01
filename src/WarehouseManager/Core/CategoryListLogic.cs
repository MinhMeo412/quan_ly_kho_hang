using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core
{
    public static class CategoryListLogic
    {
        // Lấy dữ liệu về và đổi kiểu dữ liệu sang dạng DataTable
        public static DataTable GetData()
        {
            List<Category> categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            return ConvertCategoryListToDataTable(categories);
        }

        // Đổi kiểu dữ liệu từ List<Category> sang DataTable
        private static DataTable ConvertCategoryListToDataTable(List<Category> categories)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("Category ID", typeof(int));
            dataTable.Columns.Add("Category Name", typeof(string));
            dataTable.Columns.Add("Category Description", typeof(string));

            categories ??= new List<Category>();
            foreach (Category category in categories)
            {
                dataTable.Rows.Add(category.CategoryID, category.CategoryName, category.CategoryDescription);
            }

            return dataTable;
        }

        // Đổi kiểu dữ liệu từ DataTable sang List<Category> (để thực hiện LINQ)
        private static List<Category> ConvertDataTableToCategoryList(DataTable dataTable)
        {
            List<Category> categories = new List<Category>();
            foreach (DataRow row in dataTable.Rows)
            {
                Category category = new Category((int)row[0], (string)row[1], (string?)row[2]);
                categories.Add(category);
            }
            return categories;
        }

        /// <summary>
        /// Sort theo từ mà người dùng nhập vào ô tìm kiếm
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="searchTerm"></param>
        /// <returns>DataTable</returns>
        public static DataTable SortCategoryBySearchTerm(DataTable dataTable, string searchTerm)
        {
            List<Category> categories = ConvertDataTableToCategoryList(dataTable);
            List<(Category, double)> categorySimilarities = new List<(Category, double)>();

            foreach (Category category in categories)
            {
                double maxSimilarity = Misc.MaxDouble(
                    Misc.JaccardSimilarity($"{category.CategoryID}", searchTerm),
                    Misc.JaccardSimilarity(category.CategoryName, searchTerm),
                    Misc.JaccardSimilarity(category.CategoryDescription ?? "", searchTerm)
                );

                categorySimilarities.Add((category, maxSimilarity));
            }

            List<Category> sortedCategories = categorySimilarities
                .OrderByDescending(cs => cs.Item2)
                .Select(cs => cs.Item1)
                .ToList();

            return ConvertCategoryListToDataTable(sortedCategories);
        }

        /// <summary>
        /// sort theo cột mà người dùng bấm vào
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="columnToSortBy"></param>
        /// <param name="sortColumnInDescendingOrder"></param>
        /// <returns>DataTable</returns>
        public static DataTable SortCategoryByColumn(DataTable dataTable, int columnToSortBy, bool sortColumnInDescendingOrder)
        {
            List<Category> categories = ConvertDataTableToCategoryList(dataTable);

            switch (columnToSortBy)
            {
                case 0:
                    categories = categories.OrderBy(c => c.CategoryID).ToList();
                    break;
                case 1:
                    categories = categories.OrderBy(c => c.CategoryName).ToList();
                    break;
                case 2:
                    categories = categories.OrderBy(c => c.CategoryDescription).ToList();
                    break;
                default:
                    break;
            }

            if (sortColumnInDescendingOrder)
            {
                categories.Reverse();
            }

            DataTable sortedDataTable = ConvertCategoryListToDataTable(categories);

            sortedDataTable.Columns[columnToSortBy].ColumnName = Misc.ShowCurrentSortingDirection(sortedDataTable.Columns[columnToSortBy].ColumnName, sortColumnInDescendingOrder);

            return sortedDataTable;
        }

        public static void UpdateCategory(int categoryID, string categoryName, string? categoryDescription)
        {

            // Workaround for a bug in terminal.gui that will crash the program if a row in a dropdown has a completely blank name.
            if (categoryName == "")
            {
                categoryName = " ";
            }
            Program.Warehouse.CategoryTable.Update(categoryID, categoryName, categoryDescription);
        }

        public static DataTable DeleteCategory(DataTable dataTable, int categoryID)
        {
            Program.Warehouse.CategoryTable.Delete(categoryID);
            List<Category> categories = ConvertDataTableToCategoryList(dataTable);
            categories.RemoveAll(c => c.CategoryID == categoryID);

            return ConvertCategoryListToDataTable(categories);
        }

    }
}