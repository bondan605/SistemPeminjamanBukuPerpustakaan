using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Shared.DTOs.Admin;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories;

public class BorrowRepository : IBorrowRepository
{
    private readonly AppDbContext _context;

    public BorrowRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<BorrowRequestDto> Items, int TotalCount)> GetPagedBorrowRequestsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        string? status,
        DateTime? startDate,
        DateTime? endDate,
        CancellationToken cancellationToken = default)
    {
        var query = _context.BorrowRequests
            .Include(x => x.User)
            .Include(x => x.Book)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(x => x.User!.Name.ToLower().Contains(term) ||
                                     x.Book!.Title.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x => x.Status.ToString().ToLower() == status.ToLower());
        }

        if (startDate.HasValue)
        {
            query = query.Where(x => x.BorrowDate >= startDate.Value);
        }
        if (endDate.HasValue)
        {
            query = query.Where(x => x.BorrowDate <= endDate.Value);
        }

        int totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.BorrowDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new BorrowRequestDto
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.User!.Name,
                BookId = x.BookId,
                BookTitle = x.Book!.Title,
                RequestDate = x.BorrowDate,
                Status = x.Status.ToString()
            })
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}