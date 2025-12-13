namespace ArtEva.DTOs.ProductImage
{
    public class UpdateProductImage
    {
        public int Id { get; set; } = 0;
        public string Url { get; set; }
        public string AltText { get; set; }
        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
    }
}
