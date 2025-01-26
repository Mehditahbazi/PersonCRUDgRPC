using Microsoft.EntityFrameworkCore;
using PersonCRUD.Domain.Models;

namespace PersonCRUD.Infrastructure.Data
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options) { }

        public DbSet<Individual> Individuals { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity mappings here (optional but recommended for more complex scenarios)
            modelBuilder.Entity<Individual>(entity =>
            {
                entity.HasKey(e => e.ID); // Explicitly define the primary key (if needed)
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100); // Example: Name is required and has max length 100
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.NationalNo).HasMaxLength(20);
                // ... other configurations
            });
            // You can configure other entities here if you have more than one.
            base.OnModelCreating(modelBuilder);
        }
    }
}
