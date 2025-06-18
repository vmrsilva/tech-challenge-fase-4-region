#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
#COPY ["TechChallenge.Region.Api.csproj", "TechChallenge.Region.Api/"]
COPY ["TechChallenge.Region.Api/TechChallenge.Region.Api.csproj", "TechChallenge.Region.Api/"]
COPY ["TechChallenge.Region.Domain/TechChallenge.Region.Domain.csproj", "TechChallenge.Region.Domain/"]
COPY ["TechChallenge.Region.Infrastructure/TechChallenge.Region.Infrastructure.csproj", "TechChallenge.Region.Infrastructure/"]
COPY ["TechChallenge.Region.IoC/TechChallenge.Region.IoC.csproj", "TechChallenge.Region.IoC/"]
RUN dotnet restore "./TechChallenge.Region.Api/TechChallenge.Region.Api.csproj"
COPY . .
WORKDIR "/src/TechChallenge.Region.Api"
RUN dotnet build "./TechChallenge.Region.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallenge.Region.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallenge.Region.Api.dll"]