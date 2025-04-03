using System;
using System.IO;
using Microsoft.Extensions.Logging;

/// <summary>
/// Specifies the file operations that can be performed.
/// </summary>
public enum FileOperation
{
    Copy,
    Move,
    Delete
}

/// <summary>
/// Represents a command to perform file operations such as copy, move, or delete.
/// </summary>
public class FileCommand
{
    private readonly ILogger<FileCommand> _logger;

    /// <summary>
    /// Gets the source path of the file.
    /// </summary>
    public string? SourcePath { get; }

    /// <summary>
    /// Gets the destination path of the file.
    /// </summary>
    public string? DestinationPath { get; }
    /// <summary>
    /// Gets the file operation to be performed.
    /// </summary>
    public FileOperation Operation { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCommand"/> class.
    /// </summary>
    /// <param name="sourcePath">The source path of the file.</param>
    /// <param name="destinationPath">The destination path of the file.</param>
    /// <param name="operation">The file operation to be performed.</param>
    /// <param name="logger">The logger to log information and errors.</param>
    public FileCommand(string sourcePath, string? destinationPath, FileOperation operation, ILogger<FileCommand> logger)
    {
        SourcePath = sourcePath;
        DestinationPath = destinationPath;
        Operation = operation;
        _logger = logger;
    }

    /// <summary>
    /// Executes the file operation.
    /// </summary>
    public void Execute()
    {
        try
        {
            if (!(FileOperation.Delete == Operation))
            {
                if (!File.Exists(SourcePath))
                {
                    _logger.LogWarning($"Source file does not exist: {SourcePath}");
                    return;
                }
            }

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
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError($"Access denied to {SourcePath}: {ex.Message}");
        }
        catch (IOException ex)
        {
            _logger.LogError($"IO error processing {SourcePath}: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing {SourcePath}: {ex.Message}");
        }
    }
}
