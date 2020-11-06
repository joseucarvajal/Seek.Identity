using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeekQ.Identity.Data.EntityConfigurations;
using SeekQ.Identity.Models;

namespace SeekQ.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserGender> UserGenders { get; set; }
        public DbSet<UserLanguageKnow> UserLanguageKnows { get; set; }
        public DbSet<LanguageKnow> LanguageKnows { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new LanguageKnowEntityConfiguration());
            builder.ApplyConfiguration(new UserGenderEntityConfiguration());
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }
    }
}
