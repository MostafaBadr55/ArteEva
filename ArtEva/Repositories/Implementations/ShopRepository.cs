using ArteEva.Data;
using ArteEva.Models;
using ArtEva.Models.Enums;

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

    }
}
