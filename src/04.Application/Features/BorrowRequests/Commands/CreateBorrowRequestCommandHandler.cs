using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.BorrowRequests.Commands;

public class CreateBorrowRequestCommand : IRequest<ApiResponse<int>>
{
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public string? Notes { get; set; }
}

public class CreateBorrowRequestCommandHandler : IRequestHandler<CreateBorrowRequestCommand, ApiResponse<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBorrowRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<int>> Handle(CreateBorrowRequestCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.BookId);
        if (book == null) return ApiResponse<int>.Fail("Buku tidak ditemukan.");

        if (book.Stock <= 0) return ApiResponse<int>.Fail("Stok buku sedang kosong.");

        var borrowRequest = new BorrowRequest
        {
            BookId = request.BookId,
            UserId = request.UserId,
            BorrowDate = request.BorrowDate,
            ReturnDate = request.ReturnDate,
            Notes = request.Notes,
            Status = RequestStatus.PENDING
        };

        await _unitOfWork.BorrowRequests.AddAsync(borrowRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<int>.Success(borrowRequest.Id, "Pengajuan peminjaman berhasil dibuat.");
    }
}