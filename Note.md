# Commands used to setup docker database

## Run SQL Server Container

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Pa$$w0rd2022" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

## Enable User Secrets

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Kanban" "Server=localhost;Database=msdb;User Id=sa;Password=Pa}w0rd2022;Trusted_Connection=False;Encrypt=False"
dotnet add package Microsoft.Extensions.Configuration.UserSecrets
```

//TODO: Add migration



