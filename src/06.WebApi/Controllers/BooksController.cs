using LibraryManagementSystem.Application.Features.Books.Commands;
using LibraryManagementSystem.Application.Features.Books.Queries;
using LibraryManagementSystem.Shared.DTOs.Books;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _env;

    public BooksController(IMediator mediator, IWebHostEnvironment env)
    {
        _mediator = mediator;
        _env = env;
    }

    /// <summary>
    /// Menampilkan daftar buku dengan fitur pencarian dan pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<BookResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBooks([FromQuery] GetPagedBooksQuery query)
    {
        try
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Fail("Terjadi kesalahan server saat mengambil data buku.", new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Menampilkan detail informasi satu buku berdasarkan ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BookResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBookDetail(int id)
    {
        try
        {
            var response = await _mediator.Send(new GetBookDetailQuery { Id = id });

            if (!response.IsSuccess)
            {
                return NotFound(response); 
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Fail("Terjadi kesalahan server saat mengambil detail buku.", new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Menambahkan buku baru beserta cover gambar (Khusus Admin).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBook([FromForm] CreateBookRequest request)
    {
        try
        {
            string? imageUrl = null;

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(request.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(ApiResponse<object>.Fail("Format gambar tidak valid. Hanya menerima file dengan format .png, .jpg, atau .jpeg."));
                }

                var maxFileSizeInBytes = 2 * 1024 * 1024;
                if (request.ImageFile.Length > maxFileSizeInBytes)
                {
                    return BadRequest(ApiResponse<object>.Fail("Ukuran gambar terlalu besar. Maksimal 2MB."));
                }

                string uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "books");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + fileExtension; 
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(fileStream);
                }

                imageUrl = $"/images/books/{uniqueFileName}";
            }

            var command = new CreateBookCommand
            {
                Title = request.Title,
                Author = request.Author,
                Category = request.Category,
                Isbn = request.Isbn,
                PublishedYear = request.PublishedYear,
                Description = request.Description,
                Stock = request.Stock,
                ImageUrl = imageUrl
            };
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Fail("Gagal menambahkan buku.", new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Memperbarui data buku beserta cover gambar (Khusus Admin).
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBook(int id, [FromForm] UpdateBookRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest(ApiResponse<object>.Fail("ID buku tidak sesuai."));
        }

        try
        {
            string? newImageUrl = null;

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(request.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(ApiResponse<object>.Fail("Format gambar tidak valid. Gunakan .png, .jpg, atau .jpeg."));
                }

                var maxFileSizeInBytes = 2 * 1024 * 1024;
                if (request.ImageFile.Length > maxFileSizeInBytes)
                {
                    return BadRequest(ApiResponse<object>.Fail("Ukuran gambar terlalu besar. Maksimal 2MB."));
                }

                string uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "books");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(fileStream);
                }

                newImageUrl = $"/images/books/{uniqueFileName}";
            }

            var command = new UpdateBookCommand
            {
                Id = request.Id,
                Title = request.Title,
                Author = request.Author,
                Category = request.Category,
                Isbn = request.Isbn,
                PublishedYear = request.PublishedYear,
                Description = request.Description,
                Stock = request.Stock,
                ImageUrl = newImageUrl 
            };

            var response = await _mediator.Send(command);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Fail("Gagal memperbarui buku.", new List<string> { ex.Message }));
        }
    }
    public class CreateBookRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }

        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateBookRequest
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}