using ArteEva.Data;
using ArteEva.Models;

namespace ArteEva.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
