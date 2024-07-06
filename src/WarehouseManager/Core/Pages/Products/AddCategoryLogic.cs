using WarehouseManager.Data.Entity;

namespace WarehouseManager.Core.Pages
{
    public static class AddCategoryLogic
    {
        public static void AddCategory(string categoryName, string categoryDescription)
        {
            // Workaround for a bug in terminal.gui that will crash the program if a row in a dropdown has a completely blank name.
            if (categoryName == "")
            {
                categoryName = " ";
            }

            List<Category>? categories = Program.Warehouse.CategoryTable.Categories ?? new List<Category>();
            int highestCategoryID = categories.Max(c => c.CategoryID);

            Program.Warehouse.CategoryTable.Add(highestCategoryID + 1, categoryName, categoryDescription);
        }
    }
}