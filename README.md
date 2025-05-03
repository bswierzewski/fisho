# 🐟 Fisho — Aplikacja dla wędkarzy i organizatorów zawodów

**Fisho** to platforma webowa (zoptymalizowana pod kątem urządzeń mobilnych) służąca do organizacji i uczestnictwa w zawodach wędkarskich oraz prowadzenia osobistego rejestru połowów. Głównym celem jest zapewnienie intuicyjnego narzędzia, które ułatwi zarządzanie zawodami dla organizatorów i usprawni udział dla wędkarzy na różnych poziomach zaawansowania.

## 🛠️ Stack technologiczny

- **Backend**: .NET 9
- **Frontend**: Next.js (z React i TypeScript)
- **UI**: [shadcn/ui](https://ui.shadcn.dev/) + TailwindCSS
- **Zarządzanie Użytkownikami**: Clerk
- **Baza danych**: PostgreSQL / SQLite (w zależności od środowiska)
- **Hosting Zdjęć**: Zewnętrzny serwis (np. Cloudinary)

## ✅ Kluczowe Funkcjonalności (Zakres MVP/MVP+)

Poniższe funkcjonalności stanowią rdzeń aplikacji planowany na pierwszą wersję (MVP rozszerzone o dziennik i łowiska - MVP+).

### 🏆 Zarządzanie Zawodami

- Tworzenie zawodów przez zalogowanych użytkowników (stają się Organizatorami).
- Definiowanie typu zawodów: Otwarte (każdy zalogowany może dołączyć) lub Zamknięte (uczestnicy dodawani tylko przez Organizatora).
- Konfiguracja podstawowych danych zawodów: Nazwa, Data/Czas rozpoczęcia i zakończenia, Lokalizacja (tekstowo), Regulamin (tekstowo).
- Przeglądanie listy otwartych zawodów (dla zalogowanych).
- Przeglądanie listy "Moje Zawody" (zawody organizowane, sędziowane lub w których bierze się udział).
- Widok szczegółów zawodów.

### 👤 Zarządzanie Uczestnikami i Rolami

- **Model Hybrydowy:**
  - Możliwość samodzielnego dołączania do zawodów _Otwartych_ przez zalogowanych użytkowników.
  - Możliwość _ręcznego dodawania_ uczestników do zawodów przez Organizatora:
    - Dodawanie zarejestrowanych użytkowników (wyszukiwanie).
    - Dodawanie uczestników-gości bez konta (wymagane Imię/Nazwisko/Ksywka, opcjonalny unikalny identyfikator np. nr telefonu/startowy).
- **System Ról:**
  - **Organizator (Zalogowany):** Tworzy zawody, zarządza uczestnikami i sędziami.
  - **Sędzia/Wagowy (Zalogowany):** Wyznaczony przez Organizatora; jedyna osoba mogąca rejestrować połowy _podczas zawodów_.
  - **Uczestnik (Zalogowany/Gość):** Bierze udział w zawodach.
  - **Użytkownik (Zalogowany):** Może przeglądać, tworzyć zawody, prowadzić dziennik.
  - _(Admin - rola dorozumiana dla zarządzania platformą)_.
- Możliwość wyznaczenia/usunięcia Sędziego (zarejestrowanego użytkownika) dla zawodów przez Organizatora.
- Możliwość usunięcia uczestnika przez Organizatora (przed startem zawodów).

### 🎣 Osobisty Dziennik Połowów (Funkcjonalność MVP+)

- Rejestracja indywidualnych połowów przez zalogowanego użytkownika (poza zawodami).
- Dodawanie zdjęcia złowionej ryby (wymagane, upload do Cloudinary).
- Zapisywanie danych połowu: Gatunek, Data/Czas, opcjonalnie Długość/Waga, Notatki.
- Opcjonalne powiązanie połowu z łowiskiem z bazy.
- Prywatny widok własnego dziennika połowów (galeria/lista).

### 🗺️ Zarządzanie Łowiskami (Funkcjonalność MVP+)

- Przeglądanie listy łowisk dodanych do systemu.
- Widok szczegółów łowiska (Nazwa, Lokalizacja, zdefiniowane gatunki).
- Dodawanie/Edycja informacji o łowiskach przez zalogowanych użytkowników (lub tylko adminów na start): Nazwa, Lokalizacja (tekstowo), Występujące gatunki (tekstowo).

### 📊 Wyniki i Rankingi (w Zawodach)

- Rejestracja złowionych ryb _wyłącznie przez Sędziego_ podczas trwania zawodów (wybór uczestnika z listy, dane ryby, zdjęcie).
- Automatyczne generowanie rankingu zawodów (na żywo i końcowego) na podstawie zgłoszeń Sędziego i prostego kryterium (np. suma długości/wagi).
- Wyświetlanie Tablicy Wyników na stronie szczegółów zawodów.
- Generowanie przez Organizatora unikalnego linku do **publicznej strony wyników** (read-only) dla niezalogowanych uczestników/gości.

### 📱 Interfejs i Użytkownicy

- Profil użytkownika (minimalny: nazwa, wylogowanie, dostęp do historii zawodów i dziennika).
- Uwierzytelnianie i zarządzanie sesją przez **Clerk**.
- Responsywny design z optymalizacją dla urządzeń mobilnych (Mobile-first).

## 🚀 Plany na Przyszłość (Poza MVP+)

Następujące funkcjonalności są planowane w dalszym rozwoju aplikacji, po wdrożeniu i zebraniu feedbacku z wersji MVP+:

- **Zaawansowane Zarządzanie Zawodami:**
  - Obsługa wpisowego i integracja z systemami płatności online.
  - Zarządzanie sektorami i stanowiskami na łowisku.
  - Obsługa różnych, konfigurowalnych rodzajów punktacji i regulaminów.
  - Możliwość definiowania dodatkowych, niestandardowych pól dla zawodów.
  - Archiwum zawodów z zaawansowanym wyszukiwaniem i filtrowaniem.
- **Komunikacja i Powiadomienia:**
  - System powiadomień (np. dla organizatora/sędziego o złowionej rybie, o starcie zawodów).
  - Moduł komunikacji wewnętrznej (czat w ramach zawodów, ogłoszenia).
- **Statystyki i Analizy:**
  - Szczegółowe statystyki i analizy połowów (indywidualne i zbiorcze).
  - Zaawansowane opcje filtrowania i sortowania danych.
- **Społeczność i Reputacja:**
  - System ocen i reputacji organizatorów oraz łowisk.
  * (Potencjalnie) Rozbudowane profile publiczne, listy znajomych.
- **Administracja i Narzędzia:**
  - Rozbudowany Panel Administracyjny (zarządzanie użytkownikami, globalne zarządzanie łowiskami, zawodami, moderacja).
  - Import/Eksport danych (np. lista uczestników w CSV, wyniki).
- **Lokalizacja:**
  - Wsparcie dla wielu języków (i18n).
- **Zaawansowane Zarządzanie Łowiskami:**
  - Integracja z mapami, dodawanie współrzędnych, zdjęć łowisk.
  - Bardziej ustrukturyzowana baza gatunków ryb.

---

> Projekt przygotowany przez [Bartosz Świerzewski]
> Kontakt: [swierzewski.bartosz@gmail.com]
