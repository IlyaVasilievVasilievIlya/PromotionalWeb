using Microsoft.Extensions.Configuration;

namespace PromoWeb.Settings
{
    public abstract class Settings 
    {
        public static T Load<T>(string key, IConfiguration configuration = null) where T: new()
        {
            var settings = new T();

            SettingsFactory.Create(configuration).GetSection(key).Bind(settings, (x) => { x.BindNonPublicProperties = true; });

            return settings;
        }
    }
}
