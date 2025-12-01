using ArteEva.Models;

namespace ArtEva.DTOs.Shop
{
    public class ShopDto
    {
        public int Id { get; set; }
        public int OwnerUserId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public ShopStatus Status { get; set; }
        public string RejectionMessage { get; set; }
        public decimal RatingAverage { get; set; }
    }
}
