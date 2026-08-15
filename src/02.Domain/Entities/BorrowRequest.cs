using LibraryManagementSystem.Base;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Domain.Entities;
public class BorrowRequest : BaseEntity
{
    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.PENDING;
    public string? Notes { get; set; }

    public int? ApprovedById { get; set; }
    public User? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }
}