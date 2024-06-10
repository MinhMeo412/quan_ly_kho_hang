namespace WarehouseManager.Data.Entity
{
    class ProductVariant(int productVariantID, int productID, string? productVariantImageURL, string? productVariantColor, string? productVariantSize)
    {
        public int ProductVariantID { get; set; } = productVariantID;
        public int ProductID { get; set; } = productID;
        public string? ProductVariantImageURL { get; set; } = productVariantImageURL;
        public string? ProductVariantColor { get; set; } = productVariantColor;
        public string? ProductVariantSize { get; set; } = productVariantSize;
    }
}