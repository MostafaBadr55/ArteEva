using ArtEva.DTOs.Product;

namespace ArtEva.Services
{
    public interface IProductService
    {
        public Task<CreatedProductDto> CreateProductAsync(int userId, CreateProductDto dto);
        public Task<CreatedProductDto> UpdateProductAsync(int userId, UpdateProductDto dto);
        public Task<ProductDetailsDto> GetProductByIdAsync(int productId);
    }
}
