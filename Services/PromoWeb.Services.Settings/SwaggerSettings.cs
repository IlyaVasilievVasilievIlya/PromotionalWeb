using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoWeb.Services.Settings
{
    public class SwaggerSettings
    {
        public bool Enabled { get; private set; }

        public string OAuthClientId { get; private set; } //передается для конфигурации swagger, 
        public string OAuthClientSecret { get; private set; }

        public SwaggerSettings() 
        { 
            Enabled = false;
        }
    }
}
