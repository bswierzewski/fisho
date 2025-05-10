# 🐟 Fishio — Aplikacja dla wędkarzy i organizatorów zawodów

**Fishio** to platforma webowa (zoptymalizowana pod kątem urządzeń mobilnych) służąca do organizacji i uczestnictwa w zawodach wędkarskich oraz prowadzenia osobistego rejestru połowów. Głównym celem jest zapewnienie intuicyjnego narzędzia, które ułatwi zarządzanie zawodami dla organizatorów i usprawni udział dla wędkarzy na różnych poziomach zaawansowania.

## 🎯 Główne Założenia

- **Publiczna Strona Powitalna (`/`):** Prezentuje aplikację i zachęca do rejestracji/logowania.
- **Dashboard (`/dashboard`):** Główny widok dla zalogowanego użytkownika, podsumowujący aktywności.
- **Nawigacja Mobilna:** Informacja o aplikacji z tytułem strony i ikoną użytkownika (Clerk). Dolny pasek nawigacyjny z głównymi sekcjami i centralnym przyciskiem akcji.
- **Informacje o Wersji:** Dostępne na dedykowanej stronie "O Aplikacji".

## 🛠️ Stack technologiczny

- **Backend**: .NET 9
- **Frontend**: Next.js (z React i TypeScript, App Router)
- **UI**: [shadcn/ui](https://ui.shadcn.dev/) + TailwindCSS
- **Zarządzanie Użytkownikami**: Clerk
- **Zarządzanie Stanem (Klient):** Zustand (globalny), React Query (stan serwera)
- **Formularze:** React Hook Form
- **Baza danych**: PostgreSQL (produkcja) / SQLite (dewelopment)
- **Hosting Zdjęć**: Zewnętrzny serwis (np. Cloudinary)

## ✅ Kluczowe Funkcjonalności (Zakres MVP/MVP+)

Poniższe funkcjonalności stanowią rdzeń aplikacji planowany na pierwszą wersję (MVP rozszerzone o dziennik i łowiska - MVP+).

### 🏆 Zarządzanie Zawodami

- Tworzenie zawodów przez zalogowanych użytkowników (stają się Organizatorami).
  - Definiowanie typu zawodów: Otwarte (każdy zalogowany może dołączyć) lub Zamknięte (uczestnicy dodawani tylko przez Organizatora).
  - Konfiguracja danych: Nazwa, Data/Czas rozpoczęcia i zakończenia, Lokalizacja (tekstowo), Regulamin (tekstowo), opcjonalne zdjęcie zawodów.
  - **Wybór Głównej Kategorii Punktacji:** Określenie, jak będzie liczony główny ranking (np. suma wag, suma długości).
  - **Wybór Kategorii Specjalnych:** Możliwość zdefiniowania dodatkowych kategorii (np. Największa Ryba, Najdłuższa Ryba, Najwięcej Sztuk).
  - Automatyczne generowanie unikalnego tokena (`resultsToken`) dla publicznego linku do wyników przy tworzeniu zawodów.
- Przeglądanie listy otwartych zawodów (z informacją o kategoriach).
- Przeglądanie listy "Moje Zawody" (zawody organizowane, sędziowane lub w których użytkownik bierze udział, z wizualnym rozróżnieniem statusu i informacją o kategoriach).
- Widok szczegółów zawodów (z informacją o kategoriach i linkiem do publicznych wyników).

### 👤 Zarządzanie Uczestnikami i Rolami w Zawodach

- **Model Hybrydowy Uczestnictwa:**
  - Możliwość samodzielnego dołączania do zawodów _Otwartych_ przez zalogowanych użytkowników.
  - Możliwość _ręcznego dodawania_ uczestników do zawodów przez Organizatora:
    - Dodawanie zarejestrowanych użytkowników (wyszukiwanie).
    - Dodawanie uczestników-gości bez konta (wymagane Imię/Nazwisko/Ksywka, opcjonalny unikalny identyfikator).
- **System Ról w Kontekście Zawodów:**
  - **Organizator (Zalogowany):** Tworzy zawody, zarządza uczestnikami, sędziami, kategoriami.
  - **Sędzia/Wagowy (Zalogowany):** Wyznaczony przez Organizatora; jedyna osoba mogąca rejestrować połowy _podczas zawodów_.
  - **Uczestnik (Zalogowany/Gość):** Bierze udział w zawodach.
- Możliwość wyznaczenia/usunięcia Sędziego (zarejestrowanego użytkownika) dla zawodów przez Organizatora.
- Możliwość usunięcia uczestnika przez Organizatora (przed startem zawodów).

### 🎣 Osobisty Dziennik Połowów (Funkcjonalność MVP+)

- Rejestracja indywidualnych połowów przez zalogowanego użytkownika (poza zawodami).
- Dodawanie zdjęcia złowionej ryby (wymagane, upload do Cloudinary).
- Zapisywanie danych połowu: Gatunek, Data/Czas, opcjonalnie Długość/Waga, Notatki.
- Opcjonalne powiązanie połowu z łowiskiem z bazy.
- Prywatny widok własnego dziennika połowów (responsywna siatka kart).

### 🗺️ Zarządzanie Łowiskami (Funkcjonalność MVP+)

- Przeglądanie listy łowisk.
- Widok szczegółów łowiska (Nazwa, Lokalizacja, opcjonalne zdjęcie, lista występujących gatunków ryb).
- Dodawanie/Edycja informacji o łowiskach: Nazwa, Lokalizacja (tekstowo), opcjonalne zdjęcie.
- **Zarządzanie Gatunkami Ryb na Łowisku:** Możliwość wyboru wielu predefiniowanych gatunków ryb (z dedykowanej tabeli `FishSpecies`) przypisanych do danego łowiska (relacja wiele-do-wielu).

### 📊 Wyniki i Rankingi (w Zawodach)

- Rejestracja złowionych ryb _wyłącznie przez Sędziego_ podczas trwania zawodów (wybór uczestnika, dane ryby, zdjęcie).
- **Publiczna Strona Wyników (`/results/[token]`):**
  - Dostępna przez unikalny, zawsze aktywny link.
  - Dynamicznie wyświetla zawartość w zależności od statusu zawodów:
    - **Zaplanowane (Upcoming):** Informacja o nadchodzących zawodach.
    - **Trwające (Ongoing):** Symulowane/rzeczywiste wyniki "na żywo", placeholder dla wykresu bieżącego rankingu.
    - **Zakończone (Finished):** Oficjalna tabela wyników (główna klasyfikacja), sekcje dla zwycięzców zdefiniowanych kategorii specjalnych, placeholdery dla dwóch wykresów (np. wizualizacja rankingu ogólnego i analiza kategorii).
- Wyświetlanie informacji o głównej kategorii punktacji i kategoriach specjalnych na stronie szczegółów zawodów.

### 📱 Interfejs i Użytkownicy

- **Nawigacja:**
  - Górny pasek (Navbar): Ikona informacji (z linkiem do "O Aplikacji") , dynamiczny tytuł strony, ikona użytkownika (Clerk `UserButton`).
  - Dolny pasek (BottomNavbar): Główne linki nawigacyjne (Start, Zawody, Dziennik, Łowiska) z aktywnym podświetleniem i wypełnieniem ikony, centralny, wystający przycisk akcji (kontekstowy).
- **Dashboard (`/dashboard`):** Spersonalizowany widok dla zalogowanego użytkownika z sekcjami kart (Moje Zawody, Ostatnie Połowy, Odkryj Otwarte Zawody, Lista Łowisk) w układzie poziomym, przewijanym, z kartami akcji. Karty "Moje Zawody" wizualnie rozróżniają status.
- Profil użytkownika (obsługiwany przez Clerk `<UserProfile />` na ścieżce catch-all `/profile/[[...user-profile]]`).
- Strona "O Aplikacji" (`/about`) z informacjami o wersji i buildzie.
- Uwierzytelnianie i zarządzanie sesją przez **Clerk**.
- Responsywny design z optymalizacją dla urządzeń mobilnych (Mobile-first).

### ⚙️ Backend i Infrastruktura

- Automatyczne generowanie informacji o wersji aplikacji (tag/SHA Git, data buildu) podczas procesu CI/CD (GitHub Actions).
- Dockerfile zoptymalizowany pod kątem Next.js (`standalone` output) dla wdrożeń.
- Podstawowa konfiguracja `docker-compose.yml` z możliwością automatycznych aktualizacji obrazu klienta (np. przez Watchtower).

## 🚀 Plany na Przyszłość (Poza MVP+)

Następujące funkcjonalności są planowane w dalszym rozwoju aplikacji, po wdrożeniu i zebraniu feedbacku z wersji MVP+:

- **Zaawansowane Zarządzanie Zawodami:**
  - Obsługa wpisowego i integracja z systemami płatności online.
  - Zarządzanie sektorami i stanowiskami na łowisku.
  - Możliwość definiowania przez organizatora niestandardowych pól dla zgłoszeń ryb (np. metoda połowu, przynęta).
  - Archiwum zawodów z zaawansowanym wyszukiwaniem i filtrowaniem.
  - Możliwość edycji zawodów przez Organizatora.
- **Komunikacja i Powiadomienia:**
  - Rozbudowany system powiadomień (np. Uczestnik -> Sędzia: "Złowiłem rybę"; Sędzia -> Uczestnicy: "Nowy wpis w rankingu"; o starcie zawodów). Panel powiadomień w Navbarze.
  - Moduł komunikacji wewnętrznej (czat w ramach zawodów, ogłoszenia).
- **Statystyki i Analizy:**
  - Szczegółowe statystyki i analizy połowów (indywidualne i zbiorcze).
  - Implementacja rzeczywistych wykresów na stronie wyników.
  - Zaawansowane opcje filtrowania i sortowania danych na listach.
- **Społeczność i Reputacja:**
  - System ocen i reputacji organizatorów oraz łowisk.
  - (Potencjalnie) Rozbudowane profile publiczne, listy znajomych, możliwość udostępniania połowów.
- **Administracja i Narzędzia:**
  - Rozbudowany Panel Administracyjny (zarządzanie użytkownikami, globalne zarządzanie łowiskami, gatunkami ryb, zawodami, moderacja).
  - Import/Eksport danych (np. lista uczestników w CSV, wyniki).
- **Lokalizacja:**
  - Wsparcie dla wielu języków (i18n).
- **Zaawansowane Zarządzanie Łowiskami:**
  - Integracja z mapami interaktywnymi, dodawanie współrzędnych, rysowanie granic łowisk.
  - Możliwość dodawania przez użytkowników zdjęć łowisk, komentarzy.
- **Ulepszenia UX/UI:**
  - Implementacja menu bocznego (sidebar/drawer) otwieranego ikoną hamburgera.
  - Bardziej zaawansowane komponenty wyboru (np. MultiSelect dla gatunków ryb, kategorii specjalnych).

---

> Projekt przygotowany przez [Bartosz Świerzewski]
> Kontakt: [swierzewski.bartosz@gmail.com]
