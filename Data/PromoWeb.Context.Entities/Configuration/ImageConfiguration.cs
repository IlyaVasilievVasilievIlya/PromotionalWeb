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

            builder.Property(entity => entity.FileExtension).IsRequired().HasMaxLength(6);

            builder.HasOne(a => a.AppInfo)
                .WithMany(b => b.Screenshots)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
