using ArtEva.DTOs.Shop;
using ArtEva.ViewModels.Shop;
using ArtEva.ViewModels.Shop.ProductViewModel;

namespace ArtEva.DTOs.Shop.Mappings
{
    public static class ShopMappingExtensions
    {
        public static GetProductShopViewModel ToViewModel(
            this CreatedShopDto dto)
        {
            return new GetProductShopViewModel
            {
                Id = dto.Id,
                OwnerUserName = dto.OwnerUserName,
                Name = dto.Name,
                ImageUrl = dto.ImageUrl,
                Description = dto.Description,
                Status = dto.Status,
                RatingAverage = dto.RatingAverage,

                ActiveProductViewModels = dto.activeProductDtos
                    .Select(p => new ActiveProductViewModel
                    {
                        Title = p.Title,
                        Images = p.Images,
                        Price = p.Price,
                        Status = p.Status
                    })
                    .ToList(),

                inActiveProductViewModels = dto.inActiveProductDtos
                    .Select(p => new InActiveProductViewModel
                    {
                        Title = p.Title,
                        Images = p.Images,
                        Price = p.Price,
                        Status = p.Status
                    })
                    .ToList()
            };
        }
    }
}
