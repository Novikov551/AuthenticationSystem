using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stemy.Payments.Infrastructure.Configurations;
using UserAuthenticationSystem.Domain.Core;

namespace AuthenticationSystem.Infrastructure.Configurations
{
    internal class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("user");

            builder.Property(e => e.Surname)
                .HasColumnType(ConfigurationConstants.ContentTypeName)
                .HasColumnName("surname")
                .IsRequired();

            builder.Property(e => e.Name)
                .HasColumnType(ConfigurationConstants.ContentTypeName)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(e => e.PasswordHash)
                .HasColumnType(ConfigurationConstants.ContentTypeName)
                .HasColumnName("password_hash")
                .IsRequired();

            builder.Property(e => e.EmailHash)
                .HasColumnType(ConfigurationConstants.ContentTypeName)
                .HasColumnName("email_hash")
                .IsRequired();
        }
    }
}
