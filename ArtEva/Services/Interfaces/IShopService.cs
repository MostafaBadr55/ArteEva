using ArteEva.Models;
using ArtEva.Application.Shops.Quiries;
using ArtEva.DTOs.Shop;
<<<<<<< HEAD
=======
using ArtEva.Models.Enums;
using System.Threading.Tasks;
>>>>>>> 7ef7d5956491c35f60b9324084ee1e37d86f8eee

namespace ArtEva.Services
{
    public interface IShopService
    {
        public Task<ShopPagedResult<ExistShopDto>> GetShopsAsync(
                     ShopQueryCriteria criteria,
                     int pageNumber = 1,
                     int pageSize = 20);
        public Task UpdateShopInfoAsync(int userId, UpdateShopDto dto);
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
