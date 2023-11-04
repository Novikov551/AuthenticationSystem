using AuthenticationSystem.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationSystem.Infrastructure.Configurations
{
    internal class RoleConfiguration : BaseEntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            builder.ToTable("role");

            builder.Property(e => e.RoleType)
                .HasColumnType(ConfigurationConstants.BinaryTypeName)
                .HasColumnName("role_type")
                .IsRequired();

            builder.Property(e => e.RoleName)
                .HasColumnType(ConfigurationConstants.ContentTypeName)
                .HasColumnName("role_name")
                .IsRequired();
        }
    }
}
