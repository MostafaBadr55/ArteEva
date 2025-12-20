using ArtEva.DTOs.Category;

namespace ArtEva.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto request);
        Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryRequestDto request);
        Task DeleteCategoryAsync(int id);
        Task<bool> ValidateCategoryExistsAsync(int categoryId);
    }
}
