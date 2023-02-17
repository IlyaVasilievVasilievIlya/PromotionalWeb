using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PromoWeb.Context.Entities.Configuration
{
    public class AppInfoConfiguration : IEntityTypeConfiguration<AppInfo>
    {
        public void Configure(EntityTypeBuilder<AppInfo> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Text).IsRequired().HasMaxLength(5000);
            builder.Property(entity => entity.TextTitle).IsRequired().HasMaxLength(100);

            builder.HasOne(a => a.Section)
                .WithMany(b => b.AppInfos)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
