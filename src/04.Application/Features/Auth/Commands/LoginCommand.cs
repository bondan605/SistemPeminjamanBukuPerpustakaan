using LibraryManagementSystem.Shared.DTOs.Auth;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<ApiResponse<LoginResponseDto>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}