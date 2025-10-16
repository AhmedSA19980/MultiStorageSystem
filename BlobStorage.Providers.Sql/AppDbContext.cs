using BlobStorage.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace BlobStorage.Providers.Sql;

public class AppDbContext: DbContext
{


    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {

    }

    public DbSet<User> Users { get; set; }
  
    public DbSet<BlobMetadata> BlobMetadata { get; set; }
    public DbSet<Blob> Blobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
        .IsUnique();

        modelBuilder.Entity<BlobMetadata>()
           .Property(e => e.Created_at)
           .HasDefaultValueSql("SYSUTCDATETIME()");
    }
    
}
