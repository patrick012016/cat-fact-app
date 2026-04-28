using CatFactApp.Formatters;

namespace CatFactApp.Storage;

public interface IFileService
{
    Task AppendFactAsync<T>(T model, ITextFormatter<T> formatter, CancellationToken cancellationToken = default);
}
