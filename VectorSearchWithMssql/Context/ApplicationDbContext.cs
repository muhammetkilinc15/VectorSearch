using Microsoft.EntityFrameworkCore;

namespace VectorSearchWithMssql.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Models.Product> Products { get; set; } // Assuming you have a Product model defined in Models namespace
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Models.Product>().Property(p => p.EmbedingVector).HasColumnType("vector(768)");
            modelBuilder.Entity<Models.Product>().Property(p => p.Name).HasColumnType("nvarchar(100)");
            modelBuilder.Entity<Models.Product>().Property(p => p.Description).HasColumnType("nvarchar(300)");
            modelBuilder.Entity<Models.Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Models.Product>().Property(p => p.Category).HasColumnType("nvarchar(100)");

        }
    }
}
