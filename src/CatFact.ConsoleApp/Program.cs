using CatFactApp.Clients;
using CatFactApp.Configuration;
using CatFactApp.Formatters;
using CatFactApp.Models;
using CatFactApp.Runners;
using CatFactApp.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<CatFactConfig>()
    .BindConfiguration("CatFactConfig")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddSingleton<ITextFormatter<CatFactModel>, CatFactFormatter>();
builder.Services.AddTransient<AppRunner>();

builder.Services.AddHttpClient<ICatFactClient, CatFactClient>((serviceProvider, client) =>
{
    var config = serviceProvider.GetRequiredService<IOptions<CatFactConfig>>().Value;
    client.BaseAddress = new Uri(config.ApiUrl);

    client.Timeout = TimeSpan.FromSeconds(5);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

using var host = builder.Build();

var app = host.Services.GetRequiredService<AppRunner>();
await app.RunAsync();
