using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PromoWeb.Context.Entities
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.ContactOwner).IsRequired().HasMaxLength(50);

            builder.Property(entity => entity.Email).HasMaxLength(100);
            builder.Property(entity => entity.WebSite).HasMaxLength(200);
        }
    }
}
