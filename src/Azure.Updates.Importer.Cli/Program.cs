// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Azure.Updates.Importer.Cli;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Services.AddSerilog((context, conf) => { conf.ReadFrom.Configuration(builder.Configuration); });
builder.Services.AddSingleton<MainProcess>();
var host = builder.Build();

//// Initialize the upstream ConfigurationManager
//ConfigurationManager.Initialize(host);

// Run the main process
var serviceProvider = builder.Services.BuildServiceProvider();
var service = serviceProvider.GetRequiredService<MainProcess>();
var exitCode = await service.StartAsync(args);

Environment.Exit(exitCode);