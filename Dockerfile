# ============================================================
# Dockerfile — Florist Shop Web App
# Multi-stage build: SDK for build, runtime for execution.
# ============================================================

# --- Stage 1: build ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/FloristShop.Web/FloristShop.Web.csproj FloristShop.Web/
RUN dotnet restore FloristShop.Web/FloristShop.Web.csproj

COPY src/FloristShop.Web/ FloristShop.Web/
RUN dotnet publish FloristShop.Web/FloristShop.Web.csproj \
    -c Release -o /app/publish --no-restore

# --- Stage 2: runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create upload directories.
# On Railway: mount a persistent Volume at /app/wwwroot/uploads
RUN mkdir -p wwwroot/uploads/bouquets \
             wwwroot/uploads/rental \
             wwwroot/uploads/gallery

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "FloristShop.Web.dll"]
