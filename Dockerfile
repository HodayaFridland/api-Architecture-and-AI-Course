# =============================================================================
# Stage 1 – BUILD
# Uses the full .NET SDK image to restore, build and publish the application.
# This stage is discarded afterwards; only its /app/publish output is kept.
# =============================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file first so 'dotnet restore' is cached as its own layer.
# The restore layer is only invalidated when the .csproj changes, not on every
# source-code change – this keeps rebuilds fast.
COPY api-project/api-project.csproj api-project/
RUN dotnet restore api-project/api-project.csproj

# Copy the rest of the source and publish a Release build.
# --no-restore reuses the packages downloaded in the previous step.
COPY api-project/ api-project/
WORKDIR /src/api-project
RUN dotnet publish api-project.csproj -c Release -o /app/publish --no-restore

# =============================================================================
# Stage 2 – RUNTIME
# Uses the much smaller ASP.NET runtime image (no SDK / compiler tools).
# Only the compiled output from Stage 1 is copied here.
# =============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Listen on HTTP port 8080 inside the container.
# HTTPS termination is handled externally (reverse proxy / load balancer).
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Copy the published output from the build stage.
COPY --from=build /app/publish .

# Start the API.
ENTRYPOINT ["dotnet", "api-project.dll"]
