using ArtEva.Models.Enums;

namespace ArtEva.Application.Shops.Quiries
{
    public class ShopQueryCriteria
    {
        public int? OwnerUserId { get; set; }
        public ShopStatus? Status { get; set; }
        public decimal? MinRating { get; set; }
        public decimal? MaxRating { get; set; }
        public string? Name { get; set; }
    }
}
