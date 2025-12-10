using ArteEva.Data;
using ArteEva.Models;

namespace ArteEva.Repositories
{
    public class ShopFollowerRepository : Repository<ShopFollower>, IShopFollowerRepository
    {
        public ShopFollowerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
