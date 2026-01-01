using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Azure.Updates.Importer.Cli.Commands
{
    public class MergeAzureFeedsCommand : AsyncCommand<MergeAzureFeedsCommand.Settings>
    {
        public MergeAzureFeedsCommand()
        {

        }

        public sealed class Settings : CommandSettings
        {

        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            AnsiConsoleLogger.LogInfo("Merging parquet files into a single file.");

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Starting merge process...", async ctx =>
                {
                    var task = new MergeTask();
                    task.StatusContext = ctx;
                    await task.RunAsync();
                });

            AnsiConsoleLogger.LogInfo("Merge done");

            return 0;
        }
    }
}
