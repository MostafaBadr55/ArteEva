using ArteEva.Data;
using ArteEva.Models;
using ArteEva.Repositories;
using ArtEva.DTOs.Product;
using ArtEva.DTOs.ProductImage;
using ArtEva.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ArtEva.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IShopService _shopService;

        public ProductService(
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            ICategoryRepository categoryRepository,
            ISubCategoryRepository subCategoryRepository,
            IShopService shopService)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _shopService = shopService;
        
        }

        public async Task<CreatedProductDto> CreateProductAsync(int userId, CreateProductDto dto)
        {
            // Validate input & business rules
            await ValidateProductCreationAsync(userId, dto.ShopId,dto.CategoryId, dto.SubCategoryId);

            // Generate a unique SKU
            string sku = await GenerateUniqueSkuAsync(dto.ShopId, dto.CategoryId);

            // Build product entity
            var product = new Product
            {
                ShopId = dto.ShopId,
                CategoryId = dto.CategoryId,
                SubCategoryId = dto.SubCategoryId,
                Title = dto.Title,
                SKU = sku,
                Price = dto.Price,
                IsPublished = false,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChanges();

            // Save product images
            if (dto.Images != null && dto.Images.Any())
            {
                var images = dto.Images.Select(i => new ProductImage
                {
                    ProductId = product.Id,
                    Url = i.Url,
                    AltText = i.AltText,
                    SortOrder = i.SortOrder,
                    IsPrimary = i.IsPrimary,
                    CreatedAt = DateTime.UtcNow
                });

                await _productImageRepository.AddRangeAsync(images);
                await _productImageRepository.SaveChanges();
            }

            // Load complete product with images
            var loadedProduct = await _productRepository.GetProductWithImagesAsync(product.Id);

            return MapToProductDto(loadedProduct);
        }

        ////////////////////////////////////////////////////////////
        ///Get Actions
        /////////////////////////////////////////////////////////
        public async Task<ProductDetailsDto> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductWithImagesAsync(productId);

            if (product == null || product.IsDeleted)
                throw new KeyNotFoundException("Product not found.");

            return new ProductDetailsDto
            {
                Id = product.Id,
                ShopId = product.ShopId,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                Title = product.Title,
                SKU = product.SKU,
                Price = product.Price,
                IsPublished = product.IsPublished,
                Images = product.ProductImages?
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new CreatedProductImageDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        AltText = i.AltText,
                        SortOrder = i.SortOrder,
                        IsPrimary = i.IsPrimary
                    })
                    .ToList()
            };
        }
        #region Paged products by shop
        // READ PAGED BY SHOP
        //public async Task<PagedProductsDto> GetProductsByShopPagedAsync(int shopId, int page, int pageSize)
        //{
        //    var (items, total) = await _productRepository.GetPagedProductsAsync(shopId, page, pageSize);

        //    return new PagedProductsDto
        //    {
        //        TotalCount = total,
        //        Items = items.Select(MapToProductDto).ToList()
        //    };
        //}
        #endregion

        // UPDATE PRODUCT
        public async Task<CreatedProductDto> UpdateProductAsync(int userId, UpdateProductDto dto)
        {
            var product = await _productRepository.GetProductWithImagesAsync(dto.productId);

            if (product == null || product.IsDeleted)
                throw new ValidationException("Product not found.");

            await ValidateProductCreationAsync(
                userId, product.ShopId, dto.CategoryId, dto.SubCategoryId);

            // update product base data
            product.Title = dto.Title;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.SubCategoryId = dto.SubCategoryId;
            product.UpdatedAt = DateTime.UtcNow;

             _productRepository.Update(product);
            await _productRepository.SaveChanges();

            // update images ONLY if dto.Images exists
            if (dto.Images != null)
            {
                await UpdateProductImages(product, dto.Images);
            }

            var updated = await _productRepository.GetProductWithImagesAsync(dto.productId);
            return MapToProductDto(updated);
        }






        #region Private methods
        private async Task ValidateProductCreationAsync(int userId, int shopId, int categoryId, int subCategoryId)
        {
            // 1. Validate shop
            var shop = await _shopService.GetShopByIdAsync(shopId);

            if (shop == null)
                throw new ValidationException("Shop not found.");

            if (shop.OwnerUserId != userId)
                throw new ValidationException("You are not the owner of this shop.");

            if (shop.Status == ShopStatus.Suspended|| shop.Status == ShopStatus.Pending || shop.Status == ShopStatus.Rejected)
                throw new ValidationException("Adding products is not allowed in your shop status.");

            // 2. Validate category exists
            var categoryExists = await _categoryRepository.AnyAsync(c =>
                c.Id == categoryId && !c.IsDeleted);

            if (!categoryExists)
                throw new ValidationException("Invalid category.");

            // 3. Validate subcategory ownership
            var subCategory = await _subCategoryRepository.FirstOrDefaultAsync(sc =>
                sc.Id == subCategoryId &&
                sc.CategoryId == categoryId &&
                !sc.IsDeleted);

            if (subCategory == null)
                throw new ValidationException("Invalid subcategory or does not belong to the selected category.");
        }
    

        private CreatedProductDto MapToProductDto(Product product)
        {
            return new CreatedProductDto
            {
                Id = product.Id,
                ShopId = product.ShopId,
                CategoryId = product.CategoryId,
                SubCategoryId = product.SubCategoryId,
                Title = product.Title,
                SKU = product.SKU,
                Price = product.Price,
                IsPublished = product.IsPublished,

                Images = product.ProductImages?
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new CreatedProductImageDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        AltText = i.AltText,
                        SortOrder = i.SortOrder,
                        IsPrimary = i.IsPrimary
                    })
                    .ToList()
            };
        }

        private string GenerateSku(int shopId, int categoryId)
        {
            // Example: SHP5-CAT12-AX93DK
            string random = Guid.NewGuid().ToString("N")[..6].ToUpper();
            return $"SHP{shopId}-CAT{categoryId}-{random}";
        }
        private async Task<string> GenerateUniqueSkuAsync(int shopId, int categoryId)
        {
            string sku;

            do
            {
                sku = GenerateSku(shopId, categoryId);

            } while (await _productRepository.AnyAsync(p =>
                p.SKU == sku &&
                p.ShopId == shopId &&
                !p.IsDeleted
            ));

            return sku;
        }

        #region image helper
        private async Task UpdateProductImages(Product product, List<UpdateProductImage> imagesDto)
        {
            var existing = product.ProductImages.ToList();
            var incoming = imagesDto;

            var existingIds = existing.Select(x => x.Id).ToList();
            var incomingIds = incoming.Where(i => i.Id != 0).Select(i => i.Id).ToList();

            // 1. DELETE removed images
            var toDelete = existing.Where(e => !incomingIds.Contains(e.Id)).ToList();
            if (toDelete.Any())
                _productImageRepository.RemoveRange(toDelete);

            // 2. UPDATE existing images
            foreach (var incomingImg in incoming.Where(i => i.Id != 0))
            {
                var entity = existing.First(i => i.Id == incomingImg.Id);

                entity.Url = incomingImg.Url;
                entity.AltText = incomingImg.AltText;
                entity.SortOrder = incomingImg.SortOrder;
                entity.IsPrimary = incomingImg.IsPrimary;
                entity.UpdatedAt = DateTime.UtcNow;
            }

            // 3. ADD new images
            var newImages = incoming
                .Where(i => i.Id == 0)
                .Select(i => new ProductImage
                {
                    ProductId = product.Id,
                    Url = i.Url,
                    AltText = i.AltText,
                    SortOrder = i.SortOrder,
                    IsPrimary = i.IsPrimary,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

            if (newImages.Any())
                await _productImageRepository.AddRangeAsync(newImages);

            await _productImageRepository.SaveChanges();
        }


        #endregion

        #endregion

    }
}

