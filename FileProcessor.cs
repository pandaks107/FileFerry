using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

public class FileProcessor
{
    private readonly List<FileCommand> _commands;
    private readonly ILogger<FileProcessor> _logger;

    public FileProcessor(List<FileCommand> commands, ILogger<FileProcessor> logger)
    {
        _commands = commands;
        _logger = logger;
    }

    public void ExecuteWorkflow()
    {
        _logger.LogInformation("Starting file processing workflow...");
        foreach (var command in _commands)
        {
            command.Execute();
        }
        _logger.LogInformation("Workflow execution completed.");
    }
}
