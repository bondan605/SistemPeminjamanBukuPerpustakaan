using LibraryManagementSystem.Client.Providers;
using LibraryManagementSystem.Client.Services;
using LibraryManagementSystem.Client.Services.Implementations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClientServices(this IServiceCollection services, string backendBaseUrl)
    {
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(backendBaseUrl) });

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBorrowRequestService, BorrowRequestService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        return services;
    }
}