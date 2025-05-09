using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Updates.Importer.Cli.Core
{
    internal class AnsiConsoleLogger
    {
        public static void LogInfo(string message, ILogger? logger = null)
        {
            AnsiConsole.MarkupLine($"[green]{message.EscapeMarkup()}[/]");

            if (logger is not null)
            {
                logger.LogInformation(message);
            }
        }

        public static void LogWarning(string message)
        {
            AnsiConsole.MarkupLine($"[yellow]{message.EscapeMarkup()}[/]");
        }

        public static void LogError(string message)
        {
            AnsiConsole.MarkupLine($"[red]{message.EscapeMarkup()}[/]");
        }


        public static void LogSuccess(string message)
        {
            AnsiConsole.MarkupLine($"[green]{message.EscapeMarkup()}[/]");
        }

        public static void LogDebug(string message)
        {
            AnsiConsole.MarkupLine($"[grey]{message.EscapeMarkup()}[/]");
        }
    }
}
