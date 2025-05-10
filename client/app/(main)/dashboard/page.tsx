'use client';

// Potrzebne dla przyszłych interakcji
import { Competition, Fishery, LogbookEntry } from '@/lib/definitions';
import {
  availableMainScoringCategories,
  availableSpecialCategories,
  staticCompetitions,
  staticFisheries,
  staticLogbookEntries
} from '@/lib/static-data';
import { Activity, ArrowRight, Award, CheckCircle2, Clock, ListChecks, MapPin, Plus, Trophy } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';

import { useMediaQuery } from '@/hooks/use-media-query';

import { Button } from '@/components/ui/button';

const cardHeaderBgClass = 'bg-secondary'; // Przykład: ciemny szaro-niebieski
const cardHeaderTextColorClass = 'text-secondary-foreground'; // Jasny tekst dla ciemnego tła
const cardBodyBgClass = 'bg-card'; // Tło reszty karty (np. białe lub jasnoszare)
const cardTextColorClass = 'text-foreground'; // Główny kolor tekstu w body
const cardMutedTextColorClass = 'text-muted-foreground'; // Drugorzędny kolor tekstu

// --- Komponent Pomocniczy dla Karty Akcji ---
const ActionCard = ({
  href,
  text,
  icon: Icon,
  src
}: {
  href: string;
  text: string;
  icon: React.ElementType;
  src: string;
}) => (
  <Link href={href} className="block w-40 flex-shrink-0 h-full">
    {/* Dodano h-full */}
    <div className="relative flex h-full flex-col items-center justify-center rounded-lg border border-border bg-card p-3 text-center transition-colors hover:bg-muted/50">
      {src && <Image src={src} alt="" layout="fill" objectFit="fill" className="opacity-10 z-0" />}
      <Icon className="h-8 w-8 text-primary mb-2" />
      <span className="text-sm font-medium text-foreground">{text}</span>
    </div>
  </Link>
);

