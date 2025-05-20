# client/Dockerfile

# --- Etap 1: Budowanie Aplikacji Next.js ---
# Użyj oficjalnego obrazu Node.js jako bazy dla etapu budowania.
# Wybierz wersję Node.js zgodną z Twoim projektem (np. LTS).
FROM node:20-alpine AS builder

# Ustaw katalog roboczy w kontenerze (względem kontekstu ./client)
WORKDIR /app

# Deklaruj argumenty budowania, które będą przekazywane z GitHub Actions
# Te zmienne muszą być publiczne, ponieważ są używane podczas budowania frontendu
ARG NEXT_PUBLIC_CLERK_PUBLISHABLE_KEY
ARG NEXT_PUBLIC_CLERK_TOKEN_TEMPLATE

# Ustaw je jako zmienne środowiskowe dla procesu budowania
ENV NEXT_PUBLIC_CLERK_PUBLISHABLE_KEY=$NEXT_PUBLIC_CLERK_PUBLISHABLE_KEY
ENV NEXT_PUBLIC_CLERK_TOKEN_TEMPLATE=$NEXT_PUBLIC_CLERK_TOKEN_TEMPLATE

# Skopiuj pliki package.json i package-lock.json (lub yarn.lock, pnpm-lock.yaml)
# Dockerfile jest w ./client, więc ścieżki są względem ./client
COPY package*.json ./
# Jeśli używasz yarn:
# COPY yarn.lock ./
# Jeśli używasz pnpm:
# COPY pnpm-lock.yaml ./

# Zainstaluj zależności produkcyjne i deweloperskie potrzebne do budowania
# Dla npm:
RUN npm install
# Dla yarn:
# RUN yarn install --frozen-lockfile
# Dla pnpm:
# RUN npm install -g pnpm && pnpm install --frozen-lockfile

# Skopiuj resztę kodu aplikacji z folderu ./client do /app w kontenerze
COPY . .

# Zbuduj aplikację Next.js
# Ta komenda uruchomi skrypt "build" zdefiniowany w Twoim package.json
RUN npm run build
# Dla yarn:
# RUN yarn build
# Dla pnpm:
# RUN pnpm build

# --- Etap 2: Uruchamianie Aplikacji Next.js ---
# Użyj lżejszego obrazu Node.js dla etapu uruchamiania.
FROM node:20-alpine AS runner

# Ustaw katalog roboczy
WORKDIR /app

# Ustaw zmienną środowiskową dla środowiska produkcyjnego
# Ta zmienna jest ważna dla Next.js, aby działał w trybie produkcyjnym.
ENV NODE_ENV=production

# Skopiuj zbudowaną aplikację z etapu 'builder'
# Używamy outputu 'standalone' z Next.js dla optymalizacji rozmiaru obrazu.
# Upewnij się, że masz włączony 'output: 'standalone',' w client/next.config.mjs
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static

# Skopiuj folder public, jeśli go używasz w aplikacji klienckiej
COPY --from=builder /app/public ./public

# Domyślnie Next.js standalone output nie kopiuje plików .env.
# Zmienne środowiskowe (zwłaszcza sekrety jak CLERK_SECRET_KEY)
# powinny być przekazywane do kontenera podczas jego uruchamiania,
# a nie wbudowywane w obraz.

# Ustaw użytkownika innego niż root dla bezpieczeństwa (opcjonalne, ale zalecane)
RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs
USER nextjs

# Wyeksponuj port, na którym działa aplikacja Next.js (domyślnie 3000)
EXPOSE 3000

# Zdefiniuj komendę uruchamiającą aplikację
# Używamy pliku server.js z outputu standalone
CMD ["node", "server.js"]
