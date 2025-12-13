namespace ArtEva.DTOs.Pagination.Product
{
    public class ProductDetailedDto: ProductListItemDto
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
