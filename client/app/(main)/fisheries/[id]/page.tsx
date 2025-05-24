"use client";

import { useEffect } from 'react';
import Image from 'next/image';
import Link from 'next/link';
import { notFound, useParams } from 'next/navigation'; // Use useParams for client components
import { ArrowLeft, Edit, Fish as FishIcon, List, MapPin, Plus } from 'lucide-react';
import { format } from 'date-fns';
import { pl } from 'date-fns/locale';

// Assuming DTOs from generated API client
import { FisheryDto, LogbookEntryDto, FishSpeciesDto } from '@/lib/api/models'; 
// Assuming the hook is in this path, adjust if necessary
import { useGetFisheryById } from '@/lib/api/endpoints/fisheries'; 

import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton'; // For loading state


const sectionHeaderBgClass = 'bg-slate-800';
const sectionHeaderTextColorClass = 'text-slate-100';
const cardMutedTextColorClass = 'text-muted-foreground';
// const cardTextColorClass = 'text-foreground'; // No longer used directly, can be removed if not needed elsewhere
// const cardBodyBgClass = 'bg-card'; // No longer used directly, can be removed if not needed elsewhere

const formatDateInternal = (dateInput: Date | string | number) => {
  const date = typeof dateInput === 'string' || typeof dateInput === 'number' ? new Date(dateInput) : dateInput;
  return format(date, 'dd MMMM yyyy', { locale: pl });
};

export default function FisheryDetailPage() {
  const params = useParams();
  const idParam = params.id as string;
  const fisheryId = parseInt(idParam, 10);

  // Fetch fishery data using the hook
  const { data: fishery, isLoading, error, isError } = useGetFisheryById(fisheryId, { query: { enabled: !isNaN(fisheryId) }});

  useEffect(() => {
    if (isNaN(fisheryId)) {
      notFound(); // If id is not a number, navigate to notFound
      return;
    }
    if (!isLoading && (isError || !fishery)) {
      notFound();
    }
  }, [isLoading, isError, fishery, fisheryId]);

  if (isLoading || isNaN(fisheryId)) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-12 w-1/4" />
        <Skeleton className="h-64 w-full rounded-lg" />
        <div className="flex gap-2">
          <Skeleton className="h-10 w-32" />
          <Skeleton className="h-10 w-48" />
        </div>
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          <div className="lg:col-span-2 space-y-6">
            <Skeleton className="h-12 w-full" />
            <Skeleton className="h-48 w-full" />
          </div>
          <div className="space-y-6">
            <Skeleton className="h-12 w-full" />
            <Skeleton className="h-32 w-full" />
          </div>
        </div>
      </div>
    );
  }

  // Note: notFound() is called in useEffect, which handles redirection.
  // We need to return null or some placeholder if fishery is not available yet
  // or an error occurred, to prevent rendering with undefined data.
  if (isError || !fishery) {
    // This will be shown briefly before notFound() redirects if error or no fishery
    // or if fisheryId was NaN and useEffect kicked in.
    return null; 
  }
  
  // FisheryDto does not seem to contain logbook entries directly.
  // This will be empty unless fetched separately.
  const catchesFromThisFishery: LogbookEntryDto[] = []; // fishery.logbookEntries || []).slice(0, 5);
  const definedSpeciesForDisplay = fishery.fishSpecies || [];

  const canEdit = true; // This might come from user permissions or API in a real app
  const canAddCatchHere = true; // Same as above

  return (
    <div className="space-y-6">
      {/* Header and Action Buttons */}
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
        {/* Main Column - Recent Catches */}
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
                  {catchesFromThisFishery.map((entry: LogbookEntryDto) => (
                    <li
                      key={entry.id} // Assuming LogbookEntryDto has an id
                      className={`flex items-center space-x-3 p-2 rounded border border-border/50 bg-card shadow-sm`}
                    >
                      <div className="relative h-12 w-12 flex-shrink-0 overflow-hidden rounded bg-muted">
                        {/* LogbookEntryDto has imageUrl, fishSpeciesName */}
                        <Image src={entry.imageUrl || '/placeholder-fish.svg'} alt={entry.fishSpeciesName || 'Złowiona ryba'} layout="fill" objectFit="cover" />
                      </div>
                      <div>
                        <p className={`font-semibold text-foreground`}>{entry.fishSpeciesName}</p> 
                        {/* LogbookEntryDto has catchTime (string) */}
                        <p className={`text-xs ${cardMutedTextColorClass}`}>{entry.catchTime ? formatDateInternal(entry.catchTime) : 'Brak danych'}</p>
                      </div>
                      <div className="ml-auto text-right">
                        {/* LogbookEntryDto has lengthInCm, weightInKg */}
                        {entry.lengthInCm && <p className={`text-xs ${cardMutedTextColorClass}`}>{entry.lengthInCm} cm</p>}
                        {entry.weightInKg && <p className={`text-xs ${cardMutedTextColorClass}`}>{entry.weightInKg} kg</p>}
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

        {/* Sidebar (e.g., Fish Species) */}
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
                  {/* Assuming FishSpeciesDto has id and name */}
                  {definedSpeciesForDisplay.map((species: FishSpeciesDto) => (
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
