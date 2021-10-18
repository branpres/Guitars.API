using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class GuitarConfiguration : IEntityTypeConfiguration<Guitar>
    {
        public void Configure(EntityTypeBuilder<Guitar> builder)
        {
            builder.Property(x => x.Make)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}