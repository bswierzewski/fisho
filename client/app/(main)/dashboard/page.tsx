'use client';

// Potrzebne dla przyszłych interakcji
import { Competition, Fishery, LogbookEntry } from '@/lib/definitions';
import { staticCompetitions, staticFisheries, staticLogbookEntries } from '@/lib/static-data';
import { ArrowRight, MapPin, Plus, Trophy } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';

const cardHeaderBgClass = 'bg-secondary'; // Przykład: ciemny szaro-niebieski
const cardHeaderTextColorClass = 'text-secondary-foreground'; // Jasny tekst dla ciemnego tła
const cardBodyBgClass = 'bg-card'; // Tło reszty karty (np. białe lub jasnoszare)
const cardTextColorClass = 'text-foreground'; // Główny kolor tekstu w body
const cardMutedTextColorClass = 'text-muted-foreground'; // Drugorzędny kolor tekstu

// --- Komponent Pomocniczy dla Karty Akcji ---
const ActionCard = ({ href, text, icon: Icon }: { href: string; text: string; icon: React.ElementType }) => (
  <Link href={href} className="block w-40 flex-shrink-0 h-full">
    {/* Dodano h-full */}
    <div className="flex h-full flex-col items-center justify-center rounded-lg border border-dashed border-border bg-card p-3 text-center transition-colors hover:bg-muted/50">
      {/* Tutaj możesz dodać wektorową ilustrację wędkarza zamiast ikony */}
      {/* <Image src="/vector-angler.svg" width={60} height={60} alt="" className="mb-2" /> */}
      <Icon className="h-8 w-8 text-primary mb-2" /> {/* Domyślnie ikona */}
      <span className="text-sm font-medium text-foreground">{text}</span>
    </div>
  </Link>
);

