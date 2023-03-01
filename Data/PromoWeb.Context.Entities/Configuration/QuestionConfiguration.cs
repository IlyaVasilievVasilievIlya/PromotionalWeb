using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PromoWeb.Context.Entities.Configuration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Text).IsRequired().HasMaxLength(500);

            builder.Property(entity => entity.Email).HasMaxLength(100);

            builder.Property(entity => entity.RecipientEmail).IsRequired().HasMaxLength(100);

            builder.Property(entity => entity.Date).IsRequired();
        }
    }
}
