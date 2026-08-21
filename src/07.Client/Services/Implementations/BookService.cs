using LibraryManagementSystem.Shared.DTOs.Books;
using LibraryManagementSystem.Shared.Wrappers;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage; 

namespace LibraryManagementSystem.Client.Services.Implementations;

public class BookService : IBookService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage; 

    public BookService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    public async Task<PagedResult<BookResponseDto>?> GetPagedBooksAsync(int pageNumber, int pageSize, string? searchTerm)
    {
        string url = $"api/books?PageNumber={pageNumber}&PageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            url += $"&SearchTerm={Uri.EscapeDataString(searchTerm)}";
        }

        return await _http.GetFromJsonAsync<PagedResult<BookResponseDto>>(url);
    }

    public async Task<ApiResponse<BookResponseDto>?> GetBookDetailAsync(int id)
    {
        return await _http.GetFromJsonAsync<ApiResponse<BookResponseDto>>($"api/books/{id}");
    }

    public async Task<ApiResponse<int>> CreateBookAsync(CreateBookRequest request, IBrowserFile? imageFile)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(request.Title), "Title");
        content.Add(new StringContent(request.Author), "Author");
        content.Add(new StringContent(request.Category), "Category");
        content.Add(new StringContent(request.Isbn), "Isbn");
        content.Add(new StringContent(request.PublishedYear.ToString()), "PublishedYear");
        content.Add(new StringContent(request.Stock.ToString()), "Stock");

        if (!string.IsNullOrEmpty(request.Description))
        {
            content.Add(new StringContent(request.Description), "Description");
        }

        if (imageFile != null)
        {
            var fileContent = new StreamContent(imageFile.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
            content.Add(fileContent, "ImageFile", imageFile.Name);
        }

        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.PostAsync("api/books", content);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return ApiResponse<int>.Fail("Gagal menyimpan. Anda tidak memiliki akses (Token tidak valid atau kadaluarsa).");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();
        return result ?? ApiResponse<int>.Fail("Gagal terhubung ke server.");
    }

    public async Task<ApiResponse<bool>> UpdateBookAsync(UpdateBookDto request, IBrowserFile? imageFile)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(request.Id.ToString()), "Id");
        content.Add(new StringContent(request.Title), "Title");
        content.Add(new StringContent(request.Author), "Author");
        content.Add(new StringContent(request.Category), "Category");
        content.Add(new StringContent(request.Isbn), "Isbn");
        content.Add(new StringContent(request.PublishedYear.ToString()), "PublishedYear");
        content.Add(new StringContent(request.Stock.ToString()), "Stock");

        if (!string.IsNullOrEmpty(request.Description))
        {
            content.Add(new StringContent(request.Description), "Description");
        }

        if (imageFile != null)
        {
            var fileContent = new StreamContent(imageFile.OpenReadStream(maxAllowedSize: 2 * 1024 * 1024));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
            content.Add(fileContent, "ImageFile", imageFile.Name);
        }

        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.PutAsync($"api/books/{request.Id}", content);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return ApiResponse<bool>.Fail("Gagal update. Anda tidak memiliki akses (Token tidak valid atau kadaluarsa).");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        return result ?? ApiResponse<bool>.Fail("Gagal terhubung ke server saat update.");
    }
}