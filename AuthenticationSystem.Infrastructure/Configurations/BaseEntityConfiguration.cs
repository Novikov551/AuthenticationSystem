using AuthenticationSystem.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthenticationSystem.Infrastructure.Configurations;

internal abstract class BaseEntityConfiguration<TBaseEntity> : IEntityTypeConfiguration<TBaseEntity>
    where TBaseEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TBaseEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.CreatedDate)
            .HasColumnType(ConfigurationConstants.DateTimeTypeName)
            .HasColumnName("created_date")
            .IsRequired();

        builder.Property(e => e.UpdatedDate)
            .HasColumnType(ConfigurationConstants.DateTimeTypeName)
            .HasColumnName("modified_date")
            .IsRequired();

        builder.Property(e => e.Version)
            .HasColumnType(ConfigurationConstants.GuidTypeName)
            .HasColumnName("version")
            .IsRequired();
    }
}