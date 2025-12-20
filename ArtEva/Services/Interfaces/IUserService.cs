using ArtEva.DTOs.User;

namespace ArtEva.Services.Interfaces
{
    public interface IUserService
    {
        Task<SelectRoleResponseDto> SelectRoleAsync(int userId, SelectRoleDto dto);
        Task<UserRolesDto> GetUserRolesAsync(int userId);
    }
}
