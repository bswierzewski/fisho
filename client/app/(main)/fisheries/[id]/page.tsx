// app/(main)/fisheries/[id]/page.tsx
import { FishSpecies as FishSpeciesType, Fishery, LogbookEntry } from '@/lib/definitions';
import {
  availableFishSpecies,
  // Potrzebne do mapowania ID na nazwy
  staticFisheries,
  staticLogbookEntries
} from '@/lib/static-data';
// ... (reszta importów bez zmian) ...
import { ArrowLeft, Edit, Fish as FishIcon, List, MapPin, Plus } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';
import { notFound } from 'next/navigation';

import { Button } from '@/components/ui/button';

// ... (Style i formatDateTime bez zmian) ...
const sectionHeaderBgClass = 'bg-slate-800';
const sectionHeaderTextColorClass = 'text-slate-100';
const cardMutedTextColorClass = 'text-muted-foreground';
const cardTextColorClass = 'text-foreground';
const cardBodyBgClass = 'bg-card';

const formatDate = (date: Date) => {
  return date.toLocaleDateString('pl-PL', {
    day: '2-digit',
    month: 'long',
    year: 'numeric'
  });
};

export default function FisheryDetailPage({ params }: { params: { id: string } }) {
  const fishery = staticFisheries.find((f) => f.id === params.id);

  if (!fishery) {
    notFound();
  }

  const catchesFromThisFishery = staticLogbookEntries.filter((entry) => entry.fisheryId === fishery.id).slice(0, 5);

  // Pobierz nazwy gatunków na podstawie species_ids z obiektu fishery
  const definedSpeciesForDisplay: FishSpeciesType[] = fishery.species_ids
    ? availableFishSpecies.filter((species) => fishery.species_ids!.includes(species.id))
    : [];

  const canEdit = true;
  const canAddCatchHere = true;

  return (
    <div className="space-y-6">
      {/* ... (Nagłówek i Przyciski Akcji - bez zmian) ... */}
      <div className="relative overflow-hidden rounded-lg border border-border shadow">
        {fishery.imageUrl ? (
          <div className="relative h-48 sm:h-64 w-full">
            <Image
              src={fishery.imageUrl}
              alt={`Zdjęcie łowiska ${fishery.name}`}
              layout="fill"
              objectFit="cover"
              priority
            />
            <div className="absolute inset-0 bg-gradient-to-t from-black/70 via-black/40 to-transparent"></div>
            <div className="absolute bottom-0 left-0 right-0 p-4 sm:p-6 text-white">
              <h1 className="text-2xl sm:text-3xl lg:text-4xl font-bold mb-1 drop-shadow-md">{fishery.name}</h1>
              <div className="flex items-center text-sm sm:text-base opacity-90 drop-shadow-sm">
                <MapPin className="mr-1.5 h-4 w-4 sm:h-5 sm:w-5" /> {fishery.location}
              </div>
            </div>
          </div>
        ) : (
          <div className={`p-4 sm:p-6 bg-card text-foreground`}>
            <h1 className="text-2xl sm:text-3xl font-bold mb-1">{fishery.name}</h1>
            <div className="flex items-center text-sm sm:text-base text-muted-foreground">
              <MapPin className="mr-1.5 h-4 w-4 sm:h-5 sm:w-5" /> {fishery.location}
            </div>
          </div>
        )}
      </div>

      <div className="flex flex-wrap gap-2">
        {canEdit && (
          <Link href={`/fisheries/${fishery.id}/edit`}>
            <Button variant="outline">
              <Edit className="mr-2 h-4 w-4" /> Edytuj Łowisko
            </Button>
          </Link>
        )}
        {canAddCatchHere && (
          <Link href={`/logbook/add?fisheryId=${fishery.id}`}>
            <Button className="bg-accent text-accent-foreground hover:bg-accent/90">
              <Plus className="mr-2 h-4 w-4" /> Dodaj Połów na Tym Łowisku
            </Button>
          </Link>
        )}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* ... (Kolumna Główna - Ostatnie Połowy - bez zmian) ... */}
        <div className="lg:col-span-2 space-y-6">
          <section>
            <div
              className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
            >
              <FishIcon className="h-5 w-5" />
              <h2 className="text-lg font-semibold">Ostatnie Połowy na Tym Łowisku</h2>
            </div>
            <div className={`p-4 border-x border-b rounded-b-lg bg-card`}>
              {catchesFromThisFishery.length > 0 ? (
                <ul className="space-y-3">
                  {catchesFromThisFishery.map((entry: LogbookEntry) => (
                    <li
                      key={entry.id}
                      className={`flex items-center space-x-3 p-2 rounded border border-border/50 bg-card shadow-sm`}
                    >
                      <div className="relative h-12 w-12 flex-shrink-0 overflow-hidden rounded bg-muted">
                        <Image src={entry.photoUrl} alt={entry.species} layout="fill" objectFit="cover" />
                      </div>
                      <div>
                        <p className={`font-semibold text-foreground`}>{entry.species}</p>
                        <p className={`text-xs ${cardMutedTextColorClass}`}>{formatDate(entry.catchTime)}</p>
                      </div>
                      <div className="ml-auto text-right">
                        {entry.lengthCm && <p className={`text-xs ${cardMutedTextColorClass}`}>{entry.lengthCm} cm</p>}
                        {entry.weightKg && <p className={`text-xs ${cardMutedTextColorClass}`}>{entry.weightKg} kg</p>}
                      </div>
                    </li>
                  ))}
                </ul>
              ) : (
                <p className={`text-sm ${cardMutedTextColorClass}`}>Brak zarejestrowanych połowów na tym łowisku.</p>
              )}
            </div>
          </section>
        </div>

        {/* Kolumna Boczna (np. Gatunki Ryb) */}
        <div className="space-y-6">
          <section>
            <div
              className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
            >
              <List className="h-5 w-5" />
              <h2 className="text-lg font-semibold">Występujące Gatunki</h2>
            </div>
            <div className={`p-4 border-x border-b rounded-b-lg bg-card`}>
              {definedSpeciesForDisplay.length > 0 ? (
                <ul className="space-y-1 text-sm">
                  {definedSpeciesForDisplay.map((species) => (
                    <li key={species.id} className={`flex items-center ${cardMutedTextColorClass}`}>
                      <FishIcon className="mr-2 h-4 w-4 flex-shrink-0 opacity-70" /> {species.name}
                    </li>
                  ))}
                </ul>
              ) : (
                <p className={`text-sm ${cardMutedTextColorClass}`}>Brak zdefiniowanych gatunków dla tego łowiska.</p>
              )}
            </div>
          </section>
        </div>
      </div>
    </div>
  );
}
