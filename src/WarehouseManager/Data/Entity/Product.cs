namespace WarehouseManager.Data.Entity
{
    class Product(int productID, string productName, string? productDescription, int? productPrice, int? categoryID)
    {
        public int ProductID { get; set; } = productID;
        public string ProductName { get; set; } = productName;
        public string? ProductDescription { get; set; } = productDescription;
        public int? ProductPrice { get; set; } = productPrice;
        public int? CategoryID { get; set; } = categoryID;
    }
}