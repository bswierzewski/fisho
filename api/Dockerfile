# Etap 1: Budowanie aplikacji
# Użyj oficjalnego obrazu .NET SDK jako obrazu bazowego do budowania
# Wybierz wersję SDK pasującą do Twojego projektu (np. 8.0 dla .NET 8)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

# Ustaw katalog roboczy w kontenerze
WORKDIR /app

# Kopiuj pliki .csproj i .sln i przywróć zależności jako osobną warstwę
# To wykorzystuje cache warstw Dockera, aby przyspieszyć budowanie,
# jeśli pliki projektu się nie zmieniły.
# Dostosuj ścieżki, jeśli Twoja struktura folderów jest inna.
COPY src/Domain/Domain.csproj ./src/Domain/
COPY src/Application/Application.csproj ./src/Application/
COPY src/Infrastructure/Infrastructure.csproj ./src/Infrastructure/
COPY src/API/API.csproj ./src/API/
COPY src/Fishio.sln ./src/

# Przywróć zależności dla całej solucji
RUN dotnet restore ./src/Fishio.sln

# Kopiuj resztę kodu źródłowego aplikacji
COPY src/ ./src/

# Opcjonalnie: Jeśli masz plik buildinfo.json generowany przez CI/CD i umieszczony w src/Fishio.Api
# Możesz go skopiować tutaj, jeśli nie jest częścią repozytorium Git.
# Jeśli jest w repo, to został już skopiowany z `COPY src/ ./src/`
# COPY src/Fishio.Api/buildinfo.json ./src/Fishio.Api/buildinfo.json

# Publikuj aplikację, tworząc zoptymalizowane pliki do uruchomienia
# Użyj konfiguracji Release
# --no-restore, ponieważ zależności zostały już przywrócone
# -o /app/publish określa katalog wyjściowy dla opublikowanych plików
RUN dotnet publish ./src/API/API.csproj -c Release --no-restore -o /app/publish

# Etap 2: Tworzenie finalnego obrazu uruchomieniowego
# Użyj oficjalnego obrazu ASP.NET Core Runtime jako obrazu bazowego
# Jest on znacznie mniejszy niż obraz SDK
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-env

# Ustaw katalog roboczy
WORKDIR /app

# Kopiuj opublikowane pliki z etapu budowania do finalnego obrazu
COPY --from=build-env /app/publish .

# Ustaw użytkownika nie-root dla bezpieczeństwa (opcjonalnie, ale zalecane)
# Najpierw musisz upewnić się, że użytkownik istnieje w obrazie bazowym lub go utworzyć.
# Obrazy aspnet często mają użytkownika 'app' lub 'aspnet'.
# RUN useradd -m appuser
# USER appuser
# Jeśli obraz bazowy ma użytkownika 'app':
USER app

# Zdefiniuj zmienne środowiskowe, które mogą być nadpisane przy uruchomieniu kontenera
# Te wartości są tylko domyślne, jeśli nie zostaną przekazane z zewnątrz.
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:7000
# Możesz dodać inne domyślne ENV, np. dla portu HTTPS, jeśli go używasz
# ENV ASPNETCORE_HTTPS_PORTS=8081
# ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
# ENV ASPNETCORE_Kestrel__Certificates__Default__Password=your_password_for_pfx

# Opcjonalnie: Jeśli używasz pliku buildinfo.json i jest on w /app/publish, to już tu jest.

# Ujawnij port, na którym aplikacja nasłuchuje wewnątrz kontenera
EXPOSE 7000
# EXPOSE 8081 # Jeśli używasz HTTPS

# Definiuje domyślną komendę uruchamianą przy starcie kontenera
# Uruchamia Twoją aplikację .NET DLL
ENTRYPOINT ["dotnet", "Fishio.Api.dll"]
