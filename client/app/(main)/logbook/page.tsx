'use client';

// Dla przyszłych interakcji
import { LogbookEntry } from '@/lib/definitions';
import { staticLogbookEntries } from '@/lib/static-data';
import { Filter, Fish, Plus, Search } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';

import { useGetCurrentUserLogbookEntries } from '@/lib/api/endpoints/logbook';
import { UserLogbookEntryDto } from '@/lib/api/models';

import { Button } from '@/components/ui/button';
// Dodano ikony Search i Filter
import { Input } from '@/components/ui/input';

// Style karty (można przenieść do stałych, jeśli się powtarzają)
const cardBodyBgClass = 'bg-card';
const cardTextColorClass = 'text-foreground';
const cardMutedTextColorClass = 'text-muted-foreground';

export default function LogbookPage() {
  const { data: logEntries } = useGetCurrentUserLogbookEntries({
    PageNumber: 1,
    PageSize: 20
  });

  return (
    <div className="space-y-6">
      {/* Nagłówek strony i przycisk dodawania */}
      <div className="flex flex-col sm:flex-row justify-between items-center gap-4">
        <h1 className="text-2xl sm:text-3xl font-bold text-foreground">Mój Dziennik Połowów</h1>
        <Link href="/logbook/add">
          <Button className="bg-accent text-accent-foreground hover:bg-accent/90 w-full sm:w-auto">
            <Plus className="mr-2 h-4 w-4" /> Dodaj Nowy Połów
          </Button>
        </Link>
      </div>

      {/* Pasek Wyszukiwania i Filtrowania (Opcjonalny) */}
      <div className="flex flex-col sm:flex-row gap-2">
        <div className="relative flex-grow">
          <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <Input
            type="search"
            placeholder="Szukaj w dzienniku (np. gatunek)..."
            className="w-full rounded-lg bg-card pl-9 border-border"
          />
        </div>
        <Button variant="outline">
          <Filter className="mr-2 h-4 w-4" /> Filtruj
        </Button>
      </div>

      {/* Siatka Kart Połowów */}
      {logEntries?.items && logEntries?.items.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4 sm:gap-6">
          {logEntries.items.map((entry: UserLogbookEntryDto) => (
            <Link key={entry.id} href={`/logbook/${entry.id}`}>
              <div
                // Można dodać Link do szczegółów wpisu, jeśli planujesz taki widok
                // <Link href={`/logbook/${entry.id}`}>
                className="group block overflow-hidden rounded-lg border border-border bg-card shadow transition-shadow hover:shadow-md"
                // </Link>
              >
                {/* Zdjęcie jako główny element */}
                <div className="relative aspect-square w-full bg-muted">
                  {' '}
                  {/* Używamy aspect-square dla kwadratowych kart */}
                  <Image
                    src={entry.imageUrl ?? ''}
                    alt={entry.fishSpeciesName ?? 'Zdjęcie ryby'}
                    layout="fill"
                    objectFit="cover"
                    className="transition-transform duration-300 group-hover:scale-105" // Efekt lekkiego zoomu na hover
                  />
                </div>
                {/* Treść Karty */}
                <div className={`p-3 ${cardBodyBgClass}`}>
                  <h3 className={`truncate text-base font-semibold ${cardTextColorClass}`}>{entry.fishSpeciesName}</h3>
                  <p className={`text-xs ${cardMutedTextColorClass}`}>{entry.catchTime}</p>
                  {/* Opcjonalnie: Wyświetl wymiary, jeśli dostępne */}
                  {(entry.lengthInCm || entry.weightInKg) && (
                    <p className={`mt-1 truncate text-xs ${cardMutedTextColorClass}`}>
                      {entry.lengthInCm && `${entry.lengthInCm} cm`}
                      {entry.lengthInCm && entry.weightInKg && ' / '}
                      {entry.weightInKg && `${entry.weightInKg} kg`}
                    </p>
                  )}
                </div>
              </div>
            </Link>
          ))}
        </div>
      ) : (
        <div className="mt-8 rounded-lg border border-dashed border-border bg-card p-8 text-center">
          <Fish className="mx-auto h-12 w-12 text-muted-foreground mb-4" />
          <p className="text-muted-foreground mb-4">Twój dziennik połowów jest pusty.</p>
          <Link href="/logbook/add" className="inline-block">
            <Button className="bg-accent text-accent-foreground hover:bg-accent/90">
              <Plus className="mr-2 h-4 w-4" /> Dodaj Swój Pierwszy Połów
            </Button>
          </Link>
        </div>
      )}
    </div>
  );
}
