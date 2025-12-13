using System;
using System.Threading.Tasks;
using ArtEva.DTOs.Admin;
using ArtEva.ViewModels.Admin;
using ArteEva.Models;
using Microsoft.AspNetCore.Identity;
using ArtEva.Services.Interfaces;

namespace ArtEva.Services.Implementations
{
    
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AdminService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AssignRoleResponseDto> AssignRoleAsync(AssignRoleRequestDto request)
        {
            var response = new AssignRoleResponseDto
            {
                UserName = request.UserName,
                RoleName = request.RoleName
            };

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
            {
                response.Success = false;
                response.Message = $"Role '{request.RoleName}' does not exist";
                return response;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains(request.RoleName))
            {
                response.Success = false;
                response.Message = $"User already has role '{request.RoleName}'";
                return response;
            }

            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (!result.Succeeded)
            {
                response.Success = false;
                response.Message = "Failed to add role to user";
                return response;
            }

            if (string.Equals(request.RoleName, "Seller", StringComparison.OrdinalIgnoreCase))
            {
                user.IsSeller = true;
                user.UpdatedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            response.Success = true;
            response.Message = $"Role '{request.RoleName}' added to user '{request.UserName}' successfully";
            return response;
        }
    }
}
