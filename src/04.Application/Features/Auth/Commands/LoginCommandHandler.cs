using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Shared.DTOs.Auth;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);

        if (user == null)
        {
            return ApiResponse<LoginResponseDto>.Fail("Email atau Password salah.");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

        if (!isPasswordValid)
        {
            return ApiResponse<LoginResponseDto>.Fail("Email atau Password salah.");
        }

        var token = _jwtService.GenerateToken(user);

        var response = new LoginResponseDto
        {
            Token = token,
            UserId = user.Id,
            Name = user.Name,
            Role = user.Role.ToString()
        };

        return ApiResponse<LoginResponseDto>.Success(response, "Login berhasil.");
    }
}