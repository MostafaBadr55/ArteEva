using ArteEva.Data;
using ArteEva.Models;

namespace ArteEva.Repositories
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        public ShopRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
