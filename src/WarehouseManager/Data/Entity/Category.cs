namespace WarehouseManager.Data.Entity
{
    class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";
        public string? CategoryDescription { get; set; }
    }
}