using LibraryManagementSystem.Base;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Domain.Entities;
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();
}