using ArteEva.Data;
using ArteEva.Models;
using ArtEva.DTOs.Shop;
using ArteEva.Repositories;
using Microsoft.EntityFrameworkCore;
using ArtEva.Models.Enums;

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

        public async Task<ShopDto> CreateShopAsync(int userId, CreateShopDto dto)
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

            return MapToDto(shop);
        }

        public async Task<ShopDto> GetShopByOwnerIdAsync(int userId)
        {
            var shop = await _context.Shops
                .FirstOrDefaultAsync(s => s.OwnerUserId == userId);

            if (shop == null)
            {
                return null;
            }

            return MapToDto(shop);
        }

        public async Task<ShopDto> GetShopByIdAsync(int shopId)
        {
            var shop = await _shopRepository.GetByIdAsync(shopId);

            if (shop == null)
            {
                throw new Exception("Shop not found");
            }

            return MapToDto(shop);
        }

        public async Task<IEnumerable<ShopDto>> GetPendingShopsAsync()
        {
            var shops = await _context.Shops
                .Where(s => s.Status == ShopStatus.Pending)
                .ToListAsync();

            return shops.Select(MapToDto);
        }

        public async Task<ShopDto> ApproveShopAsync(int shopId)
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

            _shopRepository.Update(shop);
            await _context.SaveChangesAsync();

            return MapToDto(shop);
        }

        public async Task<ShopDto> RejectShopAsync(int shopId, RejectShopDto dto)
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

            _shopRepository.Update(shop);
            await _context.SaveChangesAsync();

            return MapToDto(shop);
        }
        public async Task<bool> ShopExistAsync(int shopId)
        {
            var shop = _shopRepository.GetByIdAsync;
            if (shop == null)
                return false;
            else
                return true;
        }
        #region mapping
        private ShopDto MapToDto(Shop shop)
        {
            return new ShopDto
            {
                //Id = shop.Id,
                OwnerUserId = shop.OwnerUserId,
                Name = shop.Name,
                ImageUrl = shop.ImageUrl,
                Description = shop.Description,
                Status = shop.Status,
                //RejectionMessage = shop.RejectionMessage,
                RatingAverage = shop.RatingAverage
            };
        }
        #endregion
    }
}
