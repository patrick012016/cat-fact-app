# CatFactApp

A minimalist .NET 10 console application designed to fetch random cat facts from the `catfact.ninja` API and store them locally.
* **Controls**: Press **[SPACE]** to fetch a fact, or **[Q]** to quit.


## Table of contents

* [Configuration](#configuration)
* [How to run](#how-to-run)
* [License](#license)


## Configuration

The application is configured via `appsettings.json`.

```json
{
  "CatFactConfig": {
    "ApiUrl": "https://catfact.ninja/",
    "RequestUrl": "fact",
    "OutputFilePath": "cat_facts.txt"
  }
}
```
## How to run

### Clone the repository
```bash
git clone https://github.com/patrick012016/cat-fact-app.git
cd cat-fact-app
```

### Run locally

> [!IMPORTANT]
> To run this project locally, you must have the **.NET 10 SDK** installed on your machine.

To ensure the application correctly loads the configuration, navigate to the project folder:
```bash
cd src/CatFact.ConsoleApp
dotnet run
```

### Run with docker
 
Execute these commands from the **root directory** (where the `.sln` file is):
```bash

docker build -f src/CatFact.ConsoleApp/Dockerfile -t catfactapp .
docker run -it --rm catfactapp
```

## License

This project is licensed under the Apache 2.0 License - see the [LICENSE](LICENSE) file for details.
