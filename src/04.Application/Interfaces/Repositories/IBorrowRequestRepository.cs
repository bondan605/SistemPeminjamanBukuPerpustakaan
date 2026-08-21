using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Shared.Wrappers;

namespace LibraryManagementSystem.Application.Interfaces.Repositories;
public interface IBorrowRequestRepository : IGenericRepository<BorrowRequest>
{
    Task<IEnumerable<BorrowRequest>> GetUserRequestsAsync(int userId);
    Task<(IEnumerable<BorrowRequest> Data, int Total)> GetAllPagedAsync(PagedRequest request, string? status);
    Task<int> GetPendingCountAsync();
    Task<int> GetActiveBorrowCountAsync();
    Task<IEnumerable<BorrowRequest>> GetAllWithDetailsAsync();
}
