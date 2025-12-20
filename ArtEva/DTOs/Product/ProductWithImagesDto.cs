using ArtEva.DTOs.Pagination.Product;
using ArtEva.Models.Enums;

namespace ArtEva.DTOs.Product
{
    public class ProductWithImagesDto
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
        public List<ProductImageDto> Images { get; set; } = new();
    }
}