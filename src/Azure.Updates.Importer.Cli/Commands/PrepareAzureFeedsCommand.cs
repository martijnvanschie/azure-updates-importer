using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Azure.Updates.Importer.Cli.Commands
{
    public class PrepareAzureFeedsCommand : AsyncCommand<PrepareAzureFeedsCommand.Settings>
    {
        public PrepareAzureFeedsCommand()
        {

        }

        public sealed class Settings : CommandSettings
        {

        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsoleLogger.LogInfo("Preparing bronze data into silver data.");

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Start processing bronze files...", async ctx =>
                {
                    var task = new PrepareTask();
                    task.StatusContext = ctx;
                    await task.RunAsync();
                });

            AnsiConsoleLogger.LogInfo("Done");

            return 0;
        }
    }
}
