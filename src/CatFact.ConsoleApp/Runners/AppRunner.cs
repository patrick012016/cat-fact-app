using CatFactApp.Clients;
using CatFactApp.Formatters;
using CatFactApp.Models;
using CatFactApp.Storage;
using Microsoft.Extensions.Logging;

namespace CatFactApp.Runners;

public class AppRunner
{
    private readonly ICatFactClient _catFactClient;
    private readonly IFileService _fileService;
    private readonly ILogger<AppRunner> _logger;
    private readonly ITextFormatter<CatFactModel> _formatter;

    public AppRunner(
        ICatFactClient catFactClient,
        IFileService fileService,
        ILogger<AppRunner> logger, ITextFormatter<CatFactModel> formatter)
    {
        _catFactClient = catFactClient;
        _fileService = fileService;
        _logger = logger;
        _formatter = formatter;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (Console.IsInputRedirected || !Environment.UserInteractive)
        {
            await ProcessSingleFactAsync(cancellationToken);
            return;
        }
        
        Console.WriteLine("Press [SPACE] to fetch a fact.");
        Console.WriteLine("Press [Q] to quit.");

        while (!cancellationToken.IsCancellationRequested)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true).Key;

                if (key == ConsoleKey.Q)
                {
                    _logger.LogInformation("Closing the application...");
                    break;
                }

                if (key == ConsoleKey.Spacebar)
                {
                    await ProcessSingleFactAsync(cancellationToken);
                }
            }

            await Task.Delay(100, cancellationToken);
        }
    }


    private async Task ProcessSingleFactAsync(CancellationToken cancellationToken)
    {
        try
        {
            var fact = await _catFactClient.GetRandomFactAsync(cancellationToken);

            if (fact is null)
            {
                _logger.LogWarning("Received an empty response from the API.");
                return;
            }

            await _fileService.AppendFactAsync(fact, _formatter, cancellationToken);
            _logger.LogDebug("Successfully appended data to the file.");
        }
        catch (IOException ex)
        {
            _logger.LogError("Cannot write to file. Details: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "An unexpected error occurred in the main process.");
        }
    }
}
