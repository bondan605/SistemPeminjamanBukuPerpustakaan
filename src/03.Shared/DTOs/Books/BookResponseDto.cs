namespace LibraryManagementSystem.Shared.DTOs.Books;
public class BookResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Isbn { get; set; }
    public int PublishedYear { get; set; }
    public string? Description { get; set; }
    public int Stock { get; set; }
    public string StockStatus { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}