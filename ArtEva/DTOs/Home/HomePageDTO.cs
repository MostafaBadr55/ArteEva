using ArtEva.DTOs.Category;
using ArtEva.DTOs.Pagination.Product;
using ArtEva.DTOs.Product;

namespace ArtEva.DTOs.Home
{
    public class HomePageDTO
    {
        public string WelcomeMessage { get; set; }
        public IEnumerable<ProductListItemDto> FeaturedProducts { get; set; }
        public List<CategoryDto> Categories { get; set; }
     }
}
