namespace ArtEva.DTOs.Category
{
    public class CreateCategoryRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
