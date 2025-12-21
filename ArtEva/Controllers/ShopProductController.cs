using ArtEva.Application.Products.Quiries;
using ArtEva.Application.ShopProduct.Quiries;
using ArtEva.DTOs.Product;
using ArtEva.Services;
using ArtEva.Services.Interfaces;
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
        private readonly IShopProductService _shopProductService;

        public ShopProductController(IShopProductService productService)
        {
            _shopProductService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(int shopId, [FromBody] CreateProductDto dto)
        {
            dto.ShopId = shopId;
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var product = await _shopProductService.CreateShopProductAsync(userId,dto);

            return Ok(product);
        }
        [HttpGet]
        public async Task<IActionResult> GetShopProducts
            (int shopId,[FromQuery]  ShopProductQueryCriteria query,int pageNumber = 1,int pageSize = 20)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _shopProductService.GetShopProductsAsync(
                userId,
                shopId,
                query,
                pageNumber,
                pageSize);

            return Ok(result);
        }

        [HttpPut("/Update")]
        public async Task<IActionResult> UpdateProduct(
        int shopId, int productId, [FromBody] UpdateProductDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var updated = await _shopProductService.UpdateShopProductAsync(userId,dto);
            return Ok(updated);
        }
        [HttpPatch("{productId:int}/price")]
        public async Task<IActionResult> UpdatePrice(
        int shopId,
        int productId,
        [FromBody] UpdateProductPriceRequestViewModel request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result = await _shopProductService.UpdateProductPriceAsync(
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

            var result = await _shopProductService.UpdateProductStatusAsync(userId, shopId,productId,
                                                                     request.Status);
            return Ok(result);
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> DeleteProduct(int shopId, int productId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _shopProductService.DeleteShopProduct(userId, shopId, productId);
            return NoContent();
        }
    }
}
