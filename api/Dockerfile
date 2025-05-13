# api/Dockerfile

# Etap 1: Budowanie aplikacji
# Użyj oficjalnego obrazu .NET SDK. Zmień na 9.0, gdy będzie stabilny i dostępny.
# Na razie używam 8.0 jako przykładu stabilnego SDK.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
# Jeśli chcesz użyć .NET 9 Preview: FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build-env

WORKDIR /app

# Kontekst budowania to folder 'api' w repozytorium.
# Dockerfile jest w 'api/Dockerfile'.
# Plik solucji 'Fishio.sln' jest w 'api/Fishio.sln'.
# Projekty są w 'api/src/NazwaProjektu/NazwaProjektu.csproj'.

# Kopiuj plik solucji i pliki .csproj
# Ścieżki są relatywne do kontekstu ('api/').
COPY Fishio.sln ./
COPY src/Domain/Domain.csproj ./src/Domain/
COPY src/Application/Application.csproj ./src/Application/
COPY src/Infrastructure/Infrastructure.csproj ./src/Infrastructure/
COPY src/API/API.csproj ./src/API/

# Przywróć zależności dla całej solucji
# Plik solucji jest teraz w głównym katalogu /app w kontenerze
RUN dotnet restore ./Fishio.sln

# Kopiuj całą zawartość folderu 'src' (gdzie jest kod projektów)
# do podfolderu 'src' w katalogu roboczym kontenera (/app/src).
COPY src/ ./src/
# Po tym kroku, w kontenerze będziesz miał:
# /app/Fishio.sln
# /app/src/API/
# /app/src/Domain/
# /app/src/Application/
# /app/src/Infrastructure/

# Publikuj aplikację API
# Ścieżka do .csproj projektu API jest teraz src/API/API.csproj
RUN dotnet publish ./src/API/API.csproj -c Release --no-restore -o /app/publish

# Etap 2: Tworzenie finalnego obrazu uruchomieniowego
# Użyj odpowiedniej wersji środowiska uruchomieniowego ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime-env
# Jeśli używasz .NET 9 Preview: FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime-env

WORKDIR /app
COPY --from=build-env /app/publish .

USER app

# Użycie build-args do ustawienia zmiennych środowiskowych (opcjonalne)
ARG BUILD_INFO_GIT_SHA="undefined-dockerfile"
ARG BUILD_INFO_TIMESTAMP="undefined-dockerfile"
ARG BUILD_INFO_VERSION_TAG="undefined-dockerfile"

ENV APP_VERSION_SHA=$BUILD_INFO_GIT_SHA
ENV APP_BUILD_TIMESTAMP=$BUILD_INFO_TIMESTAMP
ENV APP_VERSION_TAG=$BUILD_INFO_VERSION_TAG

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:7000

EXPOSE 7000

# Nazwa DLLki będzie prawdopodobnie API.dll, jeśli Twój projekt API nazywa się API.csproj
# Jeśli nazwa projektu to Fishio.Api.csproj, to DLLka będzie Fishio.Api.dll
ENTRYPOINT ["dotnet", "API.dll"]
