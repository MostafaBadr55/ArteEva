using ArtEva.Models.Enums;

namespace ArtEva.DTOs.Shop
{
    public class RejectedShopDto
    {
        public string Name { get; set; }
        public string RejectionMessage { get; set; }
        public string OwnerUserName { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public ShopStatus Status { get; set; }
        public decimal RatingAverage { get; set; }
    }
}
