using ArteEva.Models;
using ArtEva.DTOs.Shop;
using ArtEva.Models.Enums;
using System.Threading.Tasks;

namespace ArtEva.Services
{
    public interface IShopService
    {
        public   Task UpdateShopInfoAsync(int userId, UpdateShopDto dto);
        public Task UpdateShopStatusBySellerAsync(int userId, int shopId, ShopStatus newStatus);

        public Task<CreatedShopDto> GetShopByOwnerIdAsync(int userId);
         
        public Task<Shop> EnsureShopOwnershipAsync(int userId, int shopId);

        public Task EnsureShopAllowsProductManagementAsync(int shopId);

        public Task<Shop> EnsureUserCanManageShopProductsAsync(int userId, int shopId);

        Task CreateShopAsync(int userId, CreateShopDto dto);
        Task<ExistShopDto> GetShopByIdAsync(int shopId);
        Task<IEnumerable<PendingShopDto>> GetPendingShopsAsync();
        Task<ApproveShopDto> ApproveShopAsync(int shopId);
        Task<RejectedShopDto> RejectShopAsync(int shopId, RejectShopDto dto);
        Task<bool> ShopExistAsync(int shopId);
     }
}
