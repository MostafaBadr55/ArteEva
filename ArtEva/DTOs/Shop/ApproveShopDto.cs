using ArtEva.Models.Enums;

namespace ArtEva.DTOs.Shop
{
    public class ApproveShopDto
    {
        public int OwnerUserId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public ShopStatus Status { get; set; }
        public decimal RatingAverage { get; set; }

    }
}
