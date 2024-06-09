namespace WarehouseManager.Data.Entity
{
    class ProductVariant
    {
        public int ProductVariantID { get; set; }
        public int ProductID { get; set; }
        public string? ProductVariantImageURL { get; set; }
        public string? ProductVariantColor { get; set; }
        public string? ProductVariantSize { get; set; }
    }
}