# Base image for container execution
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5171

# SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Restore dependencies
COPY ["ComprasVentas.csproj", "./"]
RUN dotnet restore "ComprasVentas.csproj"

# Copy source code and build
COPY . .
WORKDIR "/src"
RUN dotnet build "ComprasVentas.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ComprasVentas.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ComprasVentas.dll"]

