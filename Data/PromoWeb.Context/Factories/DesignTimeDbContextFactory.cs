using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PromoWeb.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
    {
        private const string migrationProjectPrefix = "PromoWeb.Context.Migrations";

        public MainDbContext CreateDbContext(string[] args)
        {
            var provider = $"{DbType.PostgreSQL}".ToLower(); //для команды migration in cli

            var configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.context.json")
                 .Build();


            DbContextOptions<MainDbContext> options;

            if (provider.Equals($"{DbType.PostgreSQL}".ToLower()))
            {
                options = new DbContextOptionsBuilder<MainDbContext>()
                        .UseNpgsql(
                            configuration.GetConnectionString(provider),
                            opts => opts
                                .MigrationsAssembly($"{migrationProjectPrefix}{DbType.PostgreSQL}")
                        )
                        .Options;
            }
            else
            {
                throw new Exception($"Unsupported provider: {provider}");
            }

            var dbf = new DbContextFactory(options); //создание maindbcontext и в конструкторе options
            return dbf.CreateContext();
        }
    }
}
