using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PromoWeb.Context.Entities.Configuration
{
    public class LinkConfiguration : IEntityTypeConfiguration<Link>
    {
        public void Configure(EntityTypeBuilder<Link> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.LinkText).IsRequired().HasMaxLength(500);

            builder.Property(entity => entity.Description).IsRequired().HasMaxLength(1000);

            builder.HasOne(a => a.Section)
                .WithMany(b => b.Links)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
