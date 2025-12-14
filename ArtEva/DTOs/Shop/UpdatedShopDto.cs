using ArtEva.Models.Enums;

namespace ArtEva.DTOs.Shop
{
    public class UpdatedShopDto
    {
      
        public string ShopName { get; set; }
        public string OwnerUserName { get; set; }   
        public ShopStatus shopStatus { get; set; }  
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal RatingAverage { get; set; }  


    }
}
