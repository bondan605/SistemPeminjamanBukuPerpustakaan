using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage; 
using LibraryManagementSystem.Application.Features.Admin.Commands;
using LibraryManagementSystem.Shared.DTOs.Admin;
using LibraryManagementSystem.Shared.Wrappers;

namespace LibraryManagementSystem.Client.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public AdminService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task SetAuthorizationHeader()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<PagedResult<BorrowRequestDto>> GetPagedBorrowRequestsAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        await SetAuthorizationHeader();

        var url = $"api/admin/borrow-requests?pageNumber={pageNumber}&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            url += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
        }
        if (!string.IsNullOrWhiteSpace(status))
        {
            url += $"&status={Uri.EscapeDataString(status)}";
        }
        if (startDate.HasValue)
        {
            url += $"&startDate={startDate.Value:yyyy-MM-dd}";
        }
        if (endDate.HasValue)
        {
            url += $"&endDate={endDate.Value:yyyy-MM-dd}";
        }

        var result = await _http.GetFromJsonAsync<PagedResult<BorrowRequestDto>>(url);
        return result ?? new PagedResult<BorrowRequestDto>();
    }

    public async Task<ApiResponse<DashboardStatsDto>?> GetDashboardStatsAsync()
    {
        await SetAuthorizationHeader();
        return await _http.GetFromJsonAsync<ApiResponse<DashboardStatsDto>>("api/admin/stats");
    }

    public async Task<ApiResponse<bool>> ApproveRequestAsync(ApproveBorrowRequestCommand command)
    {
        await SetAuthorizationHeader();
        var response = await _http.PutAsJsonAsync("api/admin/approval", command);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
        return result ?? ApiResponse<bool>.Fail("Gagal memproses persetujuan.");
    }

    public async Task<byte[]> ExportExcelAsync()
    {
        await SetAuthorizationHeader();
        return await _http.GetByteArrayAsync("api/admin/export-excel");
    }
}