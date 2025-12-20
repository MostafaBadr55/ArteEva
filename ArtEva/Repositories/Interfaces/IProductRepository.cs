using ArteEva.Models;
using ArtEva.Application.Products.Specifications;
using System.Linq.Expressions;

namespace ArteEva.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task<Product> GetProductWithImagesAsync(int productId);
        Task<IReadOnlyList<Product>> GetPagedAsync(
        ISpecification<Product> specification,
        int pageNumber,
        int pageSize);

        Task<int> CountAsync(ISpecification<Product> specification);
    }
}
