using System.Diagnostics.CodeAnalysis;
using CatFactApp.Formatters;
using CatFactApp.Models;
using Xunit;

namespace CatFactApp.Tests.Unit.Formatters;

[ExcludeFromCodeCoverage]
public class CatFactFormatterTests
{
    [Fact]
    public void Format_ShouldReturnCorrectlyFormattedString()
    {
        // Arrange
        var formatter = new CatFactFormatter();
        var model = new CatFactModel("Cats sleep 70% of their lives.", 30);

        // Act
        var result = formatter.Format(model);

        // Assert
        Assert.Contains("Fact: Cats sleep 70% of their lives. (Length: 30)", result);
        Assert.EndsWith(Environment.NewLine, result);
    }
}
