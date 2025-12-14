using ArteEva.Models;
using ArtEva.Models.Enums;
using System.Collections.ObjectModel;

namespace ArtEva.DTOs.Shop
{
    public class CreatedShopDto
    {
        public int Id { get; set; }
        public string OwnerUserName { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public ShopStatus Status { get; set; }
        public decimal RatingAverage { get; set; }

     }
}
