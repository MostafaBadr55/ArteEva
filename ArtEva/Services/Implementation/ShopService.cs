using ArteEva.Data;
using ArteEva.Models;
using ArtEva.DTOs.Shop;
using ArteEva.Repositories;
using Microsoft.EntityFrameworkCore;
using ArtEva.Models.Enums;
using ArtEva.Services.Implementation;
using System.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArtEva.DTOs.Pagination.Product;
using ArtEva.DTOs.Shop.Products;

namespace ArtEva.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService(IShopRepository shopRepository   ,IConfiguration config)
        {
            _shopRepository = shopRepository;
             
        }
        public async Task<CreatedShopDto> GetShopByOwnerIdAsync(int userId)
        {
            CreatedShopDto? shop =
              await _shopRepository.GetShopByOwnerId(userId)
             //.Where(s=>s.Status==ShopStatus.Active || s.Status == ShopStatus.Inactive)   
             .Select(s => new CreatedShopDto
             {
                 Id = s.Id,
                 OwnerUserName = s.Owner.UserName,
                 Name = s.Name,
                 ImageUrl = $"uploads/shops/{s.ImageUrl}",
                 Description = s.Description,
                 Status = s.Status,
                 RatingAverage = s.RatingAverage,
             }).FirstOrDefaultAsync();
             
            return shop;
        }

        public async Task CreateShopAsync(int userId, CreateShopDto dto)
        {
            var existingShop = await _shopRepository
                .GetShopByOwnerId(userId)
                .FirstOrDefaultAsync();

            if (existingShop != null)
            {
                throw new NotValidException("User already has a shop");
            }

            var shop = new Shop
            {
                OwnerUserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                Status = ShopStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _shopRepository.AddAsync(shop);
            await _shopRepository.SaveChanges();
        }



        public async Task<ExistShopDto> GetShopByIdAsync(int shopId)
        {
            var shop = await LoadShopOrThrowAsync(shopId);

            if (shop.Status != ShopStatus.Active && shop.Status != ShopStatus.Inactive)
            {
                throw new ForbiddenException("Shop is not available");
            }

            shop.ImageUrl = $"uploads/shops/{shop.ImageUrl}";

            return MapToDto2(shop);
        }

        public async Task<IEnumerable<PendingShopDto>> GetPendingShopsAsync()
        {
            var shops = await _shopRepository.GetPendingShops()
                .ToListAsync();
            foreach (var shop in shops)
            {
                shop.ImageUrl = $"uploads/shops/{shop.ImageUrl}";
            }

            return shops.Select(MapToDtoPending);
        }

        public async Task<ApproveShopDto> ApproveShopAsync(int shopId)
        {
            var shop =await LoadShopOrThrowAsync(shopId);

            if (shop.Status != ShopStatus.Pending)
            {
                throw new Exception("Only pending shops can be approved");
            }

            shop.Status = ShopStatus.Active;
            shop.RejectionMessage = null;
            shop.UpdatedAt = DateTime.UtcNow;

           await _shopRepository.UpdateAsync(shop);
            await _shopRepository.SaveChanges();

            return MapToDtoApprove(shop);
        }

        public async Task<RejectedShopDto> RejectShopAsync(int shopId, RejectShopDto dto)
        {
            var shop = await LoadShopOrThrowAsync(shopId);

            if (shop.Status != ShopStatus.Pending)
            {
                throw new Exception("Only pending shops can be rejected");
            }

            shop.Status = ShopStatus.Rejected;
            shop.RejectionMessage = dto.RejectionMessage;
            shop.UpdatedAt = DateTime.UtcNow;

           await _shopRepository.UpdateAsync(shop);
            await _shopRepository.SaveChanges();

            return MapToDto3(shop);
        }
        public async Task<bool> ShopExistAsync(int shopId)
        {
            var shop = _shopRepository.GetByIdAsync;
            if (shop == null)
                return false;
            else
                return true;
        }

        #region MyRegion Update Shop

        public async Task UpdateShopInfoAsync(int userId,UpdateShopDto dto)
        {
            var shop = await LoadShopForUpdateAsync(dto.ShopId, userId);

            EnsureShopCanBeUpdated(shop);

            shop.Name = dto.ShopName;
            shop.ImageUrl = dto.ImageUrl;
            shop.Description = dto.Description;

            /// Rule
            shop.Status = ShopStatus.Pending;

            shop.UpdatedAt = DateTime.UtcNow;

            await _shopRepository.SaveChanges();
        }


        public async Task UpdateShopStatusBySellerAsync(int userId, int shopId, ShopStatus newStatus)
        {
            var shop = await _shopRepository.GetByIDWithTrackingAsync(shopId);

            if (shop == null)
                throw new NotFoundException("Shop not found");

            // Ownership check
            if (shop.OwnerUserId != userId)
                throw new UnauthorizedAccessException("You are not the owner of this shop");

            // Status change allowed only Active <-> Inactive
            if (!((shop.Status == ShopStatus.Active && newStatus == ShopStatus.Inactive) ||
                  (shop.Status == ShopStatus.Inactive && newStatus == ShopStatus.Active)))
            {
                throw new NotValidException(
                    $"Sellers can only toggle status between Active and Inactive. Current status: {shop.Status}"
                );
            }

            shop.Status = newStatus;
            shop.UpdatedAt = DateTime.UtcNow;

            await _shopRepository.SaveChanges();
        }



        #endregion

        #region MyRegion

        public async Task<Shop> EnsureShopOwnershipAsync(int userId, int shopId)
        {
            var shop = await LoadShopOrThrowAsync(shopId);

            if (shop.OwnerUserId != userId)
                throw new NotValidException("You are not the owner of this shop.");

            return shop;
        }
      
        public async Task EnsureShopAllowsProductManagementAsync(int shopId)
        {
            var shop = await LoadShopOrThrowAsync(shopId);

            EnsureShopAllowsProductManagement(shop);
        }
        public async Task<Shop> EnsureUserCanManageShopProductsAsync(int userId,int shopId)
        {
            var shop = await LoadShopOrThrowAsync(shopId);

            if (shop.OwnerUserId != userId)
                throw new NotValidException("You are not the owner of this shop.");

            EnsureShopAllowsProductManagement(shop);

            return shop;
        }
         

        #endregion

        #region Private Functions
        private async Task<Shop> LoadShopOrThrowAsync(int shopId)
        {
            var shop = await _shopRepository.GetByIdAsync(shopId);

            if (shop == null)
                throw new NotValidException("Shop not found.");

            return shop;
        }
        private void EnsureShopAllowsProductManagement(Shop shop)
        {
            if (shop.Status != ShopStatus.Active &&
                shop.Status != ShopStatus.Inactive)
                throw new NotValidException(
                    $"You cannot manage products while shop status is '{shop.Status}'."
                );
        }
        private void EnsureShopCanBeUpdated(Shop shop)
        {
            if (shop.Status != ShopStatus.Active &&
                shop.Status != ShopStatus.Inactive)
                throw new NotValidException(
                    $"Shop cannot be updated while status is '{shop.Status}'."
                );
        }
        private async Task<Shop> LoadShopForUpdateAsync(int shopId, int userId)
        {
            var shop = await _shopRepository.GetByIDWithTrackingAsync(shopId);

            if (shop == null)
                throw new NotFoundException("Shop not found");

            if (shop.OwnerUserId != userId)
                throw new UnauthorizedAccessException("Not allowed to update this shop");

            return shop;
        }


        #endregion

        #region mapping
        private CreatedShopDto MapToDto(ArteEva.Models.Shop shop)
        {
            return new CreatedShopDto
            {
                Id = shop.Id,
             
                Name = shop.Name,
                ImageUrl = shop.ImageUrl,
                Description = shop.Description,
                Status = shop.Status,
                RatingAverage = shop.RatingAverage
            };
        }

        private ExistShopDto MapToDto2(Shop shop)
        {
            return new ExistShopDto
            {
                OwnerUserId = shop.OwnerUserId,
                Name = shop.Name,
                ImageUrl = shop.ImageUrl,
                Description = shop.Description,
                Status = shop.Status,
                RatingAverage = shop.RatingAverage
            };
        }


        private RejectedShopDto MapToDto3(Shop shop)
        {
            return new RejectedShopDto
            {
                Name = shop.Name,
                RejectionMessage = shop.RejectionMessage,
                OwnerUserName = shop.Owner.UserName,
                ImageUrl = shop.ImageUrl,
                Description = shop.Description,
                Status = shop.Status,
                RatingAverage = shop.RatingAverage
            };
        }

        private PendingShopDto MapToDtoPending(Shop shop)
        {
            return new PendingShopDto
            {
                OwnerUserId = shop.Id,
                Id=shop.Id,
                Name = shop.Name,
                ImageUrl = shop.ImageUrl,
                Description = shop.Description,
                Status = shop.Status,
                RatingAverage = shop.RatingAverage
            };
        }   
        private ApproveShopDto MapToDtoApprove(Shop shop)
        {
            return new ApproveShopDto
            {
                OwnerUserId = shop.OwnerUserId,
                Name = shop.Name,
                ImageUrl = shop.ImageUrl,
                Description = shop.Description,
                Status = shop.Status,
                RatingAverage = shop.RatingAverage
            };
        }

        #endregion
  
    
    }
}
