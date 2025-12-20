using ArtEva.DTOs.ProductImage;

namespace ArtEva.DTOs.Product
{
    public class UpdateProductDto
    {
        public int ShopId { get; set; }
        public int productId { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }

        public List<UpdateProductImage>? Images { get; set; }
    }
}
