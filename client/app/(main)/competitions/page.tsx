'use client';

import { Competition } from '@/lib/definitions';
import {
  availableMainScoringCategories,
  // <<< IMPORT
  availableSpecialCategories,
  // <<< IMPORT
  staticCompetitions
} from '@/lib/static-data';
import { Award, Filter, ListChecks, Plus, Search, Trophy } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';

// Upewnij się, że Competition ma pola main_scoring_category_id i selected_special_category_ids
import { Button } from '@/components/ui/button';
// <<< DODANO ListChecks, Award
import { Input } from '@/components/ui/input';

// Style nagłówka karty (dopasuj do navbara)
const cardHeaderBgClass = 'bg-slate-800'; // Użyj swoich zdefiniowanych klas lub zmiennych CSS
const cardHeaderTextColorClass = 'text-slate-100';
const cardBodyBgClass = 'bg-card';
const cardTextColorClass = 'text-foreground';
const cardMutedTextColorClass = 'text-muted-foreground';

export default function CompetitionsPage() {
  const openCompetitions = staticCompetitions.filter((c) => c.type === 'open' && c.status !== 'finished');

  // --- Funkcje pomocnicze do pobierania nazw kategorii ---
  const getMainCategoryName = (id?: string | null): string | null => {
    if (!id) return null;
    return availableMainScoringCategories.find((cat) => cat.id === id)?.name || null;
  };

  const getSpecialCategoryNames = (ids?: string[]): string[] => {
    if (!ids || ids.length === 0) return [];
    return ids.map((id) => availableSpecialCategories.find((cat) => cat.id === id)?.name).filter(Boolean) as string[];
  };
  // ---------------------------------------------------------

  // TODO: Dodać logikę wyszukiwania i filtrowania

  return (
    <div className="space-y-6">
      {/* Nagłówek strony i przycisk dodawania */}
      <div className="flex flex-col sm:flex-row justify-between items-center gap-4">
        <h1 className="text-2xl sm:text-3xl font-bold text-foreground">Otwarte Zawody</h1>
        <Link href="/competitions/add">
          <Button className="bg-accent text-accent-foreground hover:bg-accent/90 w-full sm:w-auto">
            <Plus className="mr-2 h-4 w-4" /> Stwórz Nowe Zawody
          </Button>
        </Link>
      </div>

      {/* Pasek Wyszukiwania i Filtrowania (Opcjonalny) */}
      <div className="flex flex-col sm:flex-row gap-2">
        <div className="relative flex-grow">
          <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <Input
            type="search"
            placeholder="Szukaj zawodów..."
            className="w-full rounded-lg bg-card pl-9 border-border"
          />
        </div>
        <Button variant="outline">
          <Filter className="mr-2 h-4 w-4" /> Filtruj
        </Button>
      </div>

      {/* Siatka Kart Zawodów */}
      {openCompetitions.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 sm:gap-6">
          {openCompetitions.map((comp: Competition) => {
            const mainCategoryName = getMainCategoryName(comp.main_scoring_category_id);
            const specialCategoryNames = getSpecialCategoryNames(comp.selected_special_category_ids);

            return (
              // Opakowujemy całą kartę w Link
              <Link href={`/competitions/${comp.id}`} key={comp.id} className="group flex flex-col">
                <div className="flex flex-col flex-grow overflow-hidden rounded-lg border border-border bg-card shadow transition-shadow group-hover:shadow-md">
                  {/* Nagłówek Karty */}
                  <div
                    className={`${cardHeaderBgClass} ${cardHeaderTextColorClass} relative flex h-10 flex-shrink-0 items-center space-x-2 p-3`}
                  >
                    {comp.imageUrl && (
                      <Image src={comp.imageUrl} alt="" layout="fill" objectFit="cover" className="opacity-20 z-0" />
                    )}
                    <div className="relative z-10 flex items-center space-x-2">
                      <Trophy className="h-4 w-4" />
                      <span className="text-xs font-medium truncate">Zawody</span>
                    </div>
                  </div>
                  {/* Treść Karty */}
                  <div className={`flex flex-grow flex-col justify-between p-3 ${cardBodyBgClass}`}>
                    <div>
                      <h3 className={`mb-1 truncate text-base font-semibold ${cardTextColorClass}`}>{comp.name}</h3>
                      <p className={`text-xs ${cardMutedTextColorClass}`}>
                        {comp.startTime.toLocaleDateString('pl-PL')}
                      </p>
                      <p className={`truncate text-xs ${cardMutedTextColorClass}`}>{comp.location}</p>

                      {/* Informacje o Kategoriach */}
                      {mainCategoryName && (
                        <div className="mt-2 pt-2 border-t border-border/50">
                          <div className="flex items-center text-xs text-muted-foreground">
                            <ListChecks className="mr-1.5 h-3.5 w-3.5 text-primary" />
                            <span className="font-medium text-foreground/90">Główna:</span>
                            <span className="ml-1.5 truncate">{mainCategoryName}</span>
                          </div>
                        </div>
                      )}
                      {specialCategoryNames.length > 0 && (
                        <div className={`mt-1.5 ${!mainCategoryName ? 'pt-2 border-t border-border/50' : ''}`}>
                          <div className="flex items-start text-xs text-muted-foreground">
                            <Award className="mr-1.5 h-3.5 w-3.5 text-amber-500 mt-0.5 flex-shrink-0" />
                            <span className="font-medium text-foreground/90">Specjalne:</span>
                            <span className="ml-1.5 line-clamp-2">{specialCategoryNames.join(', ')}</span>{' '}
                            {/* Ograniczenie do 2 linii */}
                          </div>
                        </div>
                      )}
                    </div>
                    {/* Przycisk "Zobacz Szczegóły" nie jest już potrzebny, bo cała karta jest linkiem */}
                  </div>
                </div>
              </Link>
            );
          })}
        </div>
      ) : (
        <div className="mt-8 rounded-lg border border-dashed border-border bg-card p-8 text-center">
          <p className="text-muted-foreground">Nie znaleziono otwartych zawodów.</p>
          <Link href="/competitions/add" className="mt-4 inline-block">
            <Button className="bg-accent text-accent-foreground hover:bg-accent/90">
              <Plus className="mr-2 h-4 w-4" /> Stwórz Pierwsze Zawody
            </Button>
          </Link>
        </div>
      )}
    </div>
  );
}
