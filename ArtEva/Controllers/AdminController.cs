using ArtEva.DTOs.Product;
using ArtEva.DTOs.Shop;
using ArtEva.Services;
using ArtEva.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ArtEva.Services.Interfaces;
using ArtEva.DTOs.Admin;

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

        [HttpGet("shops/pending")]
        public async Task<IActionResult> GetPendingShops()
        {
            var shops = await _shopService.GetPendingShopsAsync();
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

        [HttpGet("products/pending")]
        public async Task<IActionResult> GetPending([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _productService.GetAdminPendingProductsAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("products/approved")]
        public async Task<IActionResult> GetApproved([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _productService.GetAdminApprovedProductsAsync(page, pageSize);
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
