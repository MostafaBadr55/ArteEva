using ArtEva.DTOs.ProductImage;

namespace ArtEva.DTOs.Product
{
    public class CreateProductDto
    {
        public int ShopId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        public string Title { get; set; }
        public decimal Price { get; set; }

        public List<CreateProductImageDto> Images { get; set; }
    }
}
