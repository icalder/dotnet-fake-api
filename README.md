# Generic API

## Setup

See [Microsoft guide](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-9.0&tabs=visual-studio-code)

### Initial Scaffold

```powershell
dotnet new webapi --use-controllers
dotnet run
```

### Add reference to apispec project

```powershell
dotnet add reference ../fake-api-spec/tsp-output/server/aspnet/ServiceProject.csproj
```

### Add EF in-memory DB

```powershell
dotnet add package Microsoft.EntityFrameworkCore.InMemory
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet build

# NB command below creates .config/dotnet-tools.json
dotnet tool install dotnet-aspnet-codegenerator --allow-roll-forward --create-manifest-if-needed

dotnet aspnet-codegenerator controller -name TodoItemsController -async -api -m TodoItem -dc TodoContext -outDir Controllers
```