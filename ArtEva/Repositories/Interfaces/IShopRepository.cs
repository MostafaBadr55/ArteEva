using ArteEva.Models;
using ArtEva.DTOs.Shop;

namespace ArteEva.Repositories
{
    public interface IShopRepository : IRepository<Shop>
    {
       IQueryable<Shop> GetShopByOwnerId(int userId);
        IQueryable<Shop> GetPendingShops();

    }
}
