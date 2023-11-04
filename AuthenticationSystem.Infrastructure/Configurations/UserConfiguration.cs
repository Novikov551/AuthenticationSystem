using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

            builder.Property(e => e.RoleId)
                .HasColumnType(ConfigurationConstants.GuidTypeName)
                .HasColumnName("role_id")
                .IsRequired();

            builder.Property(e => e.BirthDate)
                .HasColumnType(ConfigurationConstants.DateOnlyTypeName)
                .HasColumnName("birth_date");

            builder.Property(e => e.PhoneNumber)
                .HasColumnType(ConfigurationConstants.IntegerTypeName)
                .HasColumnName("phone_number");

            builder.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.EmailHash).IsUnique();
        }
    }
}
