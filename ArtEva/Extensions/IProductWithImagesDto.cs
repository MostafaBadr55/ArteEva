using ArtEva.DTOs.Pagination.Product;

namespace ArtEva.Extensions
{
    public interface IProductWithImagesDto
    {
        List<ProductImageDto>? Images { get; set; }
    }
}