namespace WarehouseManager.Data.Entities
{
    class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string? ProductDescription { get; set; }
        public int? ProductPrice { get; set; }
        public int? ProductCategoryID { get; set; }
    }
}