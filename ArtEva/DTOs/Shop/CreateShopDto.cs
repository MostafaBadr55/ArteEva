using System.ComponentModel.DataAnnotations;

namespace ArtEva.DTOs.Shop
{
    public class CreateShopDto
    {
        [Required(ErrorMessage = "Shop name is required")]
        [MaxLength(120, ErrorMessage = "Shop name cannot exceed 120 characters")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}
