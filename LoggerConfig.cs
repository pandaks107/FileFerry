using Microsoft.Extensions.Logging;
using Serilog;
using System;

/// <summary>
/// Provides configuration for creating loggers using Serilog.
/// </summary>
public static class LoggerConfig
{
    /// <summary>
    /// Creates a logger for the specified type with the given log file path.
    /// </summary>
    /// <typeparam name="T">The type for which the logger is created.</typeparam>
    /// <param name="logFilePath">The path to the log file.</param>
    /// <returns>An <see cref="ILogger{T}"/> instance.</returns>
    public static ILogger<T> CreateLogger<T>(string logFilePath)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var factory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });

        return factory.CreateLogger<T>();
    }
}
