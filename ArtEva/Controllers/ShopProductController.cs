using ArtEva.DTOs.Product;
using ArtEva.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtEva.Controllers
{
    [Route("api/shops/{shopId}/products")]
    [ApiController]
    [Authorize(Roles ="Seller")]
    public class ShopProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ShopProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(int shopId, [FromBody] CreateProductDto dto)
        {
            dto.ShopId = shopId;
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var product = await _productService.CreateProductAsync(userId, dto);

            return Ok(product);
        }
    }
}
