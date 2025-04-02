using Microsoft.Extensions.Logging;
using Serilog;
using System;

public static class LoggerConfig
{
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
