using ArteEva.Models;

namespace ArteEva.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<Product> GetProductWithImagesAsync(int productId);
    }
}
