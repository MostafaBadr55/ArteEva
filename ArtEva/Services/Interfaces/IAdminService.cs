using System.Threading.Tasks;
using ArtEva.DTOs.Admin;
using ArtEva.ViewModels.Admin;

namespace ArtEva.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AssignRoleResponseDto> AssignRoleAsync(AssignRoleRequestDto request);
    }
}
