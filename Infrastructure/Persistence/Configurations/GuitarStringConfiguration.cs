using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class GuitarStringConfiguration : IEntityTypeConfiguration<GuitarString>
    {
        public void Configure(EntityTypeBuilder<GuitarString> builder)
        {
            builder.Property(x => x.Gauge)
                .IsRequired()
                .HasMaxLength(5);
            builder.Property(x => x.Tuning)
                .IsRequired()
                .HasMaxLength(2);
        }
    }
}