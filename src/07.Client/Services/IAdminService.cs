using LibraryManagementSystem.Application.Features.Admin.Commands;
using LibraryManagementSystem.Shared.DTOs.Admin;
using LibraryManagementSystem.Shared.Wrappers;

namespace LibraryManagementSystem.Client.Services;

public interface IAdminService
{
    Task<ApiResponse<DashboardStatsDto>?> GetDashboardStatsAsync();
    Task<PagedResult<BorrowRequestDto>> GetPagedBorrowRequestsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null);
    Task<ApiResponse<bool>> ApproveRequestAsync(ApproveBorrowRequestCommand command);
    Task<byte[]> ExportExcelAsync();
}