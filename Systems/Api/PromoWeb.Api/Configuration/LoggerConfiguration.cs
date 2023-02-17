using Serilog;

namespace PromoWeb.Api.Configuration
{
    /// <summary>
    /// Logger Configuration
    /// </summary>
    public static class LoggerConfiguration
    {
        /// <summary>
        /// Add logger
        /// </summary>
        public static void AddAppLogger(this WebApplicationBuilder builder)
        {
            var logger = new Serilog.LoggerConfiguration()
                .Enrich.WithCorrelationIdHeader()
                .Enrich.FromLogContext() //взять логи из запросов и ответов
                .ReadFrom.Configuration(builder.Configuration) //пакет настроек (берет автоматически)
                .CreateLogger();

            builder.Host.UseSerilog(logger, true);
        }
    }
}
