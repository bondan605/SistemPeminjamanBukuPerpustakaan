using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Infrastructure.Data;

namespace LibraryManagementSystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IBorrowRepository BorrowRepository { get; private set; }
    public IUserRepository Users { get; private set; }
    public IBookRepository Books { get; private set; }
    public IBorrowRequestRepository BorrowRequests { get; private set; }

    public UnitOfWork(AppDbContext context, IUserRepository userRepository, IBookRepository bookRepository, IBorrowRequestRepository borrowRequestRepository, IBorrowRepository borrowRepository)
    {
        _context = context;
        Users = userRepository;
        Books = bookRepository;
        BorrowRequests = borrowRequestRepository;
        BorrowRepository = borrowRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}