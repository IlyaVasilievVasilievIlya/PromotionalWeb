using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PromoWeb.Context.Entities.Configuration
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Description).IsRequired().HasMaxLength(100);

            builder.Property(entity => entity.ImageName).IsRequired().HasMaxLength(100);

            builder.Property(entity => entity.ImagePath).IsRequired();

            builder.HasIndex(entity => entity.ImageName).IsUnique();

            builder.HasOne(a => a.AppInfo)
                .WithMany(b => b.Screenshots)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
