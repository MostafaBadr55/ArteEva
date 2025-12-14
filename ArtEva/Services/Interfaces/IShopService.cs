using ArtEva.DTOs.Shop;
using System.Threading.Tasks;

namespace ArtEva.Services
{
    public interface IShopService
    {
        Task CreateShopAsync(int userId, CreateShopDto dto);
        Task<ExistShopDto> GetShopByIdAsync(int shopId);
        Task<IEnumerable<PendingShopDto>> GetPendingShopsAsync();
        Task<ApproveShopDto> ApproveShopAsync(int shopId);
        Task<RejectedShopDto> RejectShopAsync(int shopId, RejectShopDto dto);
        Task<bool> ShopExistAsync(int shopId);
        Task UpdateShopAsync(int UserID, UpdateShopDto updateShopDto);   
    }
}
