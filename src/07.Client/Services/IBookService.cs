using LibraryManagementSystem.Shared.DTOs.Books;
using LibraryManagementSystem.Shared.Wrappers;
using Microsoft.AspNetCore.Components.Forms;
namespace LibraryManagementSystem.Client.Services;

public interface IBookService
{
    Task<PagedResult<BookResponseDto>?> GetPagedBooksAsync(int pageNumber, int pageSize, string? searchTerm);
    Task<ApiResponse<BookResponseDto>?> GetBookDetailAsync(int id);
    Task<ApiResponse<int>> CreateBookAsync(CreateBookRequest request, IBrowserFile? imageFile);
    Task<ApiResponse<bool>> UpdateBookAsync(UpdateBookDto request, IBrowserFile? imageFile);
}