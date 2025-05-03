using Microsoft.Extensions.Logging;

namespace Azure.Updates.Importer.Cli.Core
{
    internal class ImporterContext
    {
        private static readonly ILogger<ImporterContext> _logger = LoggerManager.GetLogger<ImporterContext>();
        private static readonly Settings _settings = ConfigurationManager.GetConfiguration().Settings;
        private static readonly DateTime _dateImport = DateTime.Now;

        private static bool _isInitialized = false;

        private static DirectoryInfo _outputPath;
        private static DirectoryInfo _landingPath;
        private static DirectoryInfo _bronsePath;
        private static DirectoryInfo _silverPath;
        private static DirectoryInfo _goldPath;

        static ImporterContext()
        {
            Initialize();
        }

        public static DirectoryInfo OutputPath => _outputPath;
        public static DirectoryInfo LandingPath => _landingPath;
        public static DirectoryInfo BronsePath => _bronsePath;
        public static DirectoryInfo SilverPath => _silverPath;
        public static DirectoryInfo GoldPath => _goldPath;
        public static DateTime DateImport => _dateImport;

        private static void Initialize()
        {
            if (_isInitialized)
            {
                _logger.LogWarning("ImporterContext is already initialized.");
                return;
            }

            InitFolders();


            _isInitialized = true;
            // Initialize other components or settings here
            // For example, you might want to set up logging, configuration, etc.
            _logger.LogInformation("ImporterContext initialized.");
        }

        private static void InitFolders()
        {
            _outputPath = new DirectoryInfo(_settings.OutputPath);
            _landingPath = new DirectoryInfo(Path.Combine(_outputPath.FullName, "landing"));
            _bronsePath = new DirectoryInfo(Path.Combine(_outputPath.FullName, "bronze"));
            _silverPath = new DirectoryInfo(Path.Combine(_outputPath.FullName, "silver"));
            _goldPath = new DirectoryInfo(Path.Combine(_outputPath.FullName, "gold"));

            if (_landingPath.Exists == false)
            {
                _landingPath.Create();
            }

            if (_bronsePath.Exists == false)
            {
                _bronsePath.Create();
            }

            if (_silverPath.Exists == false)
            {
                _silverPath.Create();
            }

            if (_goldPath.Exists == false)
            {
                _goldPath.Create();
            }
        }
    }
}
