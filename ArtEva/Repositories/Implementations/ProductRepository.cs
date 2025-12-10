using ArteEva.Data;
using ArteEva.Models;
using Microsoft.EntityFrameworkCore;

namespace ArteEva.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Product> GetProductWithImagesAsync (int productId)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == productId);
            return product;
        }
    }
}
