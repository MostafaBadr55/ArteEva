using ArteEva.Data;
using ArteEva.Models;
using Microsoft.EntityFrameworkCore;

namespace ArteEva.Repositories
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdWithTracking(int productId)
        {
            IEnumerable<ProductImage> images = Query().AsTracking()
                .Where(i=>i.ProductId == productId).ToList();
            return images;
        }
    }
}
