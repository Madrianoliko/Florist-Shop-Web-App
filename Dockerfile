# ============================================================
# Dockerfile — Klimaty na kwiaty
# Multi-stage build: SDK do kompilacji, runtime do uruchomienia.
# ============================================================

# --- Etap 1: build ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Kopiujemy najpierw tylko .csproj — warstwa cache dla restore
COPY src/FloristShop.Web/FloristShop.Web.csproj FloristShop.Web/
RUN dotnet restore FloristShop.Web/FloristShop.Web.csproj

# Teraz reszta kodu
COPY src/FloristShop.Web/ FloristShop.Web/
RUN dotnet publish FloristShop.Web/FloristShop.Web.csproj \
    -c Release -o /app/publish --no-restore

# --- Etap 2: runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Katalogi na przesyłane zdjęcia (na Railway podepnij Volume pod /app/wwwroot/images)
RUN mkdir -p wwwroot/images/bouquets \
             wwwroot/images/rental \
             wwwroot/images/gallery

COPY --from=build /app/publish .

# Railway ustawia PORT dynamicznie — aplikacja sama go odczytuje w Program.cs
EXPOSE 8080

ENTRYPOINT ["dotnet", "FloristShop.Web.dll"]
