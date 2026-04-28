using System.Text.Json.Serialization;

namespace CatFactApp.Models;

public record CatFactResponse(
    [property: JsonPropertyName("fact")] string Fact,
    [property: JsonPropertyName("length")] int Length
);
