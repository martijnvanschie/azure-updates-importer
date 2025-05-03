using Microsoft.Extensions.Configuration;

namespace Azure.Updates.Importer.Cli.Core
{
    public class ConfigurationManager
    {
        internal static IConfiguration _configuration = null!;
        private static ConfigurationRoot _rootConfig = new ConfigurationRoot();

        public static void Initiate(IConfiguration configuration)
        {
            _configuration = configuration;

            configuration.GetSection(nameof(Settings)).Bind(_rootConfig.Settings);
        }

        public static ConfigurationRoot GetConfiguration()
        {
            return _rootConfig;
        }
    }

    public class ConfigurationRoot
    {
        public Settings Settings { get; set; } = new Settings();
    }

    public class Settings
    {
        public string? OutputPath { get; set; }
        public string? UpdatesUrl { get; set; }
    }
}
