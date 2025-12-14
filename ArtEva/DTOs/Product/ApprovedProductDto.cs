using ArtEva.DTOs.ProductImage;

namespace ArtEva.DTOs.Product
{
    public class ApprovedProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SKU { get; set; }
        public bool IsPublished { get; set; }
        public string ApprovalMessage { get; set; }
    }
}
