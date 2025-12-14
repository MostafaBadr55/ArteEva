using ArtEva.DTOs.subCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtEva.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly Services.Interfaces.ISubCategoryService _subCategoryService;
        public SubCategoriesController(Services.Interfaces.ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubCategoryById(int id)
        {
            var subCategory = await _subCategoryService.GetSubCategoryByIdAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            return Ok(subCategory);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSubCategories()
        {
            var subCategories = await _subCategoryService.GetAllSubCategoriesAsync();
            return Ok(subCategories);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubCategory req)
        {
            var subCategory = await _subCategoryService.CreateSubCategoryAsync(req);
            if (subCategory == null)
            {
                return BadRequest();
            }
            return Ok(subCategory);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateSubCategory([FromBody] UpdateSubcategory req)
        {
            var subCategory = await _subCategoryService.UpdateSubCategoryAsync(req);
            if (subCategory == null)
            {
                return BadRequest();
            }
            return Ok(subCategory);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            await _subCategoryService.DeleteSubCategoryAsync(id);
            return NoContent();
        }
    }
}
