using ArteEva.Models;

namespace ArteEva.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        public   Task<Cart?> GetCartWithItemsAsync(int userId);

    }
}
