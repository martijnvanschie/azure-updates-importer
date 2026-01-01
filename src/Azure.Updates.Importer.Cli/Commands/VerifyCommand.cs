using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Azure.Updates.Importer.Cli.Commands
{
    public class VerifyCommand : AsyncCommand<VerifyCommand.Settings>
    {
        public VerifyCommand()
        {

        }

        public sealed class Settings : CommandSettings
        {

        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            AnsiConsoleLogger.LogInfo("Verifying data.");

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Starting import...", async ctx =>
                {
                    var task = new VerifyTask();
                    task.StatusContext = ctx;
                    await task.RunAsync();
                });

            AnsiConsoleLogger.LogInfo("Verification finished.");

            return 0;
        }
    }
}
