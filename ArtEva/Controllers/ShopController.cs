using ArtEva.DTOs.Shop;
using ArtEva.DTOs.Shop.Mappings;
using ArtEva.Extensions;
using ArtEva.Models.Enums;
using ArtEva.Services;
using ArtEva.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArtEva.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        private readonly IShopProductService _shopProductService;

        public ShopController(IShopService shopService, IShopProductService shopProductService)
        {
            _shopService = shopService;
            _shopProductService = shopProductService;
        }

        [HttpPost]
        [Authorize(Roles = "Seller,Admin")]
        public async Task<IActionResult> CreateShop([FromBody] CreateShopDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            await _shopService.CreateShopAsync(userId, dto);

            return Ok(new
            {
                message = "Shop created successfully and sent for admin approval"
            });
        }



        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetMyShop(int pageNumber, int pageSize)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var shop = await _shopProductService
                .GetShopByOwnerIdAsync(userId, pageNumber, pageSize);

            // Shop image
            shop.ImageUrl = Request.BuildPublicUrl(shop.ImageUrl);

            // Product images (Active & Inactive)
            Request.BuildProductImagesUrls(shop.activeProductDtos);
            Request.BuildProductImagesUrls(shop.inActiveProductDtos);

            var productShopViewModel = ShopMappingExtensions.ToViewModel(shop);

            return Ok(productShopViewModel);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetShopById(int id)
        {
            var shop = await _shopService.GetShopByIdAsync(id);

            shop.ImageUrl = Request.BuildPublicUrl(shop.ImageUrl);

            return Ok(shop);
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
        public async Task<IActionResult> UpdateShopStatus(int shopId, [FromBody] UpdateShopStatusDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            int userId = int.Parse(userIdClaim);
            await _shopService.UpdateShopStatusBySellerAsync(userId, shopId, dto.NewStatus);
            return NoContent();
        }
     


    }
}
