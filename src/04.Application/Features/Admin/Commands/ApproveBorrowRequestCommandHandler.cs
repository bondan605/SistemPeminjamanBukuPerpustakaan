using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Enums;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Admin.Commands;

public class ApproveBorrowRequestCommand : IRequest<ApiResponse<bool>>
{
    public int RequestId { get; set; }
    public bool IsApproved { get; set; }
    public int AdminId { get; set; }
}

public class ApproveBorrowRequestCommandHandler : IRequestHandler<ApproveBorrowRequestCommand, ApiResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ApproveBorrowRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<bool>> Handle(ApproveBorrowRequestCommand request, CancellationToken cancellationToken)
    {
        var borrowRequest = await _unitOfWork.BorrowRequests.GetByIdAsync(request.RequestId);
        if (borrowRequest == null) return ApiResponse<bool>.Fail("Data request tidak ditemukan.");

        if (borrowRequest.Status != RequestStatus.PENDING)
            return ApiResponse<bool>.Fail("Hanya request PENDING yang bisa diproses.");

        var book = await _unitOfWork.Books.GetByIdAsync(borrowRequest.BookId);
        if (book == null) return ApiResponse<bool>.Fail("Buku tidak ditemukan.");

        if (request.IsApproved)
        {
            if (book.Stock <= 0) return ApiResponse<bool>.Fail("Stok buku habis, tidak dapat di-approve.");

            borrowRequest.Status = RequestStatus.APPROVED;
            book.Stock -= 1; 
            _unitOfWork.Books.Update(book);
        }
        else
        {
            borrowRequest.Status = RequestStatus.REJECTED;
        }

        borrowRequest.ApprovedById = request.AdminId;
        borrowRequest.ApprovedAt = DateTime.UtcNow;

        _unitOfWork.BorrowRequests.Update(borrowRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        string action = request.IsApproved ? "disetujui" : "ditolak";
        return ApiResponse<bool>.Success(true, $"Request berhasil {action}.");
    }
}