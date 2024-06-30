using System.Data;
using WarehouseManager.Data.Entity;
using WarehouseManager.Core.Utility;

namespace WarehouseManager.Core
{
    public static class CategoryListLogic
    {
        public static DataTable GetSortedCategoryList(int columnToSortBy = -1, bool sortColumnInDescendingOrder = false)
        {
            List<Category>? categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            List<Category> sortedCategories = new List<Category>();


            switch (columnToSortBy)
            {
                case 0:
                    sortedCategories = categories.OrderBy(c => c.CategoryID).ToList();
                    break;
                case 1:
                    sortedCategories = categories.OrderBy(c => c.CategoryName).ToList();
                    break;
                case 2:
                    sortedCategories = categories.OrderBy(c => c.CategoryDescription).ToList();
                    break;
                default:
                    sortedCategories = categories;
                    break;
            }

            if (sortColumnInDescendingOrder)
            {
                sortedCategories.Reverse();
            }

            return ConvertCategoryListToDataTable(sortedCategories);
        }

        public static DataTable GetSearchedCategory(string searchTerm)
        {
            List<Category> categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            List<(Category, double)> categorySimilarities = new List<(Category, double)>();

            foreach (var category in categories)
            {
                double maxSimilarity = Misc.MaxDouble(
                    Misc.JaccardSimilarity($"{category.CategoryID}", searchTerm),
                    Misc.JaccardSimilarity(category.CategoryName, searchTerm),
                    Misc.JaccardSimilarity(category.CategoryDescription ?? "", searchTerm)
                );

                categorySimilarities.Add((category, maxSimilarity));
            }

            var filteredCategories = categorySimilarities
                    .Where(cs => cs.Item2 >= 0.5)
                    .OrderByDescending(cs => cs.Item2)
                    .ToList();

            if (filteredCategories.Count < 10)
            {
                var additionalCategories = categorySimilarities
                    .OrderByDescending(cs => cs.Item2)
                    .Take(10)
                    .ToList();

                filteredCategories = additionalCategories;
            }

            var sortedCategories = filteredCategories
                .OrderByDescending(cs => cs.Item2)
                .Select(cs => cs.Item1)
                .ToList();

            return ConvertCategoryListToDataTable(sortedCategories);
        }

        private static DataTable ConvertCategoryListToDataTable(List<Category>? categories)
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

        public static string ShowCurrentSortingDirection(string currentText, int direction)
        {
            string newText = currentText;

            string upwardsArrow = "\u25B2";
            string downwardsArrow = "\u25BC";

            switch (direction)
            {
                case 0:
                    // not sorting by anything
                    if (currentText.Contains(upwardsArrow) || currentText.Contains(downwardsArrow))
                    {
                        newText = newText.Replace(upwardsArrow, "");
                        newText = newText.Replace(downwardsArrow, "");
                        newText = newText.Trim();
                    }
                    break;
                case 1:
                    // sort in ascending order
                    if (currentText.Contains(downwardsArrow))
                    {
                        newText = newText.Replace(downwardsArrow, upwardsArrow);
                    }

                    if (!currentText.Contains(upwardsArrow))
                    {
                        newText = $"{newText} {upwardsArrow}";
                    }
                    break;
                case 2:
                    // sort in descending order
                    if (currentText.Contains(upwardsArrow))
                    {
                        newText = newText.Replace(upwardsArrow, downwardsArrow);
                    }

                    if (!currentText.Contains(downwardsArrow))
                    {
                        newText = $"{newText} {downwardsArrow}";
                    }

                    break;
                default:
                    break;
            }

            return newText;
        }

        public static void UpdateCategory(int categoryID, string categoryName, string? categoryDescription)
        {
            Program.Warehouse.CategoryTable.Update(categoryID, categoryName, categoryDescription);
        }

        public static void DeleteCategory(int categoryID)
        {
            Program.Warehouse.CategoryTable.Delete(categoryID);
        }

    }
}