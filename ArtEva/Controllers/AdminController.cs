using ArteEva.Models;
using ArtEva.Application.Products.Quiries;
using ArtEva.Application.Shops.Quiries;
using ArtEva.DTOs.Admin;
using ArtEva.DTOs.Product;
using ArtEva.DTOs.Shop;
using ArtEva.Extensions;
using ArtEva.Models.Enums;
using ArtEva.Services;
using ArtEva.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtEva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IAdminService _adminService;
        private readonly IProductService _productService;

        public AdminController(IShopService shopService, IAdminService adminService, IProductService productService)
        {
            _shopService = shopService;
            _adminService = adminService;
            _productService = productService;
        }

        [HttpGet("GetShops")]
  
        public async Task<IActionResult> GetShops(
                [FromQuery] ShopQueryCriteria criteria,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 20)
        {
            var result = await _shopService.GetShopsAsync(
                criteria,
                pageNumber,
                pageSize);

            foreach (var shop in result.Items)
            {
                shop.ImageUrl = Request.BuildPublicUrl(shop.ImageUrl);
            }

            return Ok(result);
        }


        [HttpGet("shops/pending")]
        public async Task<IActionResult> GetPendingShops()
        {
            var shops = await _shopService.GetPendingShopsAsync();
            foreach (var shop in shops)
            {
                if (shop.ImageUrl == null) continue;
                shop.ImageUrl = Request.BuildPublicUrl(shop.ImageUrl);
            }
            return Ok(shops);
        }

        [HttpPost("shops/{shopId}/approve")]
        public async Task<IActionResult> ApproveShop(int shopId)
        {
            var shop = await _shopService.ApproveShopAsync(shopId);
            return Ok(new { message = "Shop approved successfully", shop });
        }

        [HttpPost("shops/{shopId}/reject")]
        public async Task<IActionResult> RejectShop(int shopId, [FromBody] RejectShopDto dto)
        {
            var shop = await _shopService.RejectShopAsync(shopId, dto);
            return Ok(new { message = "Shop rejected successfully", shop });
        }

        [HttpPost("users/assign-role")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto request)
        {
            var result = await _adminService.AssignRoleAsync(request);
            if (!result.Success)
                return BadRequest(new { message = result.Message });
            return Ok(result);
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts
            ([FromQuery] ProductQueryCriteria criteria,int pageNumber = 1,int pageSize = 20)
        {
            var result = await _productService.GetProductsAsync(
                criteria,
                pageNumber,
                pageSize);

            return Ok(result);
        }

        [HttpPost("{productId:int}/approve")]
        public async Task<IActionResult> ApproveProduct(int productId)
        {
            var result = await _productService.ApproveProductAsync(productId);
            return Ok(result);
        }
        [HttpPost("{productId:int}/reject")]
        public async Task<IActionResult> RejectProduct(
        [FromBody] RejectRequestViewModel req)
        {
            ProductToReject dto = 
                new ProductToReject { ProductId = req.ProductId, RejectionMessage= req.RejectionMessage};
            var result = await _productService.RejectProductAsync(dto);
            return Ok(result);
        }
    }
}
