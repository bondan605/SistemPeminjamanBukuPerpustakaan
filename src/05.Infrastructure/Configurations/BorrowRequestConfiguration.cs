using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Infrastructure.Configurations;
public class BorrowRequestConfiguration : IEntityTypeConfiguration<BorrowRequest>
{
    public void Configure(EntityTypeBuilder<BorrowRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BorrowDate).IsRequired();
        builder.Property(x => x.ReturnDate).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>();

        builder.HasOne(x => x.User)
               .WithMany(u => u.BorrowRequests)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ApprovedBy)
               .WithMany()
               .HasForeignKey(x => x.ApprovedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}