using ArteEva.Data;
using ArteEva.Models;

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
    }
}
