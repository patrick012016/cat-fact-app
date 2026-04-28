FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env

WORKDIR /app

COPY ["src/CatFact.ConsoleApp/CatFact.ConsoleApp.csproj", "src/CatFact.ConsoleApp/"]

RUN dotnet restore "src/CatFact.ConsoleApp/CatFact.ConsoleApp.csproj"

COPY [".", "./"]

RUN dotnet publish src/CatFact.ConsoleApp/CatFact.ConsoleApp.csproj -c Release -o /app/out --no-restore /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled-extra

WORKDIR /app

RUN chown $APP_UID:$APP_UID /app

ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build-env --chown=$APP_UID:$APP_UID ["/app/out", "./"]

USER $APP_UID

ENTRYPOINT ["dotnet", "CatFact.ConsoleApp.dll"]
