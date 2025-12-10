namespace ArtEva.DTOs.Category
{
    public class UpdateCategoryRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}