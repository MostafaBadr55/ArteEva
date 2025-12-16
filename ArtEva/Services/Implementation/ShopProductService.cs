using ArteEva.Data;
using ArteEva.Models;
using ArteEva.Repositories;
using ArtEva.DTOs.Pagination;
using ArtEva.DTOs.Pagination.Product;
using ArtEva.DTOs.Product;
using ArtEva.DTOs.ProductImage;
using ArtEva.DTOs.Shop;
using ArtEva.DTOs.Shop.Products;
using ArtEva.Models.Enums;
using ArtEva.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArtEva.Services.Implementation
{
    public class ShopProductService : IShopProductService
    {
        private readonly IShopService _shopService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService;
         private readonly IConfiguration _config;

        public ShopProductService(
            IShopService shopService,
             IProductService productService
             , IConfiguration config,
              ICategoryService categoryService,
            ISubCategoryService subCategoryService
            )
  
           
         {
            _shopService = shopService;
            _productService = productService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
             _config = config;
        }
        public async Task<CreatedShopDto> GetShopByOwnerIdAsync(int userId, int pageNumber, int pageSize)
        {
             
             CreatedShopDto? shop = 
              await _shopService.GetShopByOwnerIdAsync(userId);

            if (shop == null)
                throw new ValidationException("You don't have an active shop yet.");


            var res1 = await GetShopActiveProductsAsync(shop.Id, pageNumber, pageSize);
            var res2 = await GetShopInactiveProductsAsync(shop.Id, pageNumber, pageSize);

            var ActiveProductDtos = res1.Items.Select(s => new ActiveProductDto()
            {
                Title = s.Title,
                Images = s.Images,
                Price = s.Price,
                Status = s.Status,

            }).ToList();

            var InactiveProductDtos = res2.Items.Select(s => new InActiveProductDto()
            {
                Title = s.Title,
                Images = s.Images,
                Price = s.Price,
                Status = s.Status,

            }).ToList();
            shop.inActiveProductDtos = InactiveProductDtos;
            shop.activeProductDtos = ActiveProductDtos;
            if (shop == null)
            {
                return null;
            }

            return shop;
        }

        // Product Section 
        #region Product section 

        public async Task<CreatedProductDto> CreateShopProductAsync (int userId, CreateProductDto dto)
        {
            // Validate input & business rules
            await ValidateProductCreationAsync(userId, dto.ShopId, dto.CategoryId, dto.SubCategoryId);

            var product= await _productService.CreateProductAsync(dto);

            return product;
        }

        //Get Shop products paginated
        // shop owner: active products for this shop
        public Task<PagedResult<ProductListItemDto>> GetShopActiveProductsAsync(int shopId, int pageNumber, int pageSize)
        {
            Expression<Func<Product, bool>> filter = p => p.ShopId == shopId && p.Status == ProductStatus.Active;
            return _productService.GetPagedProductsAsync(filter, pageNumber, pageSize);
        }

        // shop owner: inactive products for this shop
        public Task<PagedResult<ProductListItemDto>> GetShopInactiveProductsAsync(int shopId, int pageNumber, int pageSize)
        {
            Expression<Func<Product, bool>> filter = p => p.ShopId == shopId && p.Status == ProductStatus.InActive;
            return _productService.GetPagedProductsAsync(filter, pageNumber, pageSize);
        }

        #region Update Product
        // UPDATE PRODUCT
        public async Task<CreatedProductDto> UpdateShopProductAsync(int userId, UpdateProductDto dto)
        {

            await ValidateProductCreationAsync(
                userId, dto.ShopId, dto.CategoryId, dto.SubCategoryId);
            var product = await _productService.GetProductForUpdateAsync(dto.productId);
            product.Title = dto.Title;
            product.CategoryId = dto.CategoryId;
            product.SubCategoryId = dto.SubCategoryId;
            await _productService.UpdateProductBaseInfoAsync(product);
            if (dto.Images != null)
                await _productService.UpdateProductImagesAsync(product, dto.Images);

            // 7) Return DTO
            return MapToCreatedProductDto(product);

        }

        public async Task<UpdatedProductStatusDto> UpdateProductStatusAsync(int userId, int shopId, int productId, ProductStatus status)
        {
            var shop = await _shopService
                .EnsureUserCanManageShopProductsAsync(userId, shopId);

            // 2) Load product
            var product = await _productService.GetProductForUpdateAsync(productId);

            if (product.ShopId != shopId)
                throw new ValidationException("Product does not belong to this shop.");

            // 3) Update status
            await _productService.UpdateProductStatusInternalAsync(product, status);

            return new UpdatedProductStatusDto
            {
                IsUpdated = true,
                ProductName = product.Title,
                UpdatedStatus = product.Status
            };
        }

        public async Task<UpdatedProductPriceDto> UpdateProductPriceAsync(int userId, int shopId, int productId, decimal newPrice)
        {
            await _shopService.EnsureShopOwnershipAsync(userId, shopId);

            var product = await _productService.GetProductForUpdateAsync(productId);

            if (product.ShopId != shopId)
                throw new ValidationException("Product does not belong to this shop.");

            // 3) Update price
            await _productService.UpdateProductPriceInternalAsync(product, newPrice);

            return new UpdatedProductPriceDto
            {
                IsChanged = true,
                ProductName = product.Title,
                Price = product.Price
            };
        }

        #endregion

        #region Private methods
        private async Task ValidateProductCreationAsync(int userId, int shopId, int categoryId, int subCategoryId)
        {
            var shop = await _shopService
                       .EnsureUserCanManageShopProductsAsync(userId, shopId);

            // 2. Validate category exists
            var categoryExists = await _categoryService.ValidateCategoryExistsAsync(categoryId);

            if (!categoryExists)
                throw new ValidationException("Invalid category.");

            // 3. Validate subcategory ownership
            var subCategory = await _subCategoryService.ValidateSubCategoryAsync(subCategoryId, categoryId);

            if (!subCategory)
                throw new ValidationException("Invalid subcategory or does not belong to the selected category.");
        }

      

    

        #endregion

        #region Products mapping 
        private CreatedProductDto MapToCreatedProductDto(Product product)
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

        #endregion
        #endregion

    }
}
