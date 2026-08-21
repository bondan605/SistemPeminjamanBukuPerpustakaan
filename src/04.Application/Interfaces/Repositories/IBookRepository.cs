using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Shared.Wrappers;

namespace LibraryManagementSystem.Application.Interfaces.Repositories;
public interface IBookRepository : IGenericRepository<Book>
{
    Task<(IEnumerable<Book> Data, int Total)> GetPagedBooksAsync(PagedRequest request);
    Task<int> GetTotalStockAsync(CancellationToken cancellationToken = default);
}
