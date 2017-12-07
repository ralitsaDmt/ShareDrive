using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShareDrive.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ShareDrive.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DbSet<Car> Cars { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Drive> Drives { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(i => {
                i.ToTable("Users");
                i.HasKey(x => x.Id);
            });
            builder.Entity<ApplicationRole>(i => {
                i.ToTable("Role");
                i.HasKey(x => x.Id);
            });
            builder.Entity<IdentityUserRole<int>>(i => {
                i.ToTable("UserRole");
                i.HasKey(x => new { x.RoleId, x.UserId });
            });
            builder.Entity<IdentityUserLogin<int>>(i => {
                i.ToTable("UserLogin");
                i.HasKey(x => new { x.ProviderKey, x.LoginProvider });
            });
            builder.Entity<IdentityRoleClaim<int>>(i => {
                i.ToTable("RoleClaims");
                i.HasKey(x => x.Id);
            });
            builder.Entity<IdentityUserClaim<int>>(i => {
                i.ToTable("UserClaims");
                i.HasKey(x => x.Id);
            });

            builder.Entity<IdentityUserToken<int>>(i =>
            {
                i.ToTable("UserTokens");
                i.HasKey(x => x.UserId);
            });

            builder.Entity<City>(i =>
            {
                i.ToTable("Cities");
                i.HasKey(x => x.Id);
            });

            builder.Entity<Drive>(i =>
            {
                i.ToTable("Drives");
                i.HasKey(x => x.Id);
            });

            builder.Entity<Car>()
                .HasOne(c => c.Owner)
                .WithMany(x => x.Cars)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Drive>()
                .HasOne(d => d.From)
                .WithMany(c => c.DrivesFrom)
                .HasForeignKey(d => d.FromId)
                .IsRequired(false);

            builder.Entity<Drive>()
                .HasOne(d => d.To)
                .WithMany(c => c.DrivesTo)
                .HasForeignKey(d => d.ToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Drive>()
                .HasOne(d => d.Car)
                .WithMany(c => c.Drives)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Drive>()
                .HasOne(d => d.Driver)
                .WithMany(dr => dr.Drives)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }    
}
