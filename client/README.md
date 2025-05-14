# 🎣 Fishio - Aplikacja Frontendowa

## 📋 Przegląd
Jest to aplikacja frontendowa dla systemu Fishio - kompleksowego systemu zarządzania zawodami wędkarskimi i dziennika połowów. Zbudowana przy użyciu Next.js 14, aplikacja zapewnia nowoczesny i responsywny interfejs użytkownika do zarządzania zawodami wędkarskimi, osobistymi dziennikami połowów i informacjami o łowiskach.

## ⭐ Główne Funkcje
- Uwierzytelnianie i autoryzacja użytkowników (przy użyciu Clerk)
- System zarządzania zawodami
- Osobisty dziennik połowów
- Baza danych łowisk
- Publiczne udostępnianie wyników
- Aktualizacje i powiadomienia w czasie rzeczywistym

## 🛠️ Stos Technologiczny
- **Framework**: Next.js 14 z App Router
- **Język**: TypeScript
- **Style**: Tailwind CSS
- **Komponenty UI**: shadcn/ui
- **Uwierzytelnianie**: Clerk
- **Zarządzanie Stanem**: React Query, Zustand
- **Obsługa Formularzy**: React Hook Form
- **Walidacja**: Zod
- **Przesyłanie Zdjęć**: Cloudinary

## 📁 Struktura Projektu
```
fishio-frontend/
├── app/                    # Główny katalog App Router
│   ├── (auth)/            # Strony związane z uwierzytelnianiem
│   ├── (main)/            # Główne strony aplikacji (chronione)
│   ├── api/               # Trasy API
│   └── results/           # Strony publicznych wyników
├── components/            # Komponenty wielokrotnego użytku
│   ├── ui/               # Komponenty z shadcn/ui
│   ├── shared/           # Komponenty współdzielone
│   └── features/         # Komponenty specyficzne dla funkcji
├── lib/                  # Narzędzia i logika biznesowa
│   ├── actions/          # Akcje serwerowe
│   ├── api/              # Funkcje klienta API
│   └── validators/       # Schematy walidacji
└── public/              # Zasoby statyczne
```

## 🚀 Rozpoczęcie Pracy

### 📋 Wymagania Wstępne
- Node.js 18.x lub nowszy
- Menedżer pakietów npm lub yarn

### 💻 Instalacja
1. Sklonuj repozytorium
2. Zainstaluj zależności:
```bash
npm install
# lub
yarn install
```

3. Skonfiguruj zmienne środowiskowe:
Utwórz plik `.env.local` z następującymi zmiennymi:
```
NEXT_PUBLIC_CLERK_PUBLISHABLE_KEY=
CLERK_SECRET_KEY=
NEXT_PUBLIC_CLERK_SIGN_IN_URL=
NEXT_PUBLIC_CLERK_SIGN_UP_URL=
NEXT_PUBLIC_CLERK_AFTER_SIGN_IN_URL=
NEXT_PUBLIC_CLERK_AFTER_SIGN_UP_URL=
NEXT_PUBLIC_API_URL=
CLOUDINARY_CLOUD_NAME=
CLOUDINARY_API_KEY=
CLOUDINARY_API_SECRET=
```

4. Uruchom serwer deweloperski:
```bash
npm run dev
# lub
yarn dev
```

## 📝 Wytyczne Rozwoju
- Przestrzegaj ustalonej struktury folderów
- Używaj TypeScript dla wszystkich nowych plików
- Implementuj odpowiednią obsługę błędów
- Pisz czysty, łatwy w utrzymaniu kod
- Przestrzegaj konwencji nazewnictwa komponentów
- Używaj Server Actions do mutacji danych
- Implementuj odpowiednie stany ładowania i błędów

## ⚡ Dostępne Skrypty
- `npm run dev`: Uruchom serwer deweloperski
- `npm run build`: Zbuduj wersję produkcyjną
- `npm run start`: Uruchom serwer produkcyjny
- `npm run lint`: Uruchom ESLint
- `npm run format`: Formatuj kod z Prettier

## 👥 Współpraca
1. Utwórz nową gałąź dla swojej funkcji
2. Wprowadź zmiany
3. Wyślij pull request

## 📚 Dodatkowe Zasoby
- [Dokumentacja Next.js](https://nextjs.org/docs)
- [Dokumentacja Clerk](https://clerk.com/docs)
- [Dokumentacja Tailwind CSS](https://tailwindcss.com/docs)
- [Dokumentacja shadcn/ui](https://ui.shadcn.com)
