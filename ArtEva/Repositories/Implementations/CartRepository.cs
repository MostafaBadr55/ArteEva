using ArteEva.Data;
using ArteEva.Models;
using Microsoft.EntityFrameworkCore;

namespace ArteEva.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Cart?> GetCartWithItemsAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId &&
                    !c.IsDeleted);
        }
    }
}
