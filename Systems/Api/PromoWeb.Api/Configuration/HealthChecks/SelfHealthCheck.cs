﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;

namespace PromoWeb.Api.Configuration.HealthChecks
{
    public class SelfHealthCheck : IHealthCheck //есть на metanit еще пример (здесь загрузка своей сборки)
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var assembly = Assembly.Load("PromoWeb.API");
            var versionNumber = assembly.GetName().Version;

            return Task.FromResult(HealthCheckResult.Healthy(description: $"Build {versionNumber}"));
        }
    }
}
