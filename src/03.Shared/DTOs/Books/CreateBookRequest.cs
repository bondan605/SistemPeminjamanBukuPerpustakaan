using System.ComponentModel.DataAnnotations;
namespace LibraryManagementSystem.Shared.DTOs.Books;
public class CreateBookRequest
{
    [Required(ErrorMessage = "Judul buku wajib diisi.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nama penulis wajib diisi.")]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategori wajib diisi.")]
    public string Category { get; set; } = string.Empty;

    public string Isbn { get; set; } = string.Empty;

    public int PublishedYear { get; set; }

    public string? Description { get; set; }

    [Range(1, 1000, ErrorMessage = "Stok minimal 1.")]
    public int Stock { get; set; }
}