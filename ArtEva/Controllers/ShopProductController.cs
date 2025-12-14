using ArtEva.DTOs.Product;
using ArtEva.Services;
using ArtEva.ViewModels.Product;
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

        [HttpPut("/Update")]
        public async Task<IActionResult> UpdateProduct(
        int shopId, int productId, [FromBody] UpdateProductDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var updated = await _productService.UpdateProductAsync(userId,dto);
            return Ok(updated);
        }
        [HttpPatch("{productId:int}/price")]
        public async Task<IActionResult> UpdatePrice(
        int shopId,
        int productId,
        [FromBody] UpdateProductPriceRequestViewModel request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _productService.UpdateProductPriceAsync(
                userId,
                shopId,
                productId,
                request.Price);

            return Ok(result);
        }
        [HttpPatch("{productId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int shopId, int productId,
                                           [FromBody] UpdateProductStatusRequestViewModel request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _productService.UpdateProductStatusAsync(userId, shopId,productId,
                                                                     request.Status);
            return Ok(result);
        }
    }
}
