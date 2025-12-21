using ArtEva.DTOs.Product;
using ArtEva.Models.Enums;

namespace ArtEva.DTOs.Pagination.Product
{
    public class ProductListItemDto: ProductWithImagesDto
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public string SKU { get; set; }
        public bool IsPublished { get; set; }
    }
}
