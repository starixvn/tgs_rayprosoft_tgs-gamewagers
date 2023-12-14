#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameWagers.dll"]