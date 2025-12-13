namespace ArtEva.DTOs.ProductImage
{
    public class CreateProductImageDto
    {
        public string Url { get; set; }
        public string AltText { get; set; }
        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
    }
}
