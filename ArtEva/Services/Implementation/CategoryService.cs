using ArteEva.Data;
using ArteEva.Models;
using ArteEva.Repositories;
using ArtEva.DTOs.Category;
using ArtEva.Services.Implementation;
using Azure.Core;

namespace ArtEva.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _config;

        public CategoryService(ICategoryRepository categoryRepository , ApplicationDbContext context, IConfiguration config) 
        {
            _categoryRepository = categoryRepository;
            _config = config;


        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto request)
        {
            var baseUrl = _config["UploadSettings:BaseUrl"];

            var existingCategory = await _categoryRepository.FirstOrDefaultAsync(c=>c.Name==request.Name);
            if(existingCategory != null)
            {
                throw new Exception("Category with the same name already exists");
            }
            Category category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageUrl = $"{baseUrl}/uploads/shops/{request.ImageUrl}",

            };
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChanges();

            return new CategoryDto
            {
                ID = existingCategory.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
            };  
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");

            }
            await _categoryRepository.Delete(id);
            await _categoryRepository.SaveChanges();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var res=   _categoryRepository.GetAllAsync();
            return res.Select(c => new CategoryDto
            {
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
            }).ToList();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
                return null;


            return new CategoryDto
            {
                ID=existingCategory.Id,
                Name = existingCategory.Name,
                Description = existingCategory.Description,
                ImageUrl = existingCategory.ImageUrl,
            };
              
        }

        public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryRequestDto request)
        {

            var existingCategory = await _categoryRepository.GetByIdAsync(request.Id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }
            existingCategory.Name = request.Name;
            existingCategory.Description = request.Description; 
            existingCategory.ImageUrl = request.ImageUrl;
            existingCategory.UpdatedAt = DateTime.UtcNow;   
           await _categoryRepository.UpdateAsync(existingCategory); 
            await _categoryRepository.SaveChanges();
            return new CategoryDto
            {
                Name = existingCategory.Name,
                Description = existingCategory.Description,
                ImageUrl = existingCategory.ImageUrl,
            };
        }

        //added
        public async Task<bool> ValidateCategoryExistsAsync(int categoryId)
        {
            bool exists = await _categoryRepository.AnyAsync(c =>
                c.Id == categoryId && !c.IsDeleted);
            return exists;
        }

    }
}
