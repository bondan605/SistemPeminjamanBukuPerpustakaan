using LibraryManagementSystem.Shared.DTOs.Admin;

namespace LibraryManagementSystem.Application.Interfaces.Repositories;

public interface IBorrowRepository
{
    Task<(IEnumerable<BorrowRequestDto> Items, int TotalCount)> GetPagedBorrowRequestsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        string? status,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken = default);
}