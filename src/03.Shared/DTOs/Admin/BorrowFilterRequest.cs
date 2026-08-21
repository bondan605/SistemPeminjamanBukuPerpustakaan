namespace LibraryManagementSystem.Shared.DTOs.Admin;

public class BorrowFilterRequest
{
    public string? SearchTerm { get; set; } 
    public string? Status { get; set; }     
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}