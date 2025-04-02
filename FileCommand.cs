using System;
using System.IO;
using Microsoft.Extensions.Logging;

public enum FileOperation
{
    Copy,
    Move,
    Delete
}

public class FileCommand
{
    private readonly ILogger<FileCommand> _logger;
    public string SourcePath { get; }
    public string? DestinationPath { get; }
    public FileOperation Operation { get; }

    public FileCommand(string sourcePath, string? destinationPath, FileOperation operation, ILogger<FileCommand> logger)
    {
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
        Operation = operation;
        _logger = logger;
    }

    public void Execute()
    {
        try
        {
            switch (Operation)
            {
                case FileOperation.Copy:
                    if (DestinationPath != null)
                    {
                        File.Copy(SourcePath, DestinationPath, true);
                        _logger.LogInformation($"Copied: {SourcePath} -> {DestinationPath}");
                    }
                    break;

                case FileOperation.Move:
                    if (DestinationPath != null)
                    {
                        File.Move(SourcePath, DestinationPath, true);
                        _logger.LogInformation($"Moved: {SourcePath} -> {DestinationPath}");
                    }
                    break;

                case FileOperation.Delete:
                    File.Delete(SourcePath);
                    _logger.LogInformation($"Deleted: {SourcePath}");
                    break;

                default:
                    _logger.LogWarning($"Invalid operation for {SourcePath}");
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing {SourcePath}: {ex.Message}");
        }
    }
}
