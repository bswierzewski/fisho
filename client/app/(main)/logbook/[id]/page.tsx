// app/(main)/logbook/[catchId]/page.tsx
import { Fishery, LogbookEntry } from '@/lib/definitions';
import { staticFisheries, staticLogbookEntries } from '@/lib/static-data';
import { ArrowLeft, CalendarDays, Edit, MapPin, Ruler, Trash2, Weight } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';
import { notFound } from 'next/navigation';

import { Button } from '@/components/ui/button';

// Ikony

// Style (dopasuj do reszty aplikacji)
const cardBodyBgClass = 'bg-card';
const cardTextColorClass = 'text-foreground';
const cardMutedTextColorClass = 'text-muted-foreground';

const formatDate = (date: Date) => {
  return date.toLocaleString('pl-PL', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
};

export default async function LogbookEntryDetailPage({ params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  const entry = staticLogbookEntries.find((e) => e.id === id);

  if (!entry) {
    notFound();
  }

  // Znajdź powiązane łowisko, jeśli istnieje
  const fishery = entry.fisheryId ? staticFisheries.find((f) => f.id === entry.fisheryId) : null;

  // Logika warunkowa dla przycisków (na razie uproszczona)
  const canEditOrDelete = true; // Załóżmy, że użytkownik jest właścicielem wpisu

  return (
    <div className="space-y-6">
      {/* Przycisk Powrotu i Tytuł */}
      <div className="flex items-center justify-between">
        <Link href="/logbook">
          <Button variant="outline" size="sm">
            <ArrowLeft className="mr-2 h-4 w-4" /> Wróć do Dziennika
          </Button>
        </Link>
        {/* Opcjonalnie: przyciski edycji/usuwania */}
        {canEditOrDelete && (
          <div className="flex gap-2">
            <Link href={`/logbook/${entry.id}/edit`}>
              {' '}
              {/* Placeholder dla edycji */}
              <Button variant="outline" size="icon">
                <Edit className="h-4 w-4" />
                <span className="sr-only">Edytuj</span>
              </Button>
            </Link>
            <Button variant="destructive" size="icon">
              {' '}
              {/* Placeholder dla usuwania */}
              <Trash2 className="h-4 w-4" />
              <span className="sr-only">Usuń</span>
            </Button>
          </div>
        )}
      </div>

      {/* Główna Karta Szczegółów Połowu */}
      <div className={`overflow-hidden rounded-lg border border-border shadow ${cardBodyBgClass}`}>
        {/* Duże Zdjęcie Ryby */}
        <div className="relative w-full aspect-[4/3] sm:aspect-video bg-muted">
          {' '}
          {/* Proporcje obrazka */}
          <Image
            src={entry.photoUrl}
            alt={`Zdjęcie ${entry.species}`}
            layout="fill"
            objectFit="contain" // Użyj 'contain', aby cała ryba była widoczna
            priority
          />
        </div>

        {/* Informacje o Połowie */}
        <div className="p-4 sm:p-6 space-y-4">
          <h1 className={`text-2xl sm:text-3xl font-bold ${cardTextColorClass}`}>{entry.species}</h1>

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-x-6 gap-y-3 text-sm">
            <div className="flex items-center">
              <CalendarDays className={`mr-2 h-5 w-5 ${cardMutedTextColorClass}`} />
              <span className={cardTextColorClass}>Data połowu:</span>
              <span className={`ml-1 font-medium ${cardTextColorClass}`}>{formatDate(entry.catchTime)}</span>
            </div>

            {entry.lengthCm && (
              <div className="flex items-center">
                <Ruler className={`mr-2 h-5 w-5 ${cardMutedTextColorClass}`} />
                <span className={cardTextColorClass}>Długość:</span>
                <span className={`ml-1 font-medium ${cardTextColorClass}`}>{entry.lengthCm} cm</span>
              </div>
            )}

            {entry.weightKg && (
              <div className="flex items-center">
                <Weight className={`mr-2 h-5 w-5 ${cardMutedTextColorClass}`} />
                <span className={cardTextColorClass}>Waga:</span>
                <span className={`ml-1 font-medium ${cardTextColorClass}`}>{entry.weightKg} kg</span>
              </div>
            )}

            {fishery && (
              <div className="flex items-center sm:col-span-2">
                {' '}
                {/* Łowisko może zająć całą szerokość */}
                <MapPin className={`mr-2 h-5 w-5 ${cardMutedTextColorClass}`} />
                <span className={cardTextColorClass}>Łowisko:</span>
                <Link href={`/fisheries/${fishery.id}`} className="ml-1 font-medium text-primary hover:underline">
                  {fishery.name}
                </Link>
              </div>
            )}
          </div>

          {/* Notatki */}
          {entry.notes && (
            <div>
              <h2 className={`text-lg font-semibold mb-1 ${cardTextColorClass}`}>Notatki:</h2>
              <p className={`text-sm whitespace-pre-wrap ${cardMutedTextColorClass}`}>{entry.notes}</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
