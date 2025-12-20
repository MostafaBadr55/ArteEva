using ArtEva.DTOs.Pagination.Product;
using ArtEva.DTOs.Product;
using ArtEva.Extensions;
using ArtEva.Models.Enums;

namespace ArtEva.DTOs.Shop.Products
{
    public class ActiveProductDto: ProductWithImagesDto
    {
        public ProductStatus Status { get; set; }= ProductStatus.Active;
    }
}
