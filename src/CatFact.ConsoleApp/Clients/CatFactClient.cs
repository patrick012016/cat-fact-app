using System.Net.Http.Json;
using System.Text.Json;
using CatFactApp.Configuration;
using CatFactApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CatFactApp.Clients;

public class CatFactClient : ICatFactClient
{
    private readonly HttpClient _httpClient;
    private readonly CatFactConfig _config;
    private readonly ILogger<CatFactClient> _logger;

    public CatFactClient(HttpClient httpClient, IOptions<CatFactConfig> config, ILogger<CatFactClient> logger)
    {
        _config = config.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CatFactModel?> GetRandomFactAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var responseDto =
                await _httpClient.GetFromJsonAsync<CatFactResponse>(_config.RequestUrl, cancellationToken);

            if (responseDto is null || string.IsNullOrWhiteSpace(responseDto.Fact) || responseDto.Length <= 0)
            {
                _logger.LogWarning("Unexpected empty data in successful API response.");
                return null;
            }

            return new CatFactModel(responseDto.Fact, responseDto.Length);
        }
        catch (Exception ex) when (ex is HttpRequestException or TimeoutException or JsonException)
        {
            _logger.LogWarning("Failed to retrieve data from API. Reason: {Reason}", ex.Message);
            return null;
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("The API request was canceled.");
            return null;
        }
    }
}
