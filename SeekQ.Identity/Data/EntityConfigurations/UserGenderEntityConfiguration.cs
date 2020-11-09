namespace SeekQ.Identity.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Profile;

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
