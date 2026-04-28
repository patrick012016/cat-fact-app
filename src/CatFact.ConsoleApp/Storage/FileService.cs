using System.Text;
using CatFactApp.Configuration;
using CatFactApp.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatFactApp.Storage;

public class FileService : IFileService
{
    private readonly CatFactConfig _config;
    private readonly ILogger<FileService> _logger;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public FileService(IOptions<CatFactConfig> options, ILogger<FileService> logger)
    {
        _config = options.Value;
        _logger = logger;
    }

    public async Task AppendFactAsync<T>(T model, ITextFormatter<T> formatter,
        CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            var directory = Path.GetDirectoryName(_config.OutputFilePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var lineToAppend = formatter.Format(model);

            await File.AppendAllTextAsync(_config.OutputFilePath, lineToAppend, Encoding.UTF8, cancellationToken);

            _logger.LogDebug("Data added to the file: {FilePath}", _config.OutputFilePath);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
