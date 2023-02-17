using Microsoft.EntityFrameworkCore;

namespace PromoWeb.Context
{
    public class DbContextFactory //используется только для миграций, в di не заносится
    {
        private readonly DbContextOptions<MainDbContext> options;

        public DbContextFactory(DbContextOptions<MainDbContext> options)
        {
            this.options = options;
        }

        public MainDbContext Create()
        {
            return new MainDbContext(options);
        }
    }
}
