fisho-app/
├── app/ # Główny katalog routingu (App Router)
│ ├── (auth)/ # Grupa routingu dla stron/API związanych z uwierzytelnianiem (Clerk może zarządzać tym inaczej)
│ │ └── api/
│ │ └── webhooks/
│ │ └── clerk/
│ │ └── route.ts # Przykładowy webhook dla Clerk (jeśli potrzebny)
│ ├── (main)/ # Grupa routingu dla głównych, uwierzytelnionych części aplikacji
│ │ ├── competitions/ # Sekcja zawodów
│ │ │ ├── create/
│ │ │ │ └── page.tsx # Client Component: useForm (RHF), onSubmit -> useMutation (RQ) -> Server Action
│ │ │ ├── [id]/
│ │ │ │ ├── components/ # Komponenty specyficzne dla widoku szczegółów zawodów
│ │ │ │ │ ├── ActionsSection.tsx # Client: useQuery (RQ) do stanu, przyciski
│ │ │ │ │ ├── FishRegistrationModal.tsx # Client: useForm (RHF), useMutation (RQ) -> Server Action
│ │ │ │ │ ├── Leaderboard.tsx # Client: useQuery (RQ) do danych rankingu
│ │ │ │ │ ├── ManageJudges.tsx # Client: useQuery (RQ), useMutation (RQ) -> Server Actions
│ │ │ │ │ └── ManageParticipants.tsx # Client: useQuery (RQ), useMutation (RQ) -> Server Actions
│ │ │ │ ├── loading.tsx
│ │ │ │ └── page.tsx # Server Component (dane początkowe), hydratacja w Client Components z RQ
│ │ │ ├── loading.tsx
│ │ │ └── page.tsx # Server Component + Client Component z useQuery (RQ)
│ │ ├── fisheries/ # Sekcja bazy łowisk
│ │ │ ├── add/
│ │ │ │ └── page.tsx # Client: useForm (RHF), useMutation (RQ) -> Server Action
│ │ │ ├── [id]/
│ │ │ │ ├── edit/
│ │ │ │ │ └── page.tsx # Client: useForm (RHF), useQuery (RQ), useMutation (RQ) -> Server Action
│ │ │ │ └── page.tsx # Server Component + Client Components z useQuery (RQ)
│ │ │ ├── loading.tsx
│ │ │ └── page.tsx # Server Component + Client Component z useQuery (RQ)
│ │ ├── logbook/ # Sekcja osobistego dziennika połowów
│ │ │ ├── add/
│ │ │ │ └── page.tsx # Client: useForm (RHF), useMutation (RQ) -> Server Action
│ │ │ ├── [catchId]/
│ │ │ │ └── page.tsx # Server Component + Client Components z useQuery (RQ)
│ │ │ ├── loading.tsx
│ │ │ └── page.tsx # Server Component + Client Component z useQuery (RQ)
│ │ ├── my-competitions/
│ │ │ ├── loading.tsx
│ │ │ └── page.tsx # Server Component + Client Component z useQuery (RQ)
│ │ └── layout.tsx # Główny layout dla zalogowanych (Navbar, Sidebar), zawiera logikę ochrony trasy (Clerk)
│ ├── results/ # Sekcja publicznych wyników
│ │ └── [id]/
│ │ └── [token]/
│ │ ├── loading.tsx
│ │ └── page.tsx # Strona publiczna, może pobierać dane serwerowo
│ ├── favicon.ico
│ ├── globals.css # Globalne style (Tailwind importy)
│ ├── layout.tsx # Główny, korzeniowy layout (<html>, <body>), zawiera <ClerkProvider>, <QueryClientProvider>
│ └── page.tsx # Strona główna / landing page (dla niezalogowanych, przyciski do /sign-in, /sign-up)
│
├── components/ # Współdzielone, reużywalne komponenty UI
│ ├── blocks/ # Większe, złożone komponenty (np. CompetitionCard)
│ ├── forms/ # Komponenty specyficzne dla formularzy (mogą używać RHF)
│ ├── icons/ # Komponenty ikon SVG
│ ├── layout/ # Komponenty layoutu (Navbar, Footer, Sidebar)
│ └── ui/ # Komponenty z shadcn/ui
│
├── hooks/ # Niestandardowe hooki React (np. do logiki UI, niekoniecznie stanu globalnego)
│ └── use-media-query.ts # Przykład
│
├── lib/ # Funkcje pomocnicze, typy, logika biznesowa, akcje serwerowe
│ ├── actions/ # Server Actions (wywoływane przez useMutation z RQ)
│ │ ├── competitionActions.ts
│ │ ├── fishCatchActions.ts
│ │ ├── logbookActions.ts
│ │ └── fisheryActions.ts
│ ├── clerk.ts # Funkcje pomocnicze dla Clerk (np. pobieranie danych użytkownika server-side)
│ ├── cloudinary.ts # Funkcje pomocnicze dla Cloudinary
│ ├── constants.ts # Stałe
│ ├── db/ # Logika bazy danych (np. Prisma client, Drizzle setup)
│ │ ├── index.ts
│ │ └── schema.ts # (jeśli używasz ORM jak Drizzle)
│ ├── definitions.ts # Definicje typów TypeScript
│ ├── queryClient.ts # Konfiguracja instancji QueryClient dla React Query
│ ├── queryKeys.ts # Centralne miejsce dla kluczy React Query
│ ├── utils.ts # Ogólne funkcje pomocnicze
│ └── validation.ts # Schematy walidacji (np. Zod) dla RHF i Server Actions
│
├── providers/ # Komponenty dostawców (Providers) dla kontekstów/bibliotek
│ └── AppProviders.tsx # Komponent opakowujący <ClerkProvider>, <QueryClientProvider> itp.
│
├── public/ # Statyczne zasoby
│ └── logo.svg
│
├── store/ # Globalne store'y Zustand
│ ├── uiStore.ts # Przykład store'a dla stanu UI
│ └── userPreferencesStore.ts # Przykład store'a dla preferencji użytkownika
│
├── .env.local
├── .eslintrc.json
├── .gitignore
├── middleware.ts # Middleware Next.js (np. do ochrony tras z użyciem Clerk)
├── next.config.mjs
├── package.json
├── postcss.config.js
├── prettier.config.js
├── tailwind.config.ts
└── tsconfig.json
