using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Azure.Updates.Importer.Cli.Commands
{
    public class ImportAzureFeedsCommand : AsyncCommand<ImportAzureFeedsCommand.Settings>
    {
        public ImportAzureFeedsCommand()
        {

        }

        public sealed class Settings : CommandSettings
        {

        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsoleLogger.LogInfo("importing Azure update feeds into parquet files.");

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Starting import...", async ctx =>
                {
                    var task = new ImportTask();
                    //task.StatusContext = ctx;
                    await task.RunAsync();
                });

            AnsiConsoleLogger.LogInfo("Merge done");

            return 0;
        }
    }
}
