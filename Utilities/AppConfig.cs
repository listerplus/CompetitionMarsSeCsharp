using System.Configuration;
using Microsoft.Extensions.Configuration;
using RazorEngine.Configuration;

namespace CompetitionMarsSeCsharp.Utilities
{
    public class AppConfig
    {
        public string Browser { get; set; }
        public string Url { get; set; }

        public static AppConfig LoadConfiguration()
        {
            // Needed packages: Microsoft.Extensions.Configuration
            // Microsoft.Extensions.Configuration.Binder
            // Microsoft.Extensions.Configuration.Json
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var appConfig = new AppConfig();
            configuration.GetSection("Settings").Bind(appConfig);

            return appConfig;
        }
    }
}