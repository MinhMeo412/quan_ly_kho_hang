namespace WarehouseManager.Data.Entity
{
    class Category(int categoryID, string categoryName, string? categoryDescription)
    {
        public int CategoryID { get; set; } = categoryID;
        public string CategoryName { get; set; } = categoryName;
        public string? CategoryDescription { get; set; } = categoryDescription;
    }
}