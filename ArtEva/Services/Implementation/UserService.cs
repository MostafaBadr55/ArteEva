using ArteEva.Data;
using ArtEva.DTOs.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ArteEva.Models;
using UserRoleEnum = ArtEva.DTOs.User.UserRole;
using ArtEva.Services.Interfaces;

namespace ArtEva.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<SelectRoleResponseDto> SelectRoleAsync(int userId, SelectRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Determine the role name
            var roleName = dto.Role == UserRoleEnum.Seller ? "Seller" : "Buyer";

            // Get current roles
            var existingRoles = await _userManager.GetRolesAsync(user);

            // Check if user already has this role
            if (existingRoles.Contains(roleName))
            {
                throw new Exception($"User already has the {roleName} role");
            }

            // Add the new role (without removing existing roles)
            var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);

            if (!addRoleResult.Succeeded)
            {
                throw new Exception($"Failed to add {roleName} role");
            }

            // Update IsSeller flag if adding Seller role
            if (dto.Role == UserRoleEnum.Seller)
            {
                user.IsSeller = true;
                user.UpdatedAt = DateTime.UtcNow;
            }

            // Invalidate existing JWTs by rotating the security stamp
            await _userManager.UpdateSecurityStampAsync(user);

            // Get updated roles after addition
            var currentRoles = await _userManager.GetRolesAsync(user);
            bool isBuyer = currentRoles.Contains("Buyer");
            bool isSeller = currentRoles.Contains("Seller");

            // Determine redirect path based on the newly added role
            string redirectTo = dto.Role == UserRoleEnum.Buyer ? "/home" : "/create-shop";

            // Return response with role information
            return new SelectRoleResponseDto
            {
                Message = $"{roleName} role added successfully",
                Role = roleName,
                RedirectTo = redirectTo,
                RequiresShopCreation = dto.Role == UserRoleEnum.Seller && !isBuyer
            };
        }

        public async Task<UserRolesDto> GetUserRolesAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserRolesDto
            {
                IsBuyer = roles.Contains("Buyer"),
                IsSeller = roles.Contains("Seller"),
                Roles = roles.ToList()
            };
        }
    }
}
