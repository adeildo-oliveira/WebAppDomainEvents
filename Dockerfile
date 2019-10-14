FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["WebApi.DomainEvents/WebApi.DomainEvents.csproj", "WebApi.DomainEvents/"]
RUN dotnet restore "WebApi.DomainEvents/WebApi.DomainEvents.csproj"
COPY . .
WORKDIR "/src/WebApi.DomainEvents"
RUN dotnet build "WebApi.DomainEvents.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.DomainEvents.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.DomainEvents.dll"]