using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Azure.Updates.Importer.Cli.Commands
{
    public class ImportAzureFeedsV2Command : AsyncCommand<ImportAzureFeedsV2Command.Settings>
    {
        public ImportAzureFeedsV2Command()
        {

        }

        public sealed class Settings : CommandSettings
        {

        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsoleLogger.LogInfo("Importing Azure release communications updates.");

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Starting import...", async ctx =>
                {
                    var task = new ImportTaskV2();
                    task.StatusContext = ctx;
                    await task.RunAsync();
                });

            AnsiConsoleLogger.LogInfo("Done importing Azure release communications updates.");

            return 0;
        }
    }
}
