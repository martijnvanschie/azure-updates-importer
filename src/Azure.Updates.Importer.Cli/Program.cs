using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Azure.Updates.Importer.Cli;
using Spectre.Console.Cli;
using Microsoft.Extensions.Configuration;
using Core = Azure.Updates.Importer.Cli.Core;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Services.AddSerilog((context, conf) => { conf.ReadFrom.Configuration(builder.Configuration); });
builder.Services.AddSingleton<MainProcess>();
builder.Services.AddSingleton<CliProcess>();
builder.Services.AddSingleton<ITypeRegistrar, TypeRegistrar>();
var host = builder.Build();

// Register upstream dependencies
var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
Core.LoggerManager.Initiate(loggerFactory);

var configuration = host.Services.GetRequiredService<IConfiguration>();
Core.ConfigurationManager.Initiate(configuration);

// Run the main process
var serviceProvider = builder.Services.BuildServiceProvider();
//var service = serviceProvider.GetRequiredService<MainProcess>();
var process = serviceProvider.GetRequiredService<CliProcess>();
var exitCode = await process.RunAsync(args);

Environment.Exit(exitCode);