﻿using Microsoft.EntityFrameworkCore;

namespace PromoWeb.Context
{
    public class DbContextOptionsFactory
    {
        private const string migrationProjectPrefix = "PromoWeb.Context.Migrations";

        public static DbContextOptions<MainDbContext> Create(string connStr, DbType dbType)
        {
            var bldr = new DbContextOptionsBuilder<MainDbContext>();

            Configure(connStr, dbType).Invoke(bldr);

            return bldr.Options;
        }

        public static Action<DbContextOptionsBuilder> Configure(string connStr, DbType dbType)
        {
            return (bldr) =>
            {
                switch (dbType)
                {
                    case DbType.PostgreSQL:
                        bldr.UseNpgsql(connStr,
                            opts => opts
                                    .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                                    .MigrationsHistoryTable("_EFMigrationsHistory", "public")
                                    .MigrationsAssembly($"{migrationProjectPrefix}{DbType.PostgreSQL}")
                            );
                        break;
                }

                bldr.EnableSensitiveDataLogging();
                //bldr.UseLazyLoadingProxies();
                bldr.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            };
        }
    }
}
