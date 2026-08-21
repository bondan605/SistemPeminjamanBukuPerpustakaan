using LibraryManagementSystem.Base;

namespace LibraryManagementSystem.Domain.Entities;
public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public int PublishedYear { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }

    public ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();
}