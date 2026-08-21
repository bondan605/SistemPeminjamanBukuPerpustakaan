namespace LibraryManagementSystem.Shared.DTOs.BorrowRequests;

public class BorrowRequestResponseDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BorrowerName { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? ApprovedByAdminName { get; set; }
}