namespace WarehouseManager.Data.Entity
{
    class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string? ProductDescription { get; set; }
        public int? ProductPrice { get; set; }
        public int? CategoryID { get; set; }
    }
}