# DAG Scan

A blockchain explorer for Constellation Network.

## Frontend Web Application

- [Nextjs 14](https://nextjs.org/)
- [shadcn UI components](https://ui.shadcn.com/)

External Dependencies:

- [Block Explorer API](https://be-mainnet.constellationnetwork.io/): native DAG block explorer API

Run Locally:

1. Open a command prompt in the directory `source/client`
2. Run the command `npm install`
3. Verify the `.env` file and change the API URLs if needed
4. Run the command `npm run dev`

## Backend Application

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Microsoft SQL Server (using entity framework, so other database technologies can be used with minimal configuration changes)
- Container Apps (Docker)

External Dependencies:

- [IP Geolocation API](https://ip-api.com/): used to get the geolocation of validator nodes
- [Block Explorer API](https://be-mainnet.constellationnetwork.io/): native DAG block explorer API
- Metagraph API Endpoints

Run Locally:

1. Make sure the [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) SDK is installed
2. If you only plan to run the worker/database, you can skip steps 3 & 4 (as these are needed for hosting the API locally)
3. Create [local development certificates](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-8.0) using `dotnet dev-certs` CLI
4. Create a new file `docker-compose.override.local.yml` next to the existing docker-compose file with the reference to the certificates, example:

```docker
services:
  dagscan.api:
    environment:
      - Kestrel__Certificates__Default__Path=/app/.aspnet/https/aspnetcore-localhost.pfx
      - ASPNETCORE_Kestrel__Certificates__Default__Password=mypassword
    volumes:
      - ~/.microsoft/usersecrets:/app/.microsoft/usersecrets:ro
      - ~/.aspnet/dev-certs/https:/app/.aspnet/https:ro
```

5. Open a command prompt in the directory `source/services` and run the docker-compose command (make sure the docker service is running):

```docker
docker-compose -f docker-compose.yml -f docker-compose.override.local.yml up
```

- You can navigate to `https://localhost:60016/swagger/index.html` to view and test the api endpoints
- You can navigate to `https://localhost:60016/hangfire/` to view and trigger scheduled jobs for the worker

## Deployment

The apps are hosted on Microsoft Azure and are released through GitHub Actions.
