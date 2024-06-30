using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core
{
    public static class AddCategoryLogic
    {
        public static void AddCategory(string categoryName, string categoryDescription)
        {
            List<Category>? categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            int highestCategoryID = categories.Max(c => c.CategoryID);

            Program.Warehouse.CategoryTable.Add(highestCategoryID + 1, categoryName, categoryDescription);
        }
    }
}