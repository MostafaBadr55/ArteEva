using ArteEva.Data;
using ArteEva.Models;
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

        public async Task<IEnumerable<Product>> GetPagedProductsWithImagesAsync(
           Expression<Func<Product, bool>> predicate,
           int pageNumber,
           int pageSize)
        {
            return await _context.Set<Product>()
                .Where(predicate)
                .Where(p => !p.IsDeleted)
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.CreatedAt) // sensible default ordering
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Set<Product>()
                .Where(predicate)
                .Where(p => !p.IsDeleted)
                .CountAsync();
        }
    }
}
