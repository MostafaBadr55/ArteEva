using ArteEva.Data;
using ArteEva.Models;
using ArteEva.Repositories;
using ArtEva.DTOs.Product;
using ArtEva.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ArtEva.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IShopService _shopService;
        private readonly ApplicationDbContext _context;

        public ProductService(
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            IShopService shopService,
            ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _shopService = shopService;
            _context = context;
        }

        public async Task<ProductDto> CreateProductAsync(
        int userId, CreateProductDto dto)
        {
            // -------------------------------------------------------------
            // 1. Verify shop exists AND belongs to this user
            // -------------------------------------------------------------
            var shop = await _shopService.GetShopByIdAsync(dto.ShopId);

            if (shop == null)
                throw new Exception("Shop not found.");

            if (shop.OwnerUserId != userId)
                throw new Exception("You are not the owner of this shop.");

            if (shop.Status != ShopStatus.Active)
                throw new Exception("Adding products is not allowed for inactive shops");

            // -------------------------------------------------------------
            // 2. Validate Category
            // -------------------------------------------------------------
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId && !c.IsDeleted);
            if (!categoryExists)
                throw new Exception("Invalid category.");

            // -------------------------------------------------------------
            // 3. Validate SubCategory belongs to Category
            // -------------------------------------------------------------
            var subCategory = await _context.SubCategories
                .FirstOrDefaultAsync(sc =>
                    sc.Id == dto.SubCategoryId &&
                    sc.CategoryId == dto.CategoryId &&
                    !sc.IsDeleted);

            if (subCategory == null)
                throw new Exception("Invalid subcategory or does not belong to category.");

            // -------------------------------------------------------------
            // 4. Validate SKU uniqueness inside this shop
            // -------------------------------------------------------------
            var skuExists = await _context.Products
                .AnyAsync(p =>
                    p.SKU == dto.SKU &&
                    p.ShopId == dto.ShopId &&
                    !p.IsDeleted);

            if (skuExists)
                throw new Exception("SKU already exists in this shop.");

            // -------------------------------------------------------------
            // 5. Create Product (Always IsPublished = false)
            // -------------------------------------------------------------
            var product = new Product
            {
                ShopId = dto.ShopId,
                CategoryId = dto.CategoryId,
                SubCategoryId = dto.SubCategoryId,
                Title = dto.Title,
                SKU = dto.SKU,
                Price = dto.Price,
                IsPublished = false,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);
            await _context.SaveChangesAsync();  // Product gets its ID here

            // -------------------------------------------------------------
            // 6. Add Images
            // -------------------------------------------------------------
            if (dto.Images != null && dto.Images.Any())
            {
                var images = dto.Images.Select(img => new ProductImage
                {
                    ProductId = product.Id,
                    Url = img.Url,
                    AltText = img.AltText,
                    SortOrder = img.SortOrder,
                    IsPrimary = img.IsPrimary,
                    CreatedAt = DateTime.UtcNow
                });

                await _productImageRepository.AddRangeAsync(images);
                await _context.SaveChangesAsync();
            }

            // -------------------------------------------------------------
            // 7. Return DTO
            // -------------------------------------------------------------
            return new ProductDto
            {
                Id = product.Id,
                ShopId = product.ShopId,
                Title = product.Title,
                Price = product.Price,
                IsPublished = product.IsPublished
            };
        }

    }
}
