using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Domain.Enums;
using LibraryManagementSystem.Shared.DTOs.Admin;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Admin.Queries;

public class GetDashboardAdminQuery : IRequest<ApiResponse<DashboardStatsDto>>
{

}

public class GetDashboardAdminQueryHandler : IRequestHandler<GetDashboardAdminQuery, ApiResponse<DashboardStatsDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDashboardAdminQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<DashboardStatsDto>> Handle(GetDashboardAdminQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var totalJudul = await _unitOfWork.Books.CountAsync(cancellationToken);
            var totalBooks = await _unitOfWork.Books.GetTotalStockAsync(cancellationToken);

            var totalBorrowed = await _unitOfWork.BorrowRequests
                .CountAsync(r => r.Status == RequestStatus.APPROVED, cancellationToken);

            var totalUsers = await _unitOfWork.Users
                .CountAsync(u => u.Role == UserRole.Peminjam, cancellationToken);

            var pendingRequests = await _unitOfWork.BorrowRequests
                .CountAsync(r => r.Status == RequestStatus.PENDING, cancellationToken);

            var stats = new DashboardStatsDto
            {
                TotalBooks = totalBooks + totalBorrowed,
                TotalJudul = totalJudul,
                TotalBorrowedBooks = totalBorrowed,
                TotalUsers = totalUsers,
                TotalActiveBorrows = await _unitOfWork.BorrowRequests.GetActiveBorrowCountAsync(),
                PendingRequests = pendingRequests
            };

            return new ApiResponse<DashboardStatsDto>
            {
                IsSuccess = true,
                Data = stats,
                Message = "Success memuat data statistik"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<DashboardStatsDto>
            {
                IsSuccess = false,
                Message = "Gagal mengambil data: " + ex.Message
            };
        }
    }
}