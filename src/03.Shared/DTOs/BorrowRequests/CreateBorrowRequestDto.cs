using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Shared.DTOs.BorrowRequests;

public class CreateBorrowRequestDto : IValidatableObject
{
    [Required(ErrorMessage = "Buku wajib dipilih.")]
    [Range(1, int.MaxValue, ErrorMessage = "ID Buku tidak valid.")]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Tanggal Pinjam wajib diisi.")]
    public DateTime? BorrowDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Tanggal Kembali wajib diisi.")]
    public DateTime? ReturnDate { get; set; } = DateTime.Today.AddDays(7);

    [MaxLength(500, ErrorMessage = "Catatan tidak boleh lebih dari 500 karakter.")]
    public string? Notes { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (BorrowDate.HasValue && ReturnDate.HasValue)
        {
            if (ReturnDate.Value <= BorrowDate.Value)
            {
                yield return new ValidationResult(
                    "Tanggal kembali harus lebih besar dari tanggal pinjam.",
                    new[] { nameof(ReturnDate) }
                );
            }

            if ((ReturnDate.Value - BorrowDate.Value).TotalDays > 14)
            {
                yield return new ValidationResult(
                   "Maksimal durasi peminjaman adalah 14 hari.",
                   new[] { nameof(ReturnDate) }
               );
            }
        }
    }
}