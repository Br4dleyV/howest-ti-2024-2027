using Microsoft.EntityFrameworkCore;
using ReadingList.Api.Models;

public class ReadingListDbContext(DbContextOptions<ReadingListDbContext> options)
    : DbContext(options)
{
    public DbSet<TitleItem> Titles => Set<TitleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TitleItem>(entity =>
        {
            entity.HasKey(title => title.Id);

            entity.Property(title => title.Title)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(title => title.IsRead)
                .IsRequired()
                .HasDefaultValue(false);
        });
    }
}