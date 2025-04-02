using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

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

        var sourcePath = Path.Combine(projectRoot, config["Paths:Source"]);
        var archivePath = Path.Combine(projectRoot, config["Paths:Archive"]);
        var destinationPath = Path.Combine(projectRoot, config["Paths:Destination"]);
        var logFilePath = Path.Combine(projectRoot, config["Logging:LogFilePath"]);
        var filename =  config["paths:Filename"] ?? "file1.txt";


        //string networkPath = Path.Combine(projectRoot, "NetworkPath");
        //string sourcePath = Path.Combine(networkPath, "Source");
        //string archivePath = Path.Combine(networkPath, "Archive");
        //string destinationPath = Path.Combine(networkPath, "Destination");
        //string logFilePath = Path.Combine(networkPath, "logs", "FileOperations.log");

        // Ensure directories exist
        //Directory.CreateDirectory(sourcePath);
        //Directory.CreateDirectory(archivePath);
        //Directory.CreateDirectory(destinationPath);
        //Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

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

        // Resolve loggers
        var loggerFactory = builder.Services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger<Program>();
        var fileCommandLogger = loggerFactory.CreateLogger<FileCommand>();
        var fileProcessorLogger = loggerFactory.CreateLogger<FileProcessor>();

        logger.LogInformation("FileFerry application started...");

        try
        {
            // File operations workflow
            var commands = new List<FileCommand>
            {
                new FileCommand(Path.Combine(archivePath, filename), Path.Combine(sourcePath, filename), FileOperation.Copy, fileCommandLogger),
                new FileCommand(Path.Combine(sourcePath, filename), Path.Combine(destinationPath, filename), FileOperation.Move, fileCommandLogger),
                new FileCommand(Path.Combine(sourcePath, filename), null, FileOperation.Delete, fileCommandLogger)
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
