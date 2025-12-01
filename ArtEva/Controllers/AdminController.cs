using ArtEva.DTOs.Shop;
using ArtEva.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtEva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IShopService _shopService;

        public AdminController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet("shops/pending")]
        public async Task<IActionResult> GetPendingShops()
        {
            try
            {
                var shops = await _shopService.GetPendingShopsAsync();
                return Ok(shops);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("shops/{shopId}/approve")]
        public async Task<IActionResult> ApproveShop(int shopId)
        {
            try
            {
                var shop = await _shopService.ApproveShopAsync(shopId);
                return Ok(new 
                { 
                    message = "Shop approved successfully",
                    shop 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("shops/{shopId}/reject")]
        public async Task<IActionResult> RejectShop(int shopId, [FromBody] RejectShopDto dto)
        {
            try
            {
                var shop = await _shopService.RejectShopAsync(shopId, dto);
                return Ok(new 
                { 
                    message = "Shop rejected successfully",
                    shop 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
