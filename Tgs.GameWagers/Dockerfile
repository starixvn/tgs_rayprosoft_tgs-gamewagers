FROM hub.lumigame.com/base/dotnet/core/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameWagers/Tgs.GameWagers.csproj", "Tgs.GameWagers/"]
COPY ["Tgs.GameWagers.Common/Tgs.GameWagers.Common.csproj", "Tgs.GameWagers.Common/"]
COPY ["Tgs.GameWagers.Core/Tgs.GameWagers.Core.csproj", "Tgs.GameWagers.Core/"]
COPY ["Tgs.GameWagers.Repositories/Tgs.GameWagers.Repositories.csproj", "Tgs.GameWagers.Repositories/"]
RUN dotnet restore "./Tgs.GameWagers/./Tgs.GameWagers.csproj"
COPY . .
WORKDIR "/src/Tgs.GameWagers"
RUN dotnet build "./Tgs.GameWagers.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameWagers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM hub.lumigame.com/base/dotnet/core/aspnet:8.0-alpine-24h1.1 AS base
USER app
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameWagers.dll"]