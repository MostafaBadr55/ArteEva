using ArteEva.Models;
using System.Linq.Expressions;

namespace ArteEva.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<Product> GetProductWithImagesAsync(int productId);
        Task<IEnumerable<Product>> GetPagedProductsWithImagesAsync(
        Expression<Func<Product, bool>> predicate,
        int pageNumber,
        int pageSize);

        Task<int> CountAsync(Expression<Func<Product, bool>> predicate);
    }
}
