FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ducktory/Ducktory.csproj", "Ducktory/"]
COPY ["DuckLibrary/DuckLibrary.csproj", "DuckLibrary/"]
RUN dotnet restore "Ducktory/Ducktory.csproj"
COPY . .
WORKDIR "/src/Ducktory"
RUN dotnet build "./Ducktory.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ducktory.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ducktory.dll"]
