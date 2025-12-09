using ArtEva.DTOs.Admin;
using ArtEva.Services;
using ArtEva.DTOs.Shop;
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



        public AdminController(IShopService shopService, IAdminService adminService)
        {
            _shopService = shopService;
            _adminService = adminService;
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
    }
}
