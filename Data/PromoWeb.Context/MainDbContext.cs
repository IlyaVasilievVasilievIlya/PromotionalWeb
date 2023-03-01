using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PromoWeb.Context.Entities;
using PromoWeb.Context.Entities.Configuration;

namespace PromoWeb.Context
{
    public class MainDbContext : IdentityDbContext<User, UserRole, Guid>
    { 
        public DbSet<AppInfo> AppInfos { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        { //вместо onconfigure Configure из designtime
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("users");
            builder.Entity<UserRole>().ToTable("user_roles");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("user_role_owners");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("user_role_claims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");


            builder.ApplyConfiguration(new AnswerConfiguration());
            builder.ApplyConfiguration(new AppInfoConfiguration());
            builder.ApplyConfiguration(new LinkConfiguration());
            builder.ApplyConfiguration(new ContactConfiguration());
            builder.ApplyConfiguration(new QuestionConfiguration());
            builder.ApplyConfiguration(new ImageConfiguration());
            builder.ApplyConfiguration(new SectionConfiguration());
        }
    }
}
