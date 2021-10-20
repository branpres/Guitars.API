using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations
{
    public class GuitarStringConfiguration : IEntityTypeConfiguration<GuitarString>
    {
        public void Configure(EntityTypeBuilder<GuitarString> builder)
        {
            builder.Property(x => x.Number)
                .IsRequired()
                .HasMaxLength(2);
            builder.Property(x => x.Gauge)
                .IsRequired()
                .HasMaxLength(5);
            builder.Property(x => x.Tuning)
                .HasMaxLength(2);
        }
    }
}