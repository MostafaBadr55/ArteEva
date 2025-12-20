using ArtEva.DTOs;

namespace ArtEva.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequestDTO request);
        Task<AuthResponse> LoginAsync(LoginRequestDTO request);
        Task<bool> UserExistsAsync(string email);
    }
}
