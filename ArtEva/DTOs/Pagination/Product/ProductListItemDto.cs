using ArtEva.Models.Enums;

namespace ArtEva.DTOs.Pagination.Product
{
    public class ProductListItemDto
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public string Title { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
        public bool IsPublished { get; set; }
        public List<ProductImageDto> Images { get; set; } = new();
    }
}
