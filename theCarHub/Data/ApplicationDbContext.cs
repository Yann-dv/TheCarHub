using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace theCarHub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Car>? Cars { get; set; }
        public DbSet<UserCar>? UserCars { get; set; }
        public DbSet<CarImages>? CarImages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserCar>()
                .HasKey(t => new { t.UserId, t.CarId });
            modelBuilder.Entity<CarImages>()
                .HasKey(t => new {t.Id, t.CarId});
        }
    }
}