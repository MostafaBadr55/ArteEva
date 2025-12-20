<<<<<<< HEAD
﻿using ArtEva.DTOs.Category;
using ArtEva.Services.Interfaces;
=======
﻿using ArteEva.Models;
using ArtEva.DTOs.Category;
using ArtEva.Extensions;
using ArtEva.Services;
>>>>>>> 7ef7d5956491c35f60b9324084ee1e37d86f8eee
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtEva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =("Admin,SuperAdmin"))]
     
    public class CategoryController : ControllerBase
    {
        public ICategoryService CategoryService { get; }
        public CategoryController( ICategoryService categoryService )
        {
            CategoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await CategoryService.GetAllCategoriesAsync();

            foreach (var category in categories)
            {
                category.ImageUrl = Request.BuildPublicUrl(category.ImageUrl);
            }

            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await CategoryService.GetCategoryByIdAsync(id);
           
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            category.ImageUrl = Request.BuildPublicUrl(category.ImageUrl);
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            try
            {
                var category = await CategoryService.CreateCategoryAsync(request);
                return Ok(new
                {
                    message = "Category created successfully",
                    category
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
             
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequestDto request)
        {
            try
            {
                var category = await CategoryService.UpdateCategoryAsync(request);
                return Ok(new
                {
                    message = "Category updated successfully",
                    category
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await CategoryService.DeleteCategoryAsync(id);
                return Ok(new { message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
