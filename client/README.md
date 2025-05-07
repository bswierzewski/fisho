fishio-app/
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

V2

fishio-frontend/
├── app/ # Main App Router directory
│ ├── (auth)/ # Routing group for authentication pages (Clerk might handle some)
│ │ ├── sign-in/[[...sign-in]]/page.tsx
│ │ └── sign-up/[[...sign-up]]/page.tsx
│ ├── (main)/ # Main routing group for logged-in users
│ │ ├── layout.tsx # Main application layout (e.g., with navigation)
│ │ ├── dashboard/ # (Optional) Main dashboard after login
│ │ │ └── page.tsx
│ │ ├── competitions/ # Competitions section
│ │ │ ├── page.tsx # List of open competitions / "My Competitions" (may require filtering logic)
│ │ │ ├── new/ # Create a new competition
│ │ │ │ └── page.tsx
│ │ │ └── [id]/ # Details of a specific competition
│ │ │ ├── page.tsx # Competition details view (info, participants, results)
│ │ │ └── settings/ # (Optional) Competition settings for the organizer
│ │ │ └── page.tsx
│ │ ├── log/ # Personal fishing log
│ │ │ ├── page.tsx # Log view (gallery/list)
│ │ │ └── new/ # Add a new catch/entry
│ │ │ └── page.tsx
│ │ ├── fisheries/ # Fisheries/Fishing Spots section
│ │ │ ├── page.tsx # List of fisheries
│ │ │ └── [id]/ # Fishery details
│ │ │ └── page.tsx
│ │ ├── profile/ # User profile
│ │ │ └── page.tsx
│ │ └── (admin)/ # (Optional) Admin section (if in the same app)
│ │ └── layout.tsx # Layout for the admin section
│ │ └── page.tsx # Main admin dashboard
│ ├── api/ # Route Handlers (Next.js API endpoints - if needed besides Server Actions)
│ │ └── webhook/ # E.g., for webhooks from Clerk or payment systems
│ │ └── clerk/
│ │ └── route.ts
│ ├── results/ # Public results page
│ │ └── [token]/ # Unique access token for results
│ │ └── page.tsx
│ ├── layout.tsx # Root application layout (e.g., <ClerkProvider>, global contexts)
│ ├── page.tsx # Application home page (landing page)
│ └── globals.css # Global styles
├── components/ # UI Components
│ ├── ui/ # Components from shadcn/ui (following shadcn convention)
│ │ ├── button.tsx
│ │ ├── card.tsx
│ │ └── ...
│ ├── shared/ # Shared, reusable application components
│ │ ├── AuthButtons.tsx # Login/Logout buttons (using Clerk)
│ │ ├── DataTable.tsx # Reusable data table
│ │ ├── LoadingSpinner.tsx
│ │ ├── Modal.tsx
│ │ ├── PageHeader.tsx
│ │ └── PhotoUploader.tsx # Component for uploading photos (e.g., to Cloudinary)
│ └── features/ # Components specific to a feature/domain
│ ├── competitions/ # Components related to competitions
│ │ ├── CompetitionCard.tsx
│ │ ├── CompetitionForm.tsx
│ │ ├── CompetitionList.tsx
│ │ ├── ParticipantManager.tsx # Manage participants (add, remove, roles)
│ │ └── ResultsTable.tsx
│ ├── fishing-log/ # Components related to the fishing log
│ │ ├── CatchCard.tsx
│ │ ├── CatchForm.tsx
│ │ └── LogView.tsx # Component to display the log (list/gallery)
│ ├── fisheries/ # Components related to fisheries
│ │ ├── FisheryCard.tsx
│ │ └── FisheryForm.tsx
│ └── user/ # Components related to the user profile
│ └── UserProfileForm.tsx
├── lib/ # Helper modules, business logic, configuration
│ ├── actions/ # Server Actions (preferred way to interact with the .NET backend)
│ │ ├── competitionActions.ts
│ │ ├── logActions.ts
│ │ ├── fisheryActions.ts
│ │ └── userActions.ts
│ ├── api/ # (Alternative/Supplement to actions) API client functions for .NET backend communication
│ │ ├── client.ts # HTTP client configuration (e.g., axios, fetch)
│ │ ├── competitions.ts # API functions for competitions
│ │ ├── catches.ts # API functions for catches/log entries
│ │ ├── fisheries.ts # API functions for fisheries
│ │ └── index.ts
│ ├── validators/ # Validation schemas (e.g., Zod)
│ │ ├── competitionSchema.ts
│ │ ├── catchSchema.ts
│ │ └── fisherySchema.ts
│ ├── hooks/ # Custom React Hooks
│ │ └── useMediaQuery.ts # E.g., for responsiveness logic
│ ├── types/ # TypeScript type definitions
│ │ ├── index.ts
│ │ ├── competition.ts
│ │ ├── user.ts
│ │ ├── catch.ts
│ │ └── fishery.ts
│ ├── utils.ts # Utility functions (e.g., date formatting, cn from shadcn)
│ ├── constants.ts # Constants used throughout the application
│ └── auth.ts # Clerk-related configuration (e.g., roles, helpers)
├── public/ # Static assets (images, fonts)
│ ├── images/
│ └── favicon.ico
├── styles/ # Additional global styles (if globals.css is not enough)
├── .env.local # Environment variables (local)
├── .eslintrc.json # ESLint configuration
├── .gitignore
├── middleware.ts # Next.js Middleware (e.g., for route protection with Clerk)
├── next.config.mjs # Next.js configuration
├── package.json
├── postcss.config.js # PostCSS configuration (for Tailwind)
├── tailwind.config.ts # TailwindCSS configuration
└── tsconfig.json # TypeScript configuration
