using ArteEva.Data;
using ArteEva.Models;
using ArtEva.Application.Products.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArteEva.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Product> GetProductWithImagesAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == productId);
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetPagedAsync(
        ISpecification<Product> specification,
        int pageNumber,
        int pageSize)
        {
            return await _context.Products
                .Where(specification.Criteria)
                .Include(p => p.ProductImages)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<Product> specification)
        {
            return await _context.Products
                .Where(specification.Criteria)
                .CountAsync();
        }
    }
}
