using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// The main entry point for the application.
/// </summary>
class Program
{
    static void Main()
    {
        // Load configuration from appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Ensures correct working directory
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Resolve relative path to ensure it is not inside 'bin/Debug'
        string projectRoot = Directory.GetCurrentDirectory();

        while (!Directory.Exists(Path.Combine(projectRoot, "NetworkPath")) && projectRoot != Path.GetPathRoot(projectRoot))
        {
            projectRoot = Directory.GetParent(projectRoot)?.FullName ?? projectRoot;
        }
        var logFilePath = Path.Combine(projectRoot, config["Logging:LogFilePath"]);

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var builder = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddSerilog();
                });
            })
            .Build();

        ExucuteFlow(projectRoot, config, builder);
    }

    private static void ExucuteFlow(string projectRoot, IConfiguration config, IHost builder)
    {
        var sourcePath = Path.Combine(projectRoot, config["Paths:Source"]);
        var archivePath = Path.Combine(projectRoot, config["Paths:Archive"]);
        var destinationPath = Path.Combine(projectRoot, config["Paths:Destination"]);
        var filename = config["paths:Filename"] ?? "file1.txt";

        

        // Resolve loggers
        var loggerFactory = builder.Services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger<Program>();
        var fileCommandLogger = loggerFactory.CreateLogger<FileCommand>();
        var fileProcessorLogger = loggerFactory.CreateLogger<FileProcessor>();

        logger.LogInformation("FileFerry application started...");


        if (!Directory.Exists(sourcePath))
        {
            logger.LogError("sourcePath not exist...");
            return;
        }

        if (!Directory.Exists(archivePath))
        {
            logger.LogError("archivePath not exist...");
            return;
        }
        if (!Directory.Exists(destinationPath))
        {
            logger.LogError("destinationPath not exist...");
            return;
        }

        try
        {
            // File operations workflow
            var commands = new List<FileCommand>
            {
                new FileCommand(Path.Combine(sourcePath, filename), Path.Combine(archivePath, filename), FileOperation.Copy, fileCommandLogger),
                new FileCommand(Path.Combine(archivePath, filename), Path.Combine(destinationPath, filename), FileOperation.Move, fileCommandLogger),
                new FileCommand(Path.Combine(destinationPath, filename), null, FileOperation.Delete, fileCommandLogger)
            };



            var processor = new FileProcessor(commands, fileProcessorLogger);
            processor.ExecuteWorkflow();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during file processing.");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        Console.WriteLine("File processing completed. Check logs for details.");
    }

   
}
