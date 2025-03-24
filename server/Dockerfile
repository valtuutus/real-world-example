FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Valtuutus.RealWorld.Api/Valtuutus.RealWorld.Api.csproj", "src/Valtuutus.RealWorld.Api/"]
COPY ["src/Valtuutus.RealWorld.ServiceDefaults/Valtuutus.RealWorld.ServiceDefaults.csproj", "src/Valtuutus.RealWorld.ServiceDefaults/"]
RUN dotnet restore "src/Valtuutus.RealWorld.Api/Valtuutus.RealWorld.Api.csproj"
COPY . .
WORKDIR "/src/src/Valtuutus.RealWorld.Api"
RUN dotnet build "Valtuutus.RealWorld.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Valtuutus.RealWorld.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Valtuutus.RealWorld.Api.dll"]
