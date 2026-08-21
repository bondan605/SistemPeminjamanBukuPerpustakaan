using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using LibraryManagementSystem.Shared.DTOs.BorrowRequests;
using LibraryManagementSystem.Shared.Wrappers;

namespace LibraryManagementSystem.Client.Services.Implementations;

public class BorrowRequestService : IBorrowRequestService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public BorrowRequestService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task SetAuthorizationHeader()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<ApiResponse<int>> CreateRequestAsync(CreateBorrowRequestDto request)
    {
        await SetAuthorizationHeader(); 

        var response = await _http.PostAsJsonAsync("api/borrowrequests", request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return ApiResponse<int>.Fail("Anda tidak memiliki akses (Sesi habis, silakan login ulang).");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<int>>();
        return result ?? ApiResponse<int>.Fail("Gagal terhubung ke server.");
    }

    public async Task<ApiResponse<List<BorrowRequestResponseDto>>> GetMyRequestsAsync()
    {
        await SetAuthorizationHeader(); 

        var response = await _http.GetAsync("api/borrowrequests/my-requests");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return ApiResponse<List<BorrowRequestResponseDto>>.Fail("Anda tidak memiliki akses (Sesi habis, silakan login ulang).");
        }

        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<BorrowRequestResponseDto>>>();
        return result ?? ApiResponse<List<BorrowRequestResponseDto>>.Fail("Gagal memuat daftar pinjaman.");
    }
}