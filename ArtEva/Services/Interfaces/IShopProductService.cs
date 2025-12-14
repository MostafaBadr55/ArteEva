using ArtEva.DTOs.Shop;

namespace ArtEva.Services.Interfaces
{
    public interface IShopProductService
    {
         Task<CreatedShopDto> GetShopByOwnerIdAsync(int userId, int pageNumber, int pageSize);

    }
}
