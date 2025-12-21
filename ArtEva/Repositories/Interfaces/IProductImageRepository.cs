using ArteEva.Models;

namespace ArteEva.Repositories
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        Task<IEnumerable<ProductImage>> GetImagesByProductIdWithTracking(int productId);
    }
}
