using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Data;
using LibraryManagementSystem.Shared.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{

    public BookRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<(IEnumerable<Book> Data, int Total)> GetPagedBooksAsync(PagedRequest request)
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(b => b.Title.ToLower().Contains(search) ||
                                     b.Author.ToLower().Contains(search) ||
                                     b.Category.ToLower().Contains(search));
        }

        int total = await query.CountAsync();

        var data = await query
            .OrderByDescending(b => b.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (data, total);
    }
    public async Task<int> GetTotalStockAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Books.SumAsync(b => b.Stock, cancellationToken);
    }
}