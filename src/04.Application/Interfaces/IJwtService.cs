using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces;
public interface IJwtService
{
    string GenerateToken(User user);
}