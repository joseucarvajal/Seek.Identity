namespace SeekQ.Identity.Data.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> configuration)
        {
            configuration.Property(c => c.EmailConfirmationCode)
                .HasMaxLength(7);

            configuration.Property(c => c.MakeBirthDatePublic)
                .HasDefaultValue(true);
            configuration.Property(c => c.MakeFirstNamePublic)
                .HasDefaultValue(true);
            configuration.Property(c => c.MakeLastNamePublic)
                .HasDefaultValue(true);

            configuration.Property(c => c.BirthDate)
                .IsRequired(false);
            configuration.Property(c => c.NickName)
                .HasMaxLength(20)
                .IsRequired(false);
            configuration.Property(c => c.FirstName)
                .HasMaxLength(50)
                .IsRequired(false);
            configuration.Property(c => c.LastName)
                .HasMaxLength(50)
                .IsRequired(false);
            configuration.Property(c => c.School)
                .HasMaxLength(50)
                .IsRequired(false);
            configuration.Property(c => c.Job)
                .HasMaxLength(50)
                .IsRequired(false); ;
            configuration.Property(c => c.About)
                .HasMaxLength(1000)
                .IsRequired(false); ;

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