# Fishio - API Backend

## Przegląd
Jest to backend API dla systemu Fishio, zbudowany przy użyciu .NET 9. Zapewnia solidną i skalowalną infrastrukturę backendową do zarządzania zawodami wędkarskimi, dziennikami użytkowników i danymi łowisk.

## Stos Technologiczny
- **Wersja .NET**: 9.0
- **Architektura**: Clean Architecture
- **Baza danych**: PostgreSQL
- **Uwierzytelnianie**: Tokeny JWT
- **Dokumentacja**: Swagger/OpenAPI
- **Testy**: NUnit
- **ORM**: Entity Framework Core

## Struktura Projektu
```
src/
├── API/                 # Projekt API - kontrolery, middleware, konfiguracja
├── Application/         # Warstwa aplikacji - logika biznesowa, DTOs, interfejsy
├── Domain/             # Warstwa domeny - encje, obiekty wartości, logika domenowa
└── Infrastructure/     # Warstwa infrastruktury - baza danych, usługi zewnętrzne
```

## Główne Funkcje
- Endpointy REST API
- Implementacja Clean Architecture
- Entity Framework Core z PostgreSQL
- Uwierzytelnianie i Autoryzacja
- Dokumentacja Swagger
- Logowanie i monitoring
- Middleware obsługi błędów
- Migracje bazy danych
- Testy jednostkowe i integracyjne

## Rozpoczęcie Pracy

### Wymagania Wstępne
- .NET SDK 9.0 lub nowszy
- PostgreSQL
- Visual Studio 2022 lub VS Code

### Instalacja
1. Sklonuj repozytorium
2. Zaktualizuj connection string w `appsettings.json`
3. Uruchom migracje bazy danych:
```bash
dotnet ef migrations add "Init" --project src/Infrastructure --startup-project src/API --output-dir Persistence/Migrations
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

### Uruchomienie Aplikacji
```bash
cd src/API
dotnet run
```

API będzie dostępne pod adresem `https://localhost:7000`, a dokumentacja Swagger pod `https://localhost:7000/swagger`

## Migracje Bazy Danych
- Dodaj nową migrację:
```bash
dotnet ef migrations add "NazwaMigracji" --project src/Infrastructure --startup-project src/API --output-dir Persistence/Migrations
```
- Usuń ostatnią migrację:
```bash
dotnet ef migrations remove --project src/Infrastructure --startup-project src/API
```
- Zaktualizuj bazę danych:
```bash
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

## Dokumentacja API
API jest dokumentowane przy użyciu Swagger/OpenAPI. Podczas uruchomionej aplikacji, odwiedź `/swagger`, aby zobaczyć interaktywną dokumentację.

## Wytyczne Rozwoju
- Przestrzegaj zasad Clean Architecture
- Używaj wzorca CQRS dla operacji
- Implementuj odpowiednią walidację
- Pisz testy jednostkowe dla logiki biznesowej
- Używaj znaczących komunikatów w commitach
- Dokumentuj endpointy API
- Odpowiednio obsługuj błędy

## Testowanie
```bash
dotnet test
```

## Współpraca
1. Utwórz nową gałąź dla swojej funkcji
2. Napisz testy dla nowej funkcjonalności
3. Upewnij się, że wszystkie testy przechodzą
4. Wyślij pull request

## Dodatkowe Zasoby
- [Dokumentacja .NET](https://docs.microsoft.com/pl-pl/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/pl-pl/ef/core/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Wytyczne REST API](https://github.com/microsoft/api-guidelines)