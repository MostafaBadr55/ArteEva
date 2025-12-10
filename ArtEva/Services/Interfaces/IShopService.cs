using ArtEva.DTOs.Shop;
using System.Threading.Tasks;

namespace ArtEva.Services
{
    public interface IShopService
    {
        Task<ShopDto> CreateShopAsync(int userId, CreateShopDto dto);
        Task<ShopDto> GetShopByOwnerIdAsync(int userId);
        Task<ShopDto> GetShopByIdAsync(int shopId);
        Task<IEnumerable<ShopDto>> GetPendingShopsAsync();
        Task<ShopDto> ApproveShopAsync(int shopId);
        Task<ShopDto> RejectShopAsync(int shopId, RejectShopDto dto);
        Task<bool> ShopExistAsync(int shopId);
    }
}
