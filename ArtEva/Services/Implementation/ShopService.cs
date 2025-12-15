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
        private readonly ApplicationDbContext _context;

        public ShopService(IShopRepository shopRepository, ApplicationDbContext context)
        {
            _shopRepository = shopRepository;
            _context = context;
        }

        public async Task<ExistShopDto> ValidateShopOwnershipAsync(int userId, int shopId)
        {
            // 1. Validate shop
            var shop = await GetShopByIdAsync(shopId);

            if (shop == null)
                throw new ValidationException("Shop not found.");

            if (shop.OwnerUserId != userId)
                throw new ValidationException("You are not the owner of this shop.");
            return shop;
        }
        public async Task ValidateShopCanAddProductsAsync(int shopId)
        {
            var shop = await _shopRepository.GetByIdAsync(shopId);

            if (shop == null)
                throw new ValidationException("Shop not found.");

            if (shop.Status != ShopStatus.Active)
                throw new ValidationException(
                    $"You cannot add or modify products while shop status is '{shop.Status}'."
                );
        }

        public async Task CreateShopAsync(int userId, CreateShopDto dto)
        {
            // Check if user already has a shop
            var existingShop = await _context.Shops
                .FirstOrDefaultAsync(s => s.OwnerUserId == userId);

            if (existingShop != null)
            {
                throw new Exception("User already has a shop");
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
            await _context.SaveChangesAsync();
             
        }
         
      
        public async Task<ExistShopDto> GetShopByIdAsync(int shopId)
        {
            var shop = await _shopRepository.GetByIdAsync(shopId);

            if (shop == null)
            {
                throw new Exception("Shop not found");
            }

            return MapToDto2(shop);
        }

        public async Task<IEnumerable<PendingShopDto>> GetPendingShopsAsync()
        {
            var shops = await _context.Shops
                .Where(s => s.Status == ShopStatus.Pending)
                .ToListAsync();

            return shops.Select(MapToDtoPending);
        }

        public async Task<ApproveShopDto> ApproveShopAsync(int shopId)
        {
            var shop = await _shopRepository.GetByIdAsync(shopId);

            if (shop == null)
            {
                throw new Exception("Shop not found");
            }

            if (shop.Status != ShopStatus.Pending)
            {
                throw new Exception("Only pending shops can be approved");
            }

            shop.Status = ShopStatus.Active;
            shop.RejectionMessage = null;
            shop.UpdatedAt = DateTime.UtcNow;

           await _shopRepository.UpdateAsync(shop);
            await _context.SaveChangesAsync();

            return MapToDtoApprove(shop);
        }

        public async Task<RejectedShopDto> RejectShopAsync(int shopId, RejectShopDto dto)
        {
            var shop = await _shopRepository.GetByIdAsync(shopId);

            if (shop == null)
            {
                throw new Exception("Shop not found");
            }

            if (shop.Status != ShopStatus.Pending)
            {
                throw new Exception("Only pending shops can be rejected");
            }

            shop.Status = ShopStatus.Rejected;
            shop.RejectionMessage = dto.RejectionMessage;
            shop.UpdatedAt = DateTime.UtcNow;

           await _shopRepository.UpdateAsync(shop);
            await _context.SaveChangesAsync();

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
        

        public async Task UpdateShopAsync(int UserID, UpdateShopDto updateShopDto) { 

             
            var shop = await _shopRepository.GetByIDWithTrackingAsync(updateShopDto.ShopId);
            if (shop == null)
            {
                throw new NotFoundException("Shop not found");
            }

            if(UserID != shop.OwnerUserId)
                throw new UnauthorizedAccessException("Not allowed to update this shop");

            shop.Name = updateShopDto.ShopName;
            shop.ImageUrl = updateShopDto.ImageUrl;
            shop.Description = updateShopDto.Description;
            shop.UpdatedAt = DateTime.UtcNow;
              
            await _context.SaveChangesAsync();
             
        }

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
