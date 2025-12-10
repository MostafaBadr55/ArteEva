using ArteEva.Repositories;
using ArtEva.DTOs.Category;
using ArtEva.DTOs.subCategory;
using ArtEva.Services.Interfaces;

namespace ArtEva.Services
{
    public class SubCategoryService : ISubCategoryService
    {
       private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryService categoryService;
        public SubCategoryService(ISubCategoryRepository subCategoryRepository, ICategoryService categoryService)
        {
            _subCategoryRepository = subCategoryRepository;
            this.categoryService = categoryService;
        }
        public async Task<SubCategoryDTO> CreateSubCategoryAsync(CreateSubCategory req)
        {
            var existingSubCategory = await _subCategoryRepository.FirstOrDefaultAsync(c => c.Name == req.Name); ;

            if (existingSubCategory == null) {
                _subCategoryRepository.AddAsync(new ArteEva.Models.SubCategory { Name = req.Name, CategoryId = req.CategoryId.Value });
 
                await _subCategoryRepository.SaveChanges();
                return new SubCategoryDTO
                {
                    Name = req.Name,

                };
            }
            throw new Exception("Category with the same name already exists");



        }

        public async Task DeleteSubCategoryAsync(int id)
        {
            var existingSubCategory =  await _subCategoryRepository.GetByIdAsync(id);
            if (existingSubCategory == null)
            {
                throw new Exception("SubCategory not found");
            }
         await _subCategoryRepository.Delete(id);
         await _subCategoryRepository.SaveChanges();



        }

        public async Task<IEnumerable<SubCategoryDTO>> GetAllSubCategoriesAsync()
        {
            var subCategories = await _subCategoryRepository.GetAllAsync();
            return subCategories.Select(c => new SubCategoryDTO
            {
                Name = c.Name,
            });
        }

        public async Task<SubCategoryDTO> GetSubCategoryByIdAsync(int id)
        {
            var subCategory =await  _subCategoryRepository.GetByIdAsync(id);
            if (subCategory == null)
            {
                throw new Exception("SubCategory not found");
            
            }
            return new SubCategoryDTO
            {
                Name = subCategory.Name,
            };
        }

        public async Task<SubCategoryDTO> UpdateSubCategoryAsync(UpdateSubcategory req)
        {          
                   
            var existingSubCategory = await _subCategoryRepository.GetByIdAsync(req.Id);
            if (existingSubCategory == null)
            {
                throw new Exception("SubCategory not found");
            }
           var category = await categoryService.GetCategoryByIdAsync(req.CategoryId.Value);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            existingSubCategory.Name = req.Name ?? existingSubCategory.Name;
            existingSubCategory.CategoryId = req.CategoryId ?? existingSubCategory.CategoryId;
            _subCategoryRepository.Update(existingSubCategory);
            await _subCategoryRepository.SaveChanges();
            return new SubCategoryDTO
            {
                Name = existingSubCategory.Name
            };

        }
    }
}
