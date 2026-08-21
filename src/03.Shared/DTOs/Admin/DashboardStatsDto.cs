namespace LibraryManagementSystem.Shared.DTOs.Admin;

public class DashboardStatsDto
{
    public int TotalBooks { get; set; }
    public int TotalUsers { get; set; }
    public int TotalActiveBorrows { get; set; }
    public int PendingRequests { get; set; }
    public int TotalJudul { get; set; }
    public int TotalBorrowedBooks { get; set; }
}