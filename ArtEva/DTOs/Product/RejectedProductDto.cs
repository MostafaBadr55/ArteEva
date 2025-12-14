namespace ArtEva.DTOs.Product
{
    public class RejectedProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SKU { get; set; }
        public bool IsPublished { get; set; }
        public string RejectionMessage { get; set; }
    }
}
