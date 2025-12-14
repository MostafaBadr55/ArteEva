using ArteEva.Models;
using ArtEva.DTOs.Pagination;
using ArtEva.DTOs.Pagination.Product;
using ArtEva.DTOs.Product;
using ArtEva.Models.Enums;
using System.Linq.Expressions;

namespace ArtEva.Services
{
    public interface IProductService
    {
        public Task<CreatedProductDto> CreateProductAsync(int userId, CreateProductDto dto);
        public Task<CreatedProductDto> UpdateProductAsync(int userId, UpdateProductDto dto);
        Task<UpdatedProductPriceDto> UpdateProductPriceAsync(int userId, int shopId, int productId, decimal newPrice);
        Task<UpdatedProductStatusDto> UpdateProductStatusAsync(int userId, int shopId, int productId, ProductStatus status);
        public Task<ProductDetailsDto> GetProductByIdAsync(int productId);

        // master dynamic paging method
        Task<PagedResult<ProductListItemDto>> GetPagedProductsAsync(
            Expression<Func<Product, bool>> filter,
            int pageNumber,
            int pageSize);

        // wrappers for explicit use cases
        Task<PagedResult<ProductListItemDto>> GetAdminPendingProductsAsync(int pageNumber, int pageSize);
        Task<PagedResult<ProductListItemDto>> GetAdminApprovedProductsAsync(int pageNumber, int pageSize);

        Task<PagedResult<ProductListItemDto>> GetShopActiveProductsAsync(int shopId, int pageNumber, int pageSize);
        Task<PagedResult<ProductListItemDto>> GetShopInactiveProductsAsync(int shopId, int pageNumber, int pageSize);
        Task<PagedResult<ProductListItemDto>> GetAllActiveProductsAsync(int pageNumber, int pageSize);
        Task<ApprovedProductDto> ApproveProductAsync(int productId);
        Task<RejectedProductDto> RejectProductAsync(ProductToReject dto);

    }
}
