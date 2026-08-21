using LibraryManagementSystem.Application.Interfaces.Repositories;

namespace LibraryManagementSystem.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBorrowRepository BorrowRepository { get; }
    IUserRepository Users { get; }
    IBookRepository Books { get; }
    IBorrowRequestRepository BorrowRequests { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}