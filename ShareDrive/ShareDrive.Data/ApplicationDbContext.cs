using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShareDrive.Models;
using Microsoft.AspNetCore.Identity;

namespace ShareDrive.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DbSet<Car> Cars { get; set; }
        
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
        }
    }    
}
