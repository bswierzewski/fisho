# Dockerfile

# --- Etap 1: Budowanie Aplikacji ---
# Użyj oficjalnego obrazu Node.js jako bazy dla etapu budowania.
# Wybierz wersję Node.js zgodną z Twoim projektem (np. LTS).
FROM node:20-alpine AS builder

# Ustaw katalog roboczy w kontenerze
WORKDIR /app

# Skopiuj pliki package.json i package-lock.json (lub yarn.lock, pnpm-lock.yaml)
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

# Skopiuj resztę kodu aplikacji
COPY . .

# Zbuduj aplikację Next.js
# Ta komenda uruchomi skrypt "build" zdefiniowany w Twoim package.json
RUN npm run build
# Dla yarn:
# RUN yarn build
# Dla pnpm:
# RUN pnpm build

# Opcjonalnie: Usuń zależności deweloperskie, aby zmniejszyć rozmiar warstwy,
# jeśli nie są potrzebne w kolejnym etapie (choć Next.js standalone output radzi sobie z tym dobrze)
# RUN npm prune --production

# --- Etap 2: Uruchamianie Aplikacji ---
# Użyj lżejszego obrazu Node.js dla etapu uruchamiania.
FROM node:20-alpine AS runner

# Ustaw katalog roboczy
WORKDIR /app

# Ustaw zmienną środowiskową dla środowiska produkcyjnego
ENV NODE_ENV production

# Skopiuj zbudowaną aplikację z etapu 'builder'
# Next.js w wersji 12+ generuje folder .next/standalone, który zawiera tylko niezbędne pliki.
# To znacznie zmniejsza rozmiar finalnego obrazu.
# Upewnij się, że masz włączony 'standalone' output w next.config.js:
# output: 'standalone',
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static

# Skopiuj folder public, jeśli go używasz
COPY --from=builder /app/public ./public

# Domyślnie Next.js standalone output nie kopiuje .env.production,
# więc jeśli go używasz, musisz go skopiować lub zarządzać zmiennymi środowiskowymi inaczej.
# Jeśli używasz .env.production:
# COPY --from=builder /app/.env.production ./

# Ustaw użytkownika innego niż root dla bezpieczeństwa (opcjonalne, ale zalecane)
# RUN addgroup --system --gid 1001 nodejs
# RUN adduser --system --uid 1001 nextjs
# USER nextjs

# Wyeksponuj port, na którym działa aplikacja Next.js (domyślnie 3000)
EXPOSE 3000

# Zdefiniuj komendę uruchamiającą aplikację
# Używamy pliku server.js z outputu standalone
CMD ["node", "server.js"]
