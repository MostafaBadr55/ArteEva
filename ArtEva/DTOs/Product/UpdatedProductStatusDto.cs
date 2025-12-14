using ArtEva.Models.Enums;

namespace ArtEva.DTOs.Product
{
    public class UpdatedProductStatusDto
    {
        public bool IsUpdated { get; set; } = false;
        public string ProductName { get; set; }
        public ProductStatus UpdatedStatus { get; set; }
    }
}
