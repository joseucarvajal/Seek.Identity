using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeekQ.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeekQ.Identity.Data.EntityConfigurations
{
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> configuration)
        {
            configuration.Property<int?>("GenderId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("GenderId")
                .IsRequired(false);
            configuration.HasOne(c => c.Gender)
                .WithMany()
                .HasForeignKey("GenderId");
        }
    }
}
