using ArtEva.DTOs.Shop;

namespace ArtEva.Services
{
    public interface IShopService
    {
        Task CreateShopAsync(int userId, CreateShopDto dto);
        Task<CreatedShopDto> GetShopByOwnerIdAsync(int userId);
        Task<ExistShopDto> GetShopByIdAsync(int shopId);
        Task<IEnumerable<CreatedShopDto>> GetPendingShopsAsync();
        Task<CreatedShopDto> ApproveShopAsync(int shopId);
        Task<RejectedShopDto> RejectShopAsync(int shopId, RejectShopDto dto);
        Task<bool> ShopExistAsync(int shopId);
        Task UpdateShopAsync(int UserID, UpdateShopDto updateShopDto);   
    }
}
