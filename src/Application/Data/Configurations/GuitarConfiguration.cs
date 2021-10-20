using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Data.Configurations
{
    public class GuitarConfiguration : IEntityTypeConfiguration<Guitar>
    {
        public void Configure(EntityTypeBuilder<Guitar> builder)
        {
            builder.Property(x => x.GuitarType)
                .IsRequired();
            builder.Property(x => x.MaxNumberOfStrings)
                .IsRequired();
            builder.Property(x => x.Make)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}