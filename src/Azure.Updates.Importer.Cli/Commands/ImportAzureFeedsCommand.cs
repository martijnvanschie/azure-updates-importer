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
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Query customer database", async ctx =>
                {
                    var task = new ImportTask();
                    await task.RunAsync();
                });

            return 0;
        }
    }
}
