namespace ArtEva.DTOs.ProductImage
{
    public class CreatedProductImageDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string AltText { get; set; }
        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
    }
}
