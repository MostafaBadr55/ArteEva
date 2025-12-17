using ArtEva.DTOs.Shop;
using ArtEva.DTOs.Shop.Mappings;
using ArtEva.Services;
using ArtEva.Services.Interfaces;
using ArtEva.ViewModels.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Security.Claims;

namespace ArtEva.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IShopProductService _shopProductService;

        public ShopController(IShopService shopService,IShopProductService shopProductService)
        {
            _shopService = shopService;
            _shopProductService = shopProductService;
        }

        [HttpPost]
        [Authorize(Roles = "Seller,Admin")]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }
                 await _shopService.CreateShopAsync(userId, dto);
                return Ok(new 
                { 
                    message = "Shop created successfully and sent for admin approval",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet()]
 
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetMyShop(int pageNumber,int pageSize)
        {
            try
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var shop = await _shopProductService.GetShopByOwnerIdAsync(userId,pageNumber, pageSize);
              var ProductShopViewModel=  ShopMappingExtensions.ToViewModel(shop);

                if (shop == null)
                {
                    return NotFound(new { message = "No shop found for this user" });
                }

                return Ok(ProductShopViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShopById(int id)
        {
            try
            {
                var shop = await _shopService.GetShopByIdAsync(id);
                return Ok(shop);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Seller")]
        [HttpPut]
        public async Task<IActionResult> UpdateShop(UpdateShopDto updateShopDto) 
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int UserID = int.Parse(userIdClaim);
            
              await _shopService.UpdateShopInfoAsync(UserID, updateShopDto);
            
            return Ok(new 
            {
                message = "Shop updated successfully",
            });
        }
        // PATCH api/shop/{shopId}/status
        [HttpPatch("{shopId}/status")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateShopStatus(
            int shopId,
            [FromBody] UpdateShopStatusDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

             int userId = int.Parse(userIdClaim);
             await _shopService.UpdateShopStatusBySellerAsync(userId, shopId, dto.NewStatus);
            return NoContent();
        }

    }
}
