using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PromoWeb.Context.Entities.Configuration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Text).IsRequired().HasMaxLength(1000);

            builder.Property(entity => entity.Date).IsRequired();

            builder.HasOne(a => a.Question)
                .WithOne(b => b.Answer)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
