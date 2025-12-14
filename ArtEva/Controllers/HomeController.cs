using ArteEva.Services;
using ArtEva.DTOs.Home;
using ArtEva.Models.Enums;
using ArtEva.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtEva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
         private readonly IProductService productService;
        private readonly ILogger<HomeController> logger;
        private readonly ICategoryService categoryService;
      public  HomeController(IProductService _productService, ICategoryService _categoryService)
        {
            productService = _productService;
            categoryService = _categoryService;
         }



        [HttpGet("{page:int}/{size:int}")]
        public async Task<IActionResult> TheHome(int page, int size)
        {
            HomePageDTO homePageDTO = new HomePageDTO();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                homePageDTO.WelcomeMessage = null;
            }
            else
            {
                homePageDTO.WelcomeMessage = User.Identity?.Name;
            }

            homePageDTO.Categories =
                (await categoryService.GetAllCategoriesAsync()).ToList();

            var pagedResult = await productService.GetPagedProductsAsync(
                c => c.ApprovalStatus == ProductApprovalStatus.Approved,
                page,
                size
            );

            homePageDTO.FeaturedProducts = pagedResult.Items.ToList();

            return Ok(homePageDTO);
        }
    }
}
