using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Books.Commands;

public class CreateBookCommand : IRequest<ApiResponse<int>>
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublishedYear { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
}

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, ApiResponse<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<int>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Category = request.Category,
            Isbn = request.Isbn,
            PublishedYear = request.PublishedYear,
            Description = request.Description,
            Stock = request.Stock,
            ImageUrl = request.ImageUrl
        };

        await _unitOfWork.Books.AddAsync(book);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<int>.Success(book.Id, "Buku berhasil ditambahkan.");
    }
}