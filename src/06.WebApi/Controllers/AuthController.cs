using LibraryManagementSystem.Application.Features.Auth.Commands;
using LibraryManagementSystem.Shared.DTOs.Auth;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Melakukan proses autentikasi (Login) untuk Peminjam dan Admin.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password))
            {
                return BadRequest(ApiResponse<object>.Fail("Email dan Password harus diisi."));
            }

            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Fail("Terjadi kesalahan pada server.", new List<string> { ex.Message });
            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}