using Spectre.Console;
using Spectre.Console.Cli;

namespace Azure.Updates.Importer.Cli
{
    internal class CliProcess
    {
        internal async Task<int> RunAsync(string[] args)
        {
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

                config.AddCommand<Commands.ImportAzureFeedsCommand>("import2")
                    .WithDescription("Import Azure updates feeds")
                    .WithExample(new[] { "import2" });

                config.ValidateExamples();

                config.SetExceptionHandler((ex, resolver) =>
                {
                    AnsiConsole.MarkupInterpolated($"[red]{ex.Message}[/]");
                    return 1;
                });

            });

            return await app.RunAsync(args);
        }
    }
}