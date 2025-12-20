namespace ArtEva.DTOs.Product
{
    public class UpdatedProductPriceDto
    {
        public bool IsChanged { get; set; } = false;
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
