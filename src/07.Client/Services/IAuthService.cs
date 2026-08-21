using LibraryManagementSystem.Shared.DTOs.Auth;
using LibraryManagementSystem.Shared.Wrappers;

namespace LibraryManagementSystem.Client.Services;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}