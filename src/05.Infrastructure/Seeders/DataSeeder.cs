using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;
using LibraryManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.Users.AnyAsync())
        {
            var adminPasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            var userPasswordHash = BCrypt.Net.BCrypt.HashPassword("John@123");

            var users = new List<User>
            {
                new User { Name = "Admin", Email = "admin@perpus.com", Password = adminPasswordHash, Role = UserRole.Admin },
                new User { Name = "John", Email = "john@gmail.com", Password = userPasswordHash, Role = UserRole.Peminjam }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        if (!await context.Books.AnyAsync())
        {
            var books = new List<Book>
            {
                new Book { Title = "Clean Code", Author = "Robert C. Martin", Category = "Software Engineering", Isbn = "978-0132350884", PublishedYear = 2008, Description = "Buku wajib programmer.", Stock = 3 },
                new Book { Title = "C# in Depth", Author = "Jon Skeet", Category = "Programming", Isbn = "978-1617294532", PublishedYear = 2019, Description = "Belajar C# mendalam.", Stock = 5 }
            };

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();
        }
    }
}