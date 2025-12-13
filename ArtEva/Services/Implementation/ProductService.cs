using ArteEva.Data;
using ArteEva.Models;
using ArteEva.Repositories;
using ArtEva.DTOs.Pagination;
using ArtEva.DTOs.Pagination.Product;
using ArtEva.DTOs.Product;
using ArtEva.DTOs.ProductImage;
using ArtEva.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

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
        #region Paged products 
        //READ PAGED BY SHOP
        // Master dynamic paging method
        public async Task<PagedResult<ProductListItemDto>> GetPagedProductsAsync(
            Expression<Func<Product, bool>> filter,
            int pageNumber,
            int pageSize)
        {
            // defensive defaults
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Max(1, pageSize);

            var items = await _productRepository.GetPagedProductsWithImagesAsync(filter, pageNumber, pageSize);
            var total = await _productRepository.CountAsync(filter);

            var dtoItems = items.Select(MapToListItemDto).ToList();

            return new PagedResult<ProductListItemDto>
            {
                Items = dtoItems,
                TotalCount = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        //Wrapper for admin: All products
        public Task<PagedResult<ProductListItemDto>> GetAdminAllProductsPagedAsync(int page, int size)
        {
            return GetPagedProductsAsync(p => true, page, size);
        }
        // admin wrapper: pending (IsPublished == false)
        public Task<PagedResult<ProductListItemDto>> GetAdminPendingProductsAsync(int pageNumber, int pageSize)
        {
            Expression<Func<Product, bool>> filter = p => p.IsPublished == false;
            return GetPagedProductsAsync(filter, pageNumber, pageSize);
        }

        // admin wrapper: approved (IsPublished == true)
        public Task<PagedResult<ProductListItemDto>> GetAdminApprovedProductsAsync(int pageNumber, int pageSize)
        {
            Expression<Func<Product, bool>> filter = p => p.IsPublished == true && p.ApprovalStatus== ProductApprovalStatus.Approved;
            return GetPagedProductsAsync(filter, pageNumber, pageSize);
        }

        // shop owner: active products for this shop
        public Task<PagedResult<ProductListItemDto>> GetShopActiveProductsAsync(int shopId, int pageNumber, int pageSize)
        {
            Expression<Func<Product, bool>> filter = p => p.ShopId == shopId && p.Status == ProductStatus.Active;
            return GetPagedProductsAsync(filter, pageNumber, pageSize);
        }

        // shop owner: inactive products for this shop
        public Task<PagedResult<ProductListItemDto>> GetShopInactiveProductsAsync(int shopId, int pageNumber, int pageSize)
        {
            Expression<Func<Product, bool>> filter = p => p.ShopId == shopId && p.Status == ProductStatus.InActive;
            return GetPagedProductsAsync(filter, pageNumber, pageSize);
        }
        
        // Buyer Get all Products
        public Task<PagedResult<ProductListItemDto>> GetAllActiveProductsAsync( int pageNumber, int pageSize)
        {
            Expression<Func<Product, bool>> filter = p => p.ApprovalStatus == ProductApprovalStatus.Approved && p.Status == ProductStatus.Active;
            return GetPagedProductsAsync(filter, pageNumber, pageSize);
        }
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
            product.Status = ProductStatus.InActive;
            product.ApprovalStatus = ProductApprovalStatus.Pending;

             _productRepository.UpdateAsync(product);
            await _productRepository.SaveChanges();

            // update images ONLY if dto.Images exists
            if (dto.Images != null)
            {
                await UpdateProductImages(product, dto.Images);
            }

            var updated = await _productRepository.GetProductWithImagesAsync(dto.productId);
            return MapToProductDto(updated);
        }

        // Admin Actions
        public async Task<ApprovedProductDto> ApproveProductAsync(int productId)
        {
            var product = await _productRepository.GetByIDWithTrackingAsync(productId);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productId} was not found.");

            product.IsPublished = true;
            product.ApprovalStatus = ProductApprovalStatus.Approved;
            
            await _productRepository.SaveChanges();
            var approved = new ApprovedProductDto
            {
                Id = productId,
                Title = product.Title,
                SKU = product.SKU,
                IsPublished = product.IsPublished,
                ApprovalMessage = $"Product with Title: {product.Title} is published now"
            };

            return approved;
        }

        public async Task<RejectedProductDto> RejectProductAsync(ProductToReject dto)
        {
            var product = await _productRepository.GetByIDWithTrackingAsync(dto.ProductId);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {dto.ProductId} was not found.");

            product.IsPublished = false;
            product.ApprovalStatus = ProductApprovalStatus.Rejected;
            product.RejectionMessage = dto.RejectionMessage;

            await _productRepository.SaveChanges();

            var rejProduct = await _productRepository.GetByIdAsync(product.Id);
            var rejected = new RejectedProductDto
            {
                Id = rejProduct.Id,
                Title = rejProduct.Title,
                SKU = rejProduct.SKU,
                IsPublished = rejProduct.IsPublished,
                RejectionMessage = rejProduct.RejectionMessage
            };

            return rejected;
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

        #region Mapping
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

        private ProductListItemDto MapToListItemDto(Product p)
        {
            return new ProductListItemDto
            {
                Id = p.Id,
                ShopId = p.ShopId,
                Title = p.Title,
                SKU = p.SKU,
                Price = p.Price,
                Status = p.Status,
                IsPublished = p.IsPublished,
                Images = p.ProductImages?
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        Url = i.Url,
                        AltText = i.AltText,
                        SortOrder = i.SortOrder,
                        IsPrimary = i.IsPrimary
                    }).ToList() ?? new List<ProductImageDto>()
            };
        }

        #endregion
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

