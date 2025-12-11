using ArtEva.DTOs.Product;

namespace ArtEva.Services
{
    public interface IProductService
    {
        public Task<CreatedProductDto> CreateProductAsync(int userId, CreateProductDto dto);
    }
}
