using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeekQ.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeekQ.Identity.Data.EntityConfigurations
{
    public class UserGenderEntityConfiguration : IEntityTypeConfiguration<UserGender>
    {
        public void Configure(EntityTypeBuilder<UserGender> configuration)
        {            
            configuration.HasKey(g => g.Id);

            configuration.Property(g => g.Id)
                .ValueGeneratedNever()
                .IsRequired();

            configuration.Property(g => g.Name)
                .HasMaxLength(30)
                .IsRequired();
        }
    }
}
