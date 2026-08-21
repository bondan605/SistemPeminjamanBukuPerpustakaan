using System.Security.Claims;
using LibraryManagementSystem.Application.Features.BorrowRequests.Commands;
using LibraryManagementSystem.Application.Features.BorrowRequests.Queries;
using LibraryManagementSystem.Shared.DTOs.BorrowRequests;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Peminjam")]
public class BorrowRequestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BorrowRequestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Mengajukan permohonan peminjaman buku.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRequest([FromBody] CreateBorrowRequestCommand command)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized(ApiResponse<object>.Fail("Token tidak valid."));

            command.UserId = int.Parse(userIdString);

            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Fail("Terjadi kesalahan server saat mengajukan peminjaman.", new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Melihat daftar riwayat peminjaman milik user yang sedang login.
    /// </summary>
    [HttpGet("my-requests")]
    [ProducesResponseType(typeof(ApiResponse<List<BorrowRequestResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMyRequests()
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized(ApiResponse<object>.Fail("Token tidak valid."));

            var query = new GetMyBorrowRequestsQuery { UserId = int.Parse(userIdString) };
            var response = await _mediator.Send(query);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Fail("Terjadi kesalahan server saat mengambil riwayat peminjaman.", new List<string> { ex.Message }));
        }
    }
}