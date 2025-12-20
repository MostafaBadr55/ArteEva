using ArteEva.Data;
using ArteEva.Models;
using ArtEva.Application.Products.Specifications;
using ArtEva.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ArteEva.Repositories
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        public ShopRepository(ApplicationDbContext context ) : base(context)
        {

        }

        public IQueryable<Shop> GetShopByOwnerId(int userId)
        {
           return  Query().Where(s => s.OwnerUserId == userId);
        }

        public IQueryable<Shop> GetPendingShops()
        {
            return GetAllAsync().Where(s => s.Status == ShopStatus.Pending);
        }

        public async Task<IReadOnlyList<Shop>> GetPagedAsync(
           ISpecification<Shop> specification,
           int pageNumber,
           int pageSize)
        {
            return await _context.Shops
                .Where(specification.Criteria)
                .OrderByDescending(s => s.UpdatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<Shop> specification)
        {
            return await _context.Shops
                .Where(specification.Criteria)
                .CountAsync();
        }

    }
}
