namespace SeekQ.Identity.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models.Profile;

    public class LanguageKnowEntityConfiguration : IEntityTypeConfiguration<LanguageKnow>
    {
        public void Configure(EntityTypeBuilder<LanguageKnow> configuration)
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