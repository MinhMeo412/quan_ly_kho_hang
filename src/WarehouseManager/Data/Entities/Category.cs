namespace WarehouseManager.Data.Entities
{
    class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";
        public string? CategoryDescription { get; set; }
    }
}