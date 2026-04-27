using CatFactApp.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<CatFactConfig>()
    .BindConfiguration("CatFactConfig")
    .ValidateDataAnnotations()
    .ValidateOnStart();


using var host = builder.Build();
