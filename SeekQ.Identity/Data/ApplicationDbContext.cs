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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserGenderEntityConfiguration());
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }
    }
}
