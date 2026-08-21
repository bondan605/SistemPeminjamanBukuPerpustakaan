using System.Security.Claims;
using LibraryManagementSystem.Application.Features.Admin.Commands;
using LibraryManagementSystem.Application.Features.Admin.Queries;
using LibraryManagementSystem.Shared.DTOs.Admin;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("borrow-requests")]
    public async Task<ActionResult<PagedResult<BorrowRequestDto>>> GetPagedBorrowRequests(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? status = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPagedBorrowRequestsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            Status = status,
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Mengambil data statistik untuk dashboard Admin.
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(ApiResponse<DashboardStatsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetDashboardStats()
    {
        var query = new GetDashboardAdminQuery();
        var response = await _mediator.Send(query);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return StatusCode(500, response);
    }

    /// <summary>
    /// Menyetujui atau menolak request peminjaman.
    /// </summary>
    [HttpPut("approval")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ApproveRequest([FromBody] ApproveBorrowRequestCommand command)
    {
        try
        {
            var adminIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(adminIdString)) return Unauthorized(ApiResponse<object>.Fail("Token tidak valid."));

            command.AdminId = int.Parse(adminIdString);

            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
            {
                return BadRequest(response); 
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Fail("Terjadi kesalahan server saat memproses persetujuan.", new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Mengunduh laporan peminjaman dalam format Excel (.xlsx).
    /// </summary>
    [HttpGet("export-excel")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExportExcel()
    {
        try
        {
            var fileBytes = await _mediator.Send(new ExportBorrowRequestsQuery());

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = $"Laporan_Peminjaman_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Fail("Terjadi kesalahan server saat mengunduh file Excel.", new List<string> { ex.Message }));
        }
    }
}