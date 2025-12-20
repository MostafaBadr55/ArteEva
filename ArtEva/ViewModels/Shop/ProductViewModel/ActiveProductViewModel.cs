using ArtEva.DTOs.Pagination.Product;
using ArtEva.Models.Enums;

namespace ArtEva.ViewModels.Shop.ProductViewModel
{
    public class ActiveProductViewModel
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
        public List<ProductImageDto> Images { get; set; } = new();

    }
}
