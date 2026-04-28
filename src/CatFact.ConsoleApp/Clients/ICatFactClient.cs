using CatFactApp.Models;

namespace CatFactApp.Clients;

public interface ICatFactClient
{
    Task<CatFactModel?> GetRandomFactAsync(CancellationToken cancellationToken = default);
}