// --- Główny Komponent Dashboardu ---
export default function DashboardPage() {
  // --- Logika Responsywnego Slice ---
  const isSm = useMediaQuery('(min-width: 640px)'); // np. małe tablety w pionie
  const isMd = useMediaQuery('(min-width: 768px)'); // np. tablety
  const isLg = useMediaQuery('(min-width: 1024px)'); // np. desktopy

  // Określ liczbę kart do wyświetlenia
  let sliceCount = 2; // Domyślnie (mobile)
  if (isLg) {
    sliceCount = 8; // Duże ekrany
  } else if (isMd) {
    sliceCount = 6; // Średnie ekrany
  } else if (isSm) {
    sliceCount = 3; // Małe tablety - nadal 2 lub można dać 3 jeśli się mieszczą
  }
  // ------------------------------------

  // Symulacja "Moich Zawodów" - bierzemy kilka z różnymi statusami
  // W prawdziwej aplikacji filtrowałbyś na podstawie ID użytkownika i jego powiązań
  const myDashboardCompetitions = [
    staticCompetitions.find((c) => c.status === 'ongoing'), // Jedne trwające
    staticCompetitions.find((c) => c.status === 'upcoming'), // Jedne nadchodzące
    staticCompetitions.find((c) => c.status === 'finished') // Jedne zakończone
  ]
    .filter(Boolean)
    .slice(0, sliceCount) as Competition[]; // Usuń null/undefined i zastosuj slice
  const recentCatches = staticLogbookEntries.slice(0, sliceCount);
  const openCompetitions = staticCompetitions
    .filter(
      (c) => c.type === 'open' && c.status !== 'finished' && !myDashboardCompetitions.some((uc) => uc.id === c.id)
    )
    .slice(0, sliceCount);
  const featuredFisheries = staticFisheries.slice(0, sliceCount);

  const getStatusStyles = (status: Competition['status']) => {
    switch (status) {
      case 'ongoing':
        return {
          borderColor: 'border-green-500', // Obramowanie dla trwających
          textColor: 'text-green-500',
          icon: <Activity className="h-3.5 w-3.5" />,
          label: 'Trwające'
        };
      case 'upcoming':
        return {
          borderColor: 'border-blue-500', // Obramowanie dla nadchodzących
          textColor: 'text-blue-500',
          icon: <Clock className="h-3.5 w-3.5" />,
          label: 'Nadchodzące'
        };
      case 'finished':
        return {
          borderColor: 'border-slate-500', // Obramowanie dla zakończonych
          textColor: 'text-slate-500',
          icon: <CheckCircle2 className="h-3.5 w-3.5" />,
          label: 'Zakończone'
        };
      default:
        return { borderColor: 'border-border', textColor: 'text-muted-foreground', icon: null, label: '' };
    }
  };

  return (
    <div className="space-y-8">
      {/* <div className="flex flex-col sm:flex-row justify-between items-center gap-4 mb-8">
        <h1 className="text-2xl sm:text-3xl font-bold text-foreground">Panel Główny</h1>
        <div className="flex flex-col sm:flex-row gap-2 w-full sm:w-auto">
          <Link href="/competitions/add" className="w-full sm:w-auto">
            <Button className="bg-accent text-accent-foreground hover:bg-accent/90 w-full">
              <Plus className="mr-2 h-4 w-4" /> Stwórz Zawody
            </Button>
          </Link>
          <Link href="/logbook/add" className="w-full sm:w-auto">
            <Button className="bg-primary text-primary-foreground hover:bg-primary/90 w-full">
              <Plus className="mr-2 h-4 w-4" /> Dodaj Połów
            </Button>
          </Link>
        </div>
      </div> */}

      {/* --- Sekcja Moje Zawody --- */}
      <section>
        <h2 className="text-xl font-bold mb-4 text-foreground">Moje Zawody</h2>
        <div className="flex space-x-4 overflow-x-auto pb-4 h-44">
          {myDashboardCompetitions.map((comp: Competition) => {
            const statusInfo = getStatusStyles(comp.status);
            return (
              <Link href={`/competitions/${comp.id}`} className="w-44 flex-shrink-0 h-full" key={comp.id}>
                <div className="h-full flex-shrink-0 overflow-hidden rounded-lg shadow border border-border flex flex-col bg-card">
                  <div
                    className={`${cardHeaderBgClass} ${cardHeaderTextColorClass} p-2 flex items-center space-x-1.5 relative h-9 flex-shrink-0`}
                  >
                    {comp.imageUrl && (
                      <Image src={comp.imageUrl} alt="" layout="fill" objectFit="cover" className="opacity-20 z-0" />
                    )}
                    <div className="relative z-10 flex items-center space-x-1">
                      <Trophy className="h-4 w-4" />
                      <span className="text-[12px] font-medium truncate">Zawody</span>
                    </div>
                  </div>
                  <div className={`p-2 flex-grow flex flex-col justify-between`}>
                    <div>
                      <h3 className={`text-sm font-semibold ${cardTextColorClass} truncate`}>{comp.name}</h3>
                      <p className={`text-xs ${cardMutedTextColorClass}`}>
                        {comp.startTime.toLocaleDateString('pl-PL')}
                      </p>
                      <p className={`text-xs ${cardMutedTextColorClass} truncate`}>{comp.location}</p>
                    </div>
                  </div>
                  {/* Etykieta statusu w nagłówku */}
                  <div
                    className={`flex items-center space-x-1 text-[10px] font-semibold p-2 ${statusInfo.textColor} bg-opacity-10 ${
                      comp.status === 'ongoing'
                        ? 'bg-green-500/10'
                        : comp.status === 'upcoming'
                          ? 'bg-blue-500/10'
                          : 'bg-slate-500/10'
                    }`}
                  >
                    {statusInfo.icon}
                    <span>{statusInfo.label}</span>
                  </div>
                </div>
              </Link>
            );
          })}
          <ActionCard
            href="/my-competitions"
            text="Zobacz Wszystkie"
            icon={ArrowRight}
            src="/laurel_wreath_black.svg"
          />
        </div>
      </section>

      {/* --- Sekcja Ostatnie Połowy --- */}
      <section>
        <h2 className="text-xl font-bold mb-4 text-foreground">Ostatnie Połowy</h2>
        <div className="flex space-x-4 overflow-x-auto pb-4 h-48">
          {recentCatches.map((entry: LogbookEntry) => (
            <Link href={`/logbook/${entry.id}`} className="w-40" key={entry.id}>
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
          <ActionCard href="/logbook/add" text="Dodaj Nowy Połów" icon={Plus} src="koi.svg" />
        </div>
      </section>

      {/* --- Sekcja Odkryj Otwarte Zawody --- */}
      <section>
        <h2 className="text-xl font-bold mb-4 text-foreground">Odkryj Otwarte Zawody</h2>
        <div className="flex space-x-4 overflow-x-auto pb-4 h-48">
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
          <ActionCard
            href="/competitions"
            text="Przeglądaj Wszystkie"
            icon={ArrowRight}
            src="laurel_wreath_green.svg"
          />
        </div>
      </section>

      {/* --- Sekcja Lista Łowisk --- */}
      <section>
        <h2 className="text-xl font-bold mb-4 text-foreground">Lista Łowisk</h2>
        <div className="flex space-x-4 overflow-x-auto pb-4 h-40">
          {featuredFisheries.map((fishery: Fishery) => (
            <Link href={`/fisheries/${fishery.id}`} className="w-48 flex-shrink-0 h-full group" key={fishery.id}>
              <div className="relative h-full w-full flex-shrink-0 overflow-hidden rounded-lg shadow border border-border flex flex-col justify-end">
                {/* Obrazek jako tło całej karty */}
                {fishery.imageUrl ? (
                  <Image
                    src={fishery.imageUrl}
                    alt={fishery.name}
                    layout="fill"
                    objectFit="cover"
                    className="z-0 transition-transform duration-300 group-hover:scale-105" // Efekt zoomu
                  />
                ) : (
                  // Fallback, jeśli brak obrazka
                  <div className="absolute inset-0 bg-muted flex items-center justify-center z-0">
                    <MapPin className="h-12 w-12 text-muted-foreground/50" />
                  </div>
                )}
                {/* Nakładka gradientowa dla czytelności tekstu */}
                <div className="absolute inset-0 bg-gradient-to-t from-black/70 via-black/30 to-transparent z-10"></div>

                {/* Treść na obrazku */}
                <div className="relative z-20 p-3 text-white">
                  <h3 className="text-sm font-semibold drop-shadow-sm truncate">{fishery.name}</h3>
                  <p className="text-xs opacity-90 drop-shadow-sm truncate">{fishery.location}</p>
                </div>
              </div>
            </Link>
          ))}
          <ActionCard href="/fisheries/add" text="Dodaj Nowe Łowisko" icon={Plus} src="/pond.svg" />
        </div>
      </section>
    </div>
  );
}
