using Microsoft.Extensions.Configuration;

namespace PromoWeb.Settings
{
    public static class SettingsFactory
    {
        public static IConfiguration Create(IConfiguration? configuration = null)
        {
            var conf = configuration ?? new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: false)
                                            .AddJsonFile("appsettings.development.json", optional: true) //не выкинет искл. что файл не найден
                                            .AddEnvironmentVariables() //в докер передаем файлы окружения, приложения их подгрузят здесь (те которые им нужны)
                                            .Build();

            return conf;
        }
    }
}
