FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY server/NuGet.config server/NuGet.config
COPY server/GenesysForge.sln server/GenesysForge.sln
COPY server/src/GenesysForge.Domain/GenesysForge.Domain.csproj server/src/GenesysForge.Domain/
COPY server/src/GenesysForge.Contracts/GenesysForge.Contracts.csproj server/src/GenesysForge.Contracts/
COPY server/src/GenesysForge.Application/GenesysForge.Application.csproj server/src/GenesysForge.Application/
COPY server/src/GenesysForge.Infrastructure/GenesysForge.Infrastructure.csproj server/src/GenesysForge.Infrastructure/
COPY server/src/GenesysForge.WebApi/GenesysForge.WebApi.csproj server/src/GenesysForge.WebApi/
RUN dotnet restore server/src/GenesysForge.WebApi/GenesysForge.WebApi.csproj

COPY server server
RUN dotnet publish server/src/GenesysForge.WebApi/GenesysForge.WebApi.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

RUN apk add --no-cache curl

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GenesysForge.WebApi.dll"]
