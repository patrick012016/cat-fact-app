using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using CatFactApp.Clients;
using CatFactApp.Configuration;
using CatFactApp.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using Xunit;

namespace CatFactApp.Tests.Clients;

[ExcludeFromCodeCoverage]
public class CatFactClientTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly IOptions<CatFactConfig> _options;
    private readonly string _expectedRequestUrl;

    public CatFactClientTests()
    {
        _mockHttp = new MockHttpMessageHandler();

        var config = new CatFactConfig
        {
            ApiUrl = "https://catfact.ninja/",
            RequestUrl = "fact"
        };
        _options = Options.Create(config);

        _expectedRequestUrl = $"{config.ApiUrl}{config.RequestUrl}";
    }

    [Fact]
    public async Task GetRandomFactAsync_WhenApiReturnsSuccess_ShouldReturnModel()
    {
        // Arrange
        var expectedFact = "Test fact";
        var expectedLength = 9;
        var apiResponse = new CatFactResponse(expectedFact, expectedLength);
        var jsonResponse = JsonSerializer.Serialize(apiResponse);

        _mockHttp.Expect(HttpMethod.Get, _expectedRequestUrl)
            .Respond(HttpStatusCode.OK, "application/json", jsonResponse);
        var sut = CreateSut();

        // Act
        var result = await sut.GetRandomFactAsync(CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedFact, result.Fact);
        Assert.Equal(expectedLength, result.Length);
        _mockHttp.VerifyNoOutstandingExpectation();
    }

    [Theory]
    [InlineData("", 10)]
    [InlineData("   ", 10)]
    [InlineData("Test fact", 0)]
    [InlineData("Test fact", -15)]
    public async Task GetRandomFactAsync_WhenApiReturnsInvalidData_ShouldReturnNull(string invalidFact,
        int invalidLength)
    {
        // Arrange
        var apiResponse = new CatFactResponse(invalidFact, invalidLength);
        var jsonResponse = JsonSerializer.Serialize(apiResponse);

        _mockHttp.Expect(HttpMethod.Get, _expectedRequestUrl)
            .Respond(HttpStatusCode.OK, "application/json", jsonResponse);
        var sut = CreateSut();

        // Act
        var result = await sut.GetRandomFactAsync(CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRandomFactAsync_WhenRequestIsCanceled_ShouldReturnNull()
    {
        // Arrange
        var sut = CreateSut();
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        // Act
        var result = await sut.GetRandomFactAsync(cancellationTokenSource.Token);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetRandomFactAsync_WhenApiReturns500_ShouldReturnNull()
    {
        // Arrange
        _mockHttp.Expect(HttpMethod.Get, _expectedRequestUrl)
            .Respond(HttpStatusCode.InternalServerError);
        var sut = CreateSut();

        // Act
        var result = await sut.GetRandomFactAsync(CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetRandomFactAsync_WhenApiReturnsInvalidJson_ShouldReturnNull()
    {
        // Arrange
        _mockHttp.Expect(HttpMethod.Get, _expectedRequestUrl)
            .Respond("application/json", "<h1>Example HTML, not JSON media</h1>");
        var sut = CreateSut();

        // Act
        var result = await sut.GetRandomFactAsync(CancellationToken.None);

        // Assert
        Assert.Null(result);
        _mockHttp.VerifyNoOutstandingExpectation();
    }

    private CatFactClient CreateSut()
    {
        var httpClient = _mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri(_options.Value.ApiUrl);

        return new CatFactClient(httpClient, _options, NullLogger<CatFactClient>.Instance);
    }
}
