using ArteEva.Models;
using ArtEva.DTOs.Category;
using ArtEva.DTOs.subCategory;

namespace ArtEva.Services.Interfaces
{
    public interface ISubCategoryService
    {
        Task<SubCategoryDTO> GetSubCategoryByIdAsync(int id);
        Task<IEnumerable<SubCategoryDTO>> GetAllSubCategoriesAsync();
        Task<SubCategoryDTO> CreateSubCategoryAsync(CreateSubCategory req);
        Task<SubCategoryDTO> UpdateSubCategoryAsync(UpdateSubcategory req);
        Task DeleteSubCategoryAsync(int id);

    }
}
