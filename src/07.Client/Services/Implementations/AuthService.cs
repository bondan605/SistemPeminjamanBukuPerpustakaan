using System.Net.Http.Json;
using Blazored.LocalStorage;
using LibraryManagementSystem.Shared.DTOs.Auth;
using LibraryManagementSystem.Shared.Wrappers;
using Microsoft.AspNetCore.Components.Authorization;
using LibraryManagementSystem.Client.Providers;

namespace LibraryManagementSystem.Client.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);

        
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();
        System.Diagnostics.Debug.WriteLine($"IsSuccess: {result?.IsSuccess}, Token: {result?.Data?.Token}");

        if (result != null && result.IsSuccess && result.Data != null)
        {
            await _localStorage.SetItemAsync("authToken", result.Data.Token);

            await _localStorage.SetItemAsync("userName", result.Data.Name);
            await _localStorage.SetItemAsync("userId", result.Data.UserId);

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Data.Token);

            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Data.Token);
        }

        return result ?? ApiResponse<LoginResponseDto>.Fail("Gagal terhubung ke server.");
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("userName");
        await _localStorage.RemoveItemAsync("userId");

        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        _http.DefaultRequestHeaders.Authorization = null;
    }
}