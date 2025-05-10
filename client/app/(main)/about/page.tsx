import { APP_VERSION, BUILD_DATE } from '@/lib/appVersion';
import { Calendar, FileText, GitCommit, Info, Mail, Shield, Users } from 'lucide-react';
import Link from 'next/link';

// Style (zakładamy, że są zdefiniowane globalnie)
const cardBodyBgClass = 'bg-card';
const cardTextColorClass = 'text-foreground';
const cardMutedTextColorClass = 'text-muted-foreground';
const sectionHeaderBgClass = 'bg-secondary'; // Użyjemy tego dla nagłówka sekcji
const sectionHeaderTextColorClass = 'text-secondary-foreground';

export default function AboutPage() {
  const buildDateFormatted = BUILD_DATE
    ? new Date(BUILD_DATE).toLocaleString('pl-PL', {
        day: '2-digit',
        month: 'long',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      })
    : 'Nieznana';

  return (
    <>
      <div className="space-y-8 max-w-2xl mx-auto">
        {' '}
        {/* Ograniczamy szerokość dla lepszej czytelności */}
        {/* Główny Nagłówek Strony */}
        <div className="text-center">
          <Info className={`mx-auto h-16 w-16 mb-4 text-primary`} />
          <h1 className={`text-3xl font-bold ${cardTextColorClass}`}>O Aplikacji Fisho</h1>
          <p className={`mt-2 text-lg ${cardMutedTextColorClass}`}>Wszystko, co musisz wiedzieć o naszej platformie.</p>
        </div>
        {/* Sekcja Informacji o Wersji */}
        <section>
          <div
            className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
          >
            <GitCommit className="h-5 w-5" />
            <h2 className="text-lg font-semibold">Wersja Aplikacji</h2>
          </div>
          <div
            className={`p-4 sm:p-6 border-x border-b rounded-b-lg ${cardBodyBgClass} ${cardTextColorClass} space-y-2 text-sm`}
          >
            {APP_VERSION && (
              <p>
                <strong>Aktualna Wersja:</strong> {APP_VERSION}
              </p>
            )}
            {BUILD_DATE && (
              <p>
                <strong>Data Buildu:</strong> {buildDateFormatted}
              </p>
            )}
            <p>Dziękujemy za korzystanie z Fisho!</p>
          </div>
        </section>
        {/* Sekcja Główne Funkcje (Przykład) */}
        <section>
          <div
            className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
          >
            <Users className="h-5 w-5" />
            <h2 className="text-lg font-semibold">Kluczowe Funkcje</h2>
          </div>
          <div className={`p-4 sm:p-6 border-x border-b rounded-b-lg ${cardBodyBgClass} ${cardTextColorClass} text-sm`}>
            <ul className="list-disc list-inside space-y-1">
              <li>Organizowanie i zarządzanie zawodami wędkarskimi.</li>
              <li>Uczestnictwo w zawodach otwartych i zamkniętych.</li>
              <li>Prowadzenie osobistego dziennika połowów ze zdjęciami.</li>
              <li>Odkrywanie i zarządzanie informacjami o łowiskach.</li>
              <li>Śledzenie wyników i rankingów.</li>
            </ul>
          </div>
        </section>
        {/* Sekcja Linki (Przykład) */}
        <section>
          <div
            className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
          >
            <FileText className="h-5 w-5" />
            <h2 className="text-lg font-semibold">Przydatne Linki</h2>
          </div>
          <div
            className={`p-4 sm:p-6 border-x border-b rounded-b-lg ${cardBodyBgClass} ${cardTextColorClass} space-y-2 text-sm`}
          >
            <p>
              <Link href="/privacy-policy" className="text-primary hover:underline">
                Polityka Prywatności
              </Link>{' '}
              (placeholder)
            </p>
            <p>
              <Link href="/terms-of-service" className="text-primary hover:underline">
                Regulamin Usługi
              </Link>{' '}
              (placeholder)
            </p>
          </div>
        </section>
        {/* Sekcja Kontakt (Przykład) */}
        <section>
          <div
            className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
          >
            <Mail className="h-5 w-5" />
            <h2 className="text-lg font-semibold">Kontakt</h2>
          </div>
          <div className={`p-4 sm:p-6 border-x border-b rounded-b-lg ${cardBodyBgClass} ${cardTextColorClass} text-sm`}>
            <p>Masz pytania lub sugestie? Skontaktuj się z nami:</p>
            <p className="mt-1">
              <a href="mailto:swierzewski.bartosz@gmail.com" className="text-primary hover:underline">
                swierzewski.bartosz@gmail.com {/* Zastąp prawdziwym adresem */}
              </a>
            </p>
            <p className={`mt-2 text-xs ${cardMutedTextColorClass}`}>(Odpowiemy najszybciej jak to możliwe)</p>
          </div>
        </section>
      </div>
    </>
  );
}
