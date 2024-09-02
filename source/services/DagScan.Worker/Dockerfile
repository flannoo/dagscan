FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DagScan.Worker/DagScan.Worker.csproj", "DagScan.Worker/"]
RUN dotnet restore "DagScan.Worker/DagScan.Worker.csproj"
COPY . .
WORKDIR "/src/DagScan.Worker"
RUN dotnet build "DagScan.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DagScan.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DagScan.Worker.dll"]
