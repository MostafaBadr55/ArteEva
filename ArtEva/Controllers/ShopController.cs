using ArtEva.DTOs.Shop;
using ArtEva.Services;
using ArtEva.Services.Interfaces;
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
                
                if (shop == null)
                {
                    return NotFound(new { message = "No shop found for this user" });
                }

                return Ok(shop);
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
        [HttpPut]
        public async Task<IActionResult> UpdateShop(UpdateShopDto updateShopDto) 
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int UserID = int.Parse(userIdClaim);
            
              await _shopService.UpdateShopAsync(UserID, updateShopDto);
            
            return Ok(new 
            {
                message = "Shop updated successfully",
            });
        }


    }
}
