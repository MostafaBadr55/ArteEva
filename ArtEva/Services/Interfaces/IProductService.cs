using ArteEva.Models;
using ArtEva.DTOs.Pagination;
using ArtEva.DTOs.Pagination.Product;
using ArtEva.DTOs.Product;
using ArtEva.DTOs.ProductImage;
using ArtEva.Models.Enums;
using System.Linq.Expressions;

namespace ArtEva.Services
{
    public interface IProductService
    {
        public Task<CreatedProductDto> CreateProductAsync( CreateProductDto dto);
        Task UpdateProductBaseInfoAsync(Product product);
        Task UpdateProductPriceInternalAsync(Product product, decimal newPrice);
        Task UpdateProductStatusInternalAsync(Product product, ProductStatus status);
        public Task<Product> GetProductByIdAsync(int productId);
        Task<Product> GetProductForUpdateAsync(int productId);

        // master dynamic paging method
        Task<PagedResult<ProductListItemDto>> GetPagedProductsAsync(
            Expression<Func<Product, bool>> filter,
            int pageNumber,
            int pageSize);

        // wrappers for explicit use cases
        Task<PagedResult<ProductListItemDto>> GetAdminPendingProductsAsync(int pageNumber, int pageSize);
        Task<PagedResult<ProductListItemDto>> GetAdminApprovedProductsAsync(int pageNumber, int pageSize);
        Task<PagedResult<ProductListItemDto>> GetAllActiveProductsAsync(int pageNumber, int pageSize);
        Task<ApprovedProductDto> ApproveProductAsync(int productId);
        Task<RejectedProductDto> RejectProductAsync(ProductToReject dto);

        Task UpdateProductImagesAsync(Product product, List<UpdateProductImage> imagesDto);

    }
}
