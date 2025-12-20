using ArteEva.Models;
using ArtEva.DTOs.Shop.Products;
using ArtEva.Models.Enums;
using ArtEva.ViewModels.Shop.ProductViewModel;

namespace ArtEva.ViewModels.Shop
{
    public class GetProductShopViewModel
    {
        public int Id { get; set; }
        public string OwnerUserName { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; } 
        public string Description { get; set; }
        public ShopStatus Status { get; set; }
        public decimal RatingAverage { get; set; }

        public ICollection<ActiveProductViewModel> ActiveProductViewModels { get; set; }
        public ICollection<InActiveProductViewModel> inActiveProductViewModels { get; set; }


    }
}
