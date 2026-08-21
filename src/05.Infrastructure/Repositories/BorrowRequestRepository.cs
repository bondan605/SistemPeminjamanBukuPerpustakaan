using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Shared.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories;

public class BorrowRequestRepository : GenericRepository<BorrowRequest>, IBorrowRequestRepository
{
    public BorrowRequestRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BorrowRequest>> GetUserRequestsAsync(int userId)
    {
        return await _context.BorrowRequests
            .Include(x => x.Book)
            .Include(x => x.User)
            .Include(x => x.ApprovedBy)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.BorrowDate)
            .ToListAsync();
    }

    public async Task<(IEnumerable<BorrowRequest> Data, int Total)> GetAllPagedAsync(PagedRequest request, string? status)
    {
        var query = _context.BorrowRequests
            .Include(x => x.Book)
            .Include(x => x.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            if (Enum.TryParse<RequestStatus>(status, true, out var parsedStatus))
            {
                query = query.Where(x => x.Status == parsedStatus);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(x => x.User!.Name.ToLower().Contains(search) ||
                                     x.Book!.Title.ToLower().Contains(search));
        }

        int total = await query.CountAsync();

        var data = await query
            .OrderByDescending(x => x.BorrowDate)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (data, total);
    }

    public async Task<int> GetPendingCountAsync()
    {
        return await _context.BorrowRequests.CountAsync(x => x.Status == RequestStatus.PENDING);
    }

    public async Task<int> GetActiveBorrowCountAsync()
    {
        return await _context.BorrowRequests.CountAsync(x => x.Status == RequestStatus.APPROVED);
    }

    public async Task<IEnumerable<BorrowRequest>> GetAllWithDetailsAsync()
    {
        return await _context.BorrowRequests
            .Include(x => x.User)
            .Include(x => x.Book)
            .Include(x => x.ApprovedBy)
            .OrderByDescending(x => x.BorrowDate)
            .ToListAsync();
    }
}