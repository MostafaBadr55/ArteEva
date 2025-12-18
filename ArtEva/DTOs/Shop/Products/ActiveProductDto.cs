using ArtEva.DTOs.Pagination.Product;
using ArtEva.Extensions;
using ArtEva.Models.Enums;

namespace ArtEva.DTOs.Shop.Products
{
    public class ActiveProductDto: IProductWithImagesDto
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
        public List<ProductImageDto> Images { get; set; } = new();
    }
}
