// app/page.tsx - PUBLICZNA STRONA POWITALNA
// Importuj ikony, jeśli ich używasz
import { BookOpen, MapPin, Trophy, User, Users } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';

// NIE JEST JUŻ ASYNC, NIE SPRAWDZA auth() TUTAJ
export default function LandingPage() {
  // Renderuj zawartość strony powitalnej
  return (
    <div className="flex min-h-screen items-center justify-center bg-muted p-4">
      {/* Karta/Kontener główny */}
      <div className="w-full max-w-sm overflow-hidden rounded-xl bg-card shadow-lg">
        <div className="p-6 sm:p-8">
          {/* Logo */}
          <div className="mb-6">
            <Image src="/fisherman.svg" alt="Ilustracja Fishio" width={350} height={350} className="mx-auto" />
          </div>

          {/* Nagłówek */}
          <h1 className="mb-6 text-center text-2xl font-bold leading-tight text-foreground sm:text-3xl">
            Odkryj Pełnię Możliwości Fishio!
          </h1>

          {/* Lista Korzyści */}
          <div className="space-y-4">
            {/* Korzyść 1 */}
            <div className="flex items-start space-x-3">
              <div className="flex-shrink-0">
                <Trophy className="h-6 w-6 text-primary" />
              </div>
              <div>
                <p className="font-medium text-foreground">Organizuj i bierz udział w zawodach</p>
                <p className="text-sm text-muted-foreground">(prywatnych i otwartych)</p>
              </div>
            </div>
            {/* Korzyść 2 */}
            <div className="flex items-start space-x-3">
              <div className="flex-shrink-0">
                <BookOpen className="h-6 w-6 text-primary" />
              </div>
              <div>
                <p className="font-medium text-foreground">Prowadź cyfrowy dziennik</p>
                <p className="text-sm text-muted-foreground">swoich połowów ze zdjęciami.</p>
              </div>
            </div>
            {/* Korzyść 3 */}
            <div className="flex items-start space-x-3">
              <div className="flex-shrink-0">
                <MapPin className="h-6 w-6 text-primary" />
              </div>
              <div>
                <p className="font-medium text-foreground">Odkrywaj i zapisuj informacje</p>
                <p className="text-sm text-muted-foreground">o najlepszych łowiskach.</p>
              </div>
            </div>
            {/* Korzyść 4 */}
            <div className="flex items-start space-x-3">
              <div className="flex-shrink-0">
                <User className="h-6 w-6 text-primary" />
              </div>
              <div>
                <p className="font-medium text-foreground">Śledź swoje wyniki</p>
                <p className="text-sm text-muted-foreground">postępy i statystyki.</p>
              </div>
            </div>
            {/* Korzyść 5 */}
            <div className="flex items-start space-x-3">
              <div className="flex-shrink-0">
                <Users className="h-6 w-6 text-primary" />
              </div>
              <div>
                <p className="font-medium text-foreground">Bądź częścią wędkarskiej społeczności</p>
              </div>
            </div>
          </div>

          {/* Przyciski Akcji */}
          <div className="mt-8 space-y-4">
            <Link
              href="/sign-up"
              className="block w-full rounded-lg bg-accent px-5 py-3 text-center text-lg font-semibold text-accent-foreground shadow-sm transition duration-150 ease-in-out hover:opacity-90 focus:outline-none focus:ring-2 focus:ring-accent focus:ring-offset-2"
              style={{ backgroundColor: 'var(--accent)', color: 'var(--accent-foreground)' }}
            >
              Zarejestruj się za Darmo
            </Link>

            <p className="text-center text-sm text-muted-foreground">
              Masz już konto?{' '}
              <Link
                href="/sign-in"
                className="font-medium text-primary hover:underline"
                style={{ color: 'var(--primary)' }}
              >
                Zaloguj się
              </Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}
