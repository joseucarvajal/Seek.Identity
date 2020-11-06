namespace SeekQ.Identity.Data.EntityConfigurations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class UserLanguageKnowEntityConfiguration : IEntityTypeConfiguration<UserLanguageKnow>
    {
        public void Configure(EntityTypeBuilder<UserLanguageKnow> configuration)
        {
            configuration.HasKey(g => g.Id);

            configuration.HasIndex(u => new { u.LanguageKnowId, u.ApplicationUserId }).IsUnique();

            configuration.Property<Guid>("ApplicationUserId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ApplicationUserId")
                .IsRequired();
            configuration.HasOne(c => c.ApplicationUser)
                .WithMany()
                .HasForeignKey("ApplicationUserId");

            configuration.Property<int>("LanguageKnowId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("LanguageKnowId")
                .IsRequired();
            configuration.HasOne(c => c.LanguageKnow)
                .WithMany()
                .HasForeignKey("LanguageKnowId");
        }
    }
}