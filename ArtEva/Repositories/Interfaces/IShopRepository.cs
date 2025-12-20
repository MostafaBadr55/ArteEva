using ArteEva.Models;
using ArtEva.Application.Products.Specifications;
using ArtEva.DTOs.Shop;

namespace ArteEva.Repositories
{
    public interface IShopRepository : IRepository<Shop>
    {
       IQueryable<Shop> GetShopByOwnerId(int userId);
        IQueryable<Shop> GetPendingShops();
        Task<IReadOnlyList<Shop>> GetPagedAsync(
                ISpecification<Shop> specification,
                int pageNumber,
                int pageSize);

        Task<int> CountAsync(ISpecification<Shop> specification);

    
    }
}
