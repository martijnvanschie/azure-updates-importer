using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Tasks;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Azure.Updates.Importer.Cli
{
    internal class CliProcess
    {
        private static ILogger<CliProcess> _logger;

        public CliProcess(ILogger<CliProcess> logger)
        {
            _logger = logger;
        }

        internal async Task<int> RunAsync(string[] args)
        {
            // Log the current process folder and thread id
            _logger.LogInformation("Current process folder: {folder}", Environment.CurrentDirectory);
            _logger.LogInformation("Current thread id: {threadId}", Environment.ProcessId);


            var app = new CommandApp();
            app.Configure(config =>
            {
                config.SetApplicationName("aui");
                config.SetApplicationVersion("1.0");

#if DEBUG
                config.PropagateExceptions();
#endif

                //foreach (ICommandManager command in commandManager)
                //{
                //    command.AddCommands(config);
                //}

                config.AddCommand<Commands.ImportAzureFeedsCommand>("import")
                    .WithDescription("Import Azure updates feeds")
                    .WithExample(new[] { "import" });

                config.AddCommand<Commands.MergeAzureFeedsCommand>("merge")
                    .WithDescription("Merge Azure updates feeds")
                    .WithExample(new[] { "merge" });

                config.ValidateExamples();

                config.SetExceptionHandler((ex, resolver) =>
                {
                    AnsiConsole.MarkupInterpolated($"[red]{ex.Message}[/]");
                    return 1;
                });

            });

            try
            {
                return await app.RunAsync(args);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupInterpolated($"[red]{ex.Message}[/]");
                return -1;
            }
        }
    }
}