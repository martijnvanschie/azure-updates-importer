﻿using Azure.Updates.Importer.Cli.Core;
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
                    .WithDescription("Import raw Azure updates feeds data into the landing zone")
                    .WithExample(new[] { "import" });

                config.AddCommand<Commands.MergeAzureFeedsCommand>("merge")
                    .WithDescription("Merge raw Azure updates feeds into the Bronze layer")
                    .WithExample(new[] { "merge" });

                config.AddCommand<Commands.PrepareAzureFeedsCommand>("prepare")
                    .WithDescription("Filter, Clean and Augment files moving them to the Silver layer")
                    .WithExample(new[] { "prepare" });

                config.AddCommand<Commands.VerifyCommand>("verify")
                    .WithDescription("Verify the integrity of the landing zone and bronze data")
                    .WithExample(new[] { "verify" });

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