using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Infrastructure.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Author).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Category).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Isbn).HasMaxLength(20);

        builder.ToTable(t => t.HasCheckConstraint("CK_Book_Stock", "[Stock] >= 0"));
    }
}