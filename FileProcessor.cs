using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a processor that executes a workflow of file commands.
/// </summary>
public class FileProcessor
{
    private readonly List<FileCommand> _commands;
    private readonly ILogger<FileProcessor> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileProcessor"/> class.
    /// </summary>
    /// <param name="commands">The list of file commands to be executed.</param>
    /// <param name="logger">The logger to log information and errors.</param>
    public FileProcessor(List<FileCommand> commands, ILogger<FileProcessor> logger)
    {
        _commands = commands;
        _logger = logger;
    }
    /// <summary>
    /// Executes the workflow of file commands.
    /// </summary>
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
