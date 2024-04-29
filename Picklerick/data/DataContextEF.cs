using Picklerick.Models;
using Microsoft.EntityFrameworkCore;

namespace Picklerick.Data
{
    public class DataContextEF(IConfiguration config) : DbContext
    {
        private readonly IConfiguration _config = config;

        public virtual DbSet<Rick> Ricks {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseNpgsql(_config.GetConnectionString("DefaultConnection"),
                        optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rick>()
                .ToTable("Ricks")
                .HasKey(r => r.Id);

            modelBuilder.Entity<Rick>()
                .ToTable("Users", "TutorialAppSchema")
                .HasKey(r => r.Id);
        }
    }
}