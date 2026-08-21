using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Books.Commands;

public class UpdateBookCommand : IRequest<ApiResponse<bool>>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }

    public string? ImageUrl { get; set; }
}

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, ApiResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<bool>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.Id);
        if (book == null)
        {
            return ApiResponse<bool>.Fail("Buku tidak ditemukan.");
        }

        book.Title = request.Title;
        book.Author = request.Author;
        book.Category = request.Category;
        book.Isbn = request.Isbn;
        book.PublishedYear = request.PublishedYear;
        book.Description = request.Description;
        book.Stock = request.Stock;

        if (!string.IsNullOrEmpty(request.ImageUrl))
        {
            book.ImageUrl = request.ImageUrl;
        }

        _unitOfWork.Books.Update(book);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<bool>.Success(true, "Data buku berhasil diperbarui.");
    }
}