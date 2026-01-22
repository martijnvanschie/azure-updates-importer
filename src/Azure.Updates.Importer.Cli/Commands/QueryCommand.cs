using Azure.Updates.Importer.Cli.Core;
using Azure.Updates.Importer.Cli.Tasks;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Text.Json;

namespace Azure.Updates.Importer.Cli.Commands
{
    public class QueryCommand : AsyncCommand<QueryCommand.Settings>
    {
        private static readonly ILogger<MergeTask> _logger = LoggerManager.GetLogger<MergeTask>();

        public QueryCommand()
        {

        }

        public sealed class Settings : CommandSettings
        {
            //[CommandArgument(2, "[name]")]
            //[Description("The package name to add")]
            //public string PackageName { get; init; } = string.Empty;

            //[CommandArgument(0, "<name2>")]
            //[Description("The package name to add")]
            //public string PackageName2 { get; init; } = string.Empty;

            //[CommandArgument(1, "<name>")]
            //[Description("The package name to add")]
            //public string PackageName1 { get; init; } = string.Empty;

            [CommandOption("--id")]
            [Description("Id of the release communication")]
            public string Id { get; init; }

            [CommandOption("--max")]
            [Description("Maximum number of release communications to retrieve")]
            [DefaultValue(200)]
            public int MaxResults { get; init; } = 200;

            [CommandOption("--modified-from")]
            [Description("The date time string from which to query. Should be in unversal time format [yellow]yyyy-MM-ddTHH:mm:ssZ[/]")]
            public string ModifiedDateTimeString { get; init; }
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            AnsiConsoleLogger.LogInfo("Querying release communications api.");

            if (string.IsNullOrEmpty(settings.Id) == false)
            {
                AnsiConsoleLogger.LogInfo($"Getting release communication with id: {settings.Id}");
            }

            if (settings.MaxResults != 200)
            {
                AnsiConsoleLogger.LogInfo($"Maximum results set to: {settings.MaxResults}");
            }

            DateTime? fromDate = null;
            if (string.IsNullOrEmpty(settings.ModifiedDateTimeString) == false)
            {
                AnsiConsoleLogger.LogInfo($"Seach for modification date set to: {settings.ModifiedDateTimeString}");
                fromDate = DateTimeUtils.FromUnversalDateTimeString(settings.ModifiedDateTimeString);
            }

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Starting import...", async ctx =>
                {
                    ReleaseCommunicationsClient client = new ReleaseCommunicationsClient();

                    var result = await client.GetReleaseCommunicationsJsonAsync(id: settings.Id, maxResults: settings.MaxResults, modifiedSince: fromDate);

                    AnsiConsole.Markup(Markup.Escape(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true })));
                    //var task = new VerifyTask();
                    //task.StatusContext = ctx;
                    //await task.RunAsync();
                });

            AnsiConsoleLogger.LogInfo("Query finished.");

            return 0;
        }
    }
}