// --- Główny Komponent Dashboardu ---
export default function DashboardPage() {
  // Ograniczamy liczbę elementów do wyświetlenia
  const upcomingCompetitions = staticCompetitions.filter((c) => c.status === 'upcoming').slice(0, 2);
  const recentCatches = staticLogbookEntries.slice(0, 2);
  // Dla "Odkryj Otwarte Zawody" weźmy inne niż nadchodzące, jeśli są
  const openCompetitions = staticCompetitions
    .filter((c) => c.type === 'open' && c.status !== 'finished' && !upcomingCompetitions.some((uc) => uc.id === c.id))
    .slice(0, 2);
  const featuredFisheries = staticFisheries.slice(0, 2);

  return (
    <div className="space-y-8">
      {/* --- Sekcja Nadchodzące Zawody --- */}
      <section>
        <h2 className="text-xl font-bold mb-4 text-foreground">Nadchodzące Zawody</h2>
        {/* Kontener dla kart z ustaloną wysokością, aby ActionCard pasowała */}
        <div className="flex space-x-4 overflow-x-auto pb-4 h-48">
          {' '}
          {/* Ustalona wysokość */}
          {upcomingCompetitions.map((comp: Competition) => (
            <Link href={`/competitions/${comp.id}`} className="w-40" key={comp.id}>
              <div className="h-full flex-shrink-0 overflow-hidden rounded-lg shadow border border-border flex flex-col">
                <div
                  className={`${cardHeaderBgClass} ${cardHeaderTextColorClass} p-3 flex items-center space-x-2 relative h-10 flex-shrink-0`}
                >
                  {comp.imageUrl && (
                    <Image src={comp.imageUrl} alt="" layout="fill" objectFit="cover" className="opacity-20 z-0" />
                  )}
                  <div className="relative z-10 flex items-center space-x-2">
                    <Trophy className="h-4 w-4" />
                    <span className="text-xs font-medium truncate">Zawody</span>
                  </div>
                </div>
                <div className={`p-3 flex-grow flex flex-col justify-between ${cardBodyBgClass}`}>
                  <div>
                    <h3 className={`text-sm font-semibold ${cardTextColorClass} mb-1 truncate`}>{comp.name}</h3>
                    <p className={`text-xs ${cardMutedTextColorClass}`}>{comp.startTime.toLocaleDateString('pl-PL')}</p>
                    <p className={`text-xs ${cardMutedTextColorClass} truncate`}>{comp.location}</p>
                  </div>
                </div>
              </div>
            </Link>
          ))}
          <ActionCard href="/my-competitions" text="Zobacz Wszystkie" icon={ArrowRight} />
        </div>
      </section>

      {/* --- Sekcja Ostatnie Połowy --- */}
      <section>
        <h2 className="text-xl font-bold mb-4 text-foreground">Ostatnie Połowy</h2>
        <div className="flex space-x-4 overflow-x-auto pb-4 h-48">
          {recentCatches.map((entry: LogbookEntry) => (
            <Link href={`/logbook`} className="w-40" key={entry.id}>
              <div className="h-full w-40 flex-shrink-0 overflow-hidden rounded-lg shadow border border-border flex flex-col">
                {/* Zdjęcie jako główna część karty */}
                <div className="relative h-24 w-full bg-muted flex-shrink-0">
                  <Image src={entry.photoUrl} alt={entry.species} layout="fill" objectFit="cover" />
                </div>
                <div className={`p-3 flex-grow flex flex-col justify-between ${cardBodyBgClass}`}>
                  <div>
                    <h3 className={`text-sm font-semibold ${cardTextColorClass} mb-1 truncate`}>{entry.species}</h3>
                    <p className={`text-xs ${cardMutedTextColorClass}`}>
                      {entry.catchTime.toLocaleDateString('pl-PL')}
                    </p>
                  </div>
                </div>
              </div>
            </Link>
          ))}
          <ActionCard href="/logbook/add" text="Dodaj Nowy Połów" icon={Plus} />
        </div>
      </section>

      {/* --- Sekcja Odkryj Otwarte Zawody --- */}
      <section>
        <h2 className="text-xl font-bold mb-4 text-foreground">Odkryj Otwarte Zawody</h2>
        <div className="flex space-x-4 overflow-x-auto pb-4 h-48">
          {' '}
          {/* Ustalona wysokość */}
          {openCompetitions.map((comp: Competition) => (
            <Link href={`/competitions/${comp.id}`} className="w-40" key={comp.id}>
              <div className="h-full w-40 flex-shrink-0 overflow-hidden rounded-lg shadow border border-border flex flex-col">
                <div
                  className={`${cardHeaderBgClass} ${cardHeaderTextColorClass} p-3 flex items-center space-x-2 relative h-10 flex-shrink-0`}
                >
                  {comp.imageUrl && (
                    <Image src={comp.imageUrl} alt="" layout="fill" objectFit="cover" className="opacity-20 z-0" />
                  )}
                  <div className="relative z-10 flex items-center space-x-2">
                    <Trophy className="h-4 w-4" />
                    <span className="text-xs font-medium truncate">Otwarte Zawody</span>
                  </div>
                </div>
                <div className={`p-3 flex-grow flex flex-col justify-between ${cardBodyBgClass}`}>
                  <div>
                    <h3 className={`text-sm font-semibold ${cardTextColorClass} mb-1 truncate`}>{comp.name}</h3>
                    <p className={`text-xs ${cardMutedTextColorClass}`}>{comp.startTime.toLocaleDateString('pl-PL')}</p>
                    <p className={`text-xs ${cardMutedTextColorClass} truncate`}>{comp.location}</p>
                  </div>
                </div>
              </div>
            </Link>
          ))}
          <ActionCard href="/competitions" text="Przeglądaj Wszystkie" icon={ArrowRight} />
        </div>
      </section>

      {/* --- Sekcja Lista Łowisk --- */}
      <section>
        <h2 className="text-xl font-bold mb-4 text-foreground">Lista Łowisk</h2>
        <div className="flex space-x-4 overflow-x-auto pb-4 h-48">
          {' '}
          {/* Ustalona wysokość */}
          {featuredFisheries.map((fishery: Fishery) => (
            <Link href={`/fisheries`} className="w-40" key={fishery.id}>
              <div className="h-full w-40 flex-shrink-0 overflow-hidden rounded-lg shadow border border-border flex flex-col">
                <div
                  className={`${cardHeaderBgClass} ${cardHeaderTextColorClass} p-3 flex items-center space-x-2 relative h-10 flex-shrink-0`}
                >
                  {fishery.imageUrl && (
                    <Image src={fishery.imageUrl} alt="" layout="fill" objectFit="cover" className="opacity-20 z-0" />
                  )}
                  <div className="relative z-10 flex items-center space-x-2">
                    <MapPin className="h-4 w-4" />
                    <span className="text-xs font-medium truncate">Łowisko</span>
                  </div>
                </div>
                <div className={`relative p-3 flex-grow flex flex-col justify-between ${cardBodyBgClass}`}>
                  <div>
                    <h3 className={`text-sm font-semibold ${cardTextColorClass} mb-1 truncate`}>{fishery.name}</h3>
                    <p className={`text-xs ${cardMutedTextColorClass} truncate`}>{fishery.location}</p>
                  </div>
                </div>
              </div>
            </Link>
          ))}
          <ActionCard href="/fisheries/add" text="Dodaj Nowe Łowisko" icon={Plus} />
        </div>
      </section>
    </div>
  );
}
