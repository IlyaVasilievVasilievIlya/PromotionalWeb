using Microsoft.EntityFrameworkCore;

namespace PromoWeb.Context
{
    public class DbContextFactory
    {
        private readonly DbContextOptions<MainDbContext> options;

        public DbContextFactory(DbContextOptions<MainDbContext> options)
        {
            this.options = options;
        }

        public MainDbContext CreateContext()
        {
            return new MainDbContext(options);
        }
    }
}
