using LibraryManagementSystem.Shared.DTOs.BorrowRequests;
using LibraryManagementSystem.Shared.Wrappers;

namespace LibraryManagementSystem.Client.Services;

public interface IBorrowRequestService
{
    Task<ApiResponse<int>> CreateRequestAsync(CreateBorrowRequestDto request);
    Task<ApiResponse<List<BorrowRequestResponseDto>>> GetMyRequestsAsync();
}