using ArtEva.DTOs.ProductImage;

namespace ArtEva.DTOs.Product
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Title { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }

        public List<CreatedProductImageDto> Images { get; set; }
    }
}
