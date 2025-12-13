namespace ArtEva.DTOs.Product
{
    public class PagedProductsDto
    {
        public List<CreatedProductDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
