FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY hello-city-server.sln ./
COPY HelloCity.Api/HelloCity.Api.csproj HelloCity.Api/
COPY HelloCity.Models/HelloCity.Models.csproj HelloCity.Models/
COPY HelloCity.Repository/HelloCity.Repository.csproj HelloCity.Repository/
COPY HelloCity.Services/HelloCity.Services.csproj HelloCity.Services/
COPY HelloCity.IRepository/HelloCity.IRepository.csproj HelloCity.IRepository/
COPY HelloCity.IServices/HelloCity.IServices.csproj HelloCity.IServices/

RUN dotnet restore "HelloCity.Api/HelloCity.Api.csproj"

COPY . .
WORKDIR "/src/HelloCity.Api"
RUN dotnet build "HelloCity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HelloCity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloCity.Api.dll"]
