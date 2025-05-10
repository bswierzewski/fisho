'use client';

import { Competition } from '@/lib/definitions';
import { availableMainScoringCategories, availableSpecialCategories, staticCompetitions } from '@/lib/static-data';
import { Award, Edit, Filter, ListChecks, Plus, Search, ShieldCheck, Trophy, UserCheck } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';

// Upewnij się, że Competition ma pola kategorii
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';

// Style nagłówka karty (dopasuj do navbara)
const cardHeaderBgClass = 'bg-slate-800';
const cardHeaderTextColorClass = 'text-slate-100';
const cardBodyBgClass = 'bg-card';
const cardTextColorClass = 'text-foreground';
const cardMutedTextColorClass = 'text-muted-foreground';

// Symulacja ID zalogowanego użytkownika i jego ról w zawodach
const currentUserId = 'user-logged-in';
const userRolesInCompetitions: { competitionId: string; role: 'organizer' | 'judge' | 'participant' }[] = [
  { competitionId: 'comp-1', role: 'participant' },
  { competitionId: 'comp-2', role: 'organizer' },
  { competitionId: 'comp-3', role: 'judge' }
];

export default function MyCompetitionsPage() {
  const myCompetitions = staticCompetitions
    .filter((comp) => userRolesInCompetitions.some((roleInfo) => roleInfo.competitionId === comp.id)) // Poprawione filtrowanie
    .map((comp) => {
      const roleInfo = userRolesInCompetitions.find((r) => r.competitionId === comp.id);
      return { ...comp, userRole: roleInfo?.role };
    });

  // --- Funkcje pomocnicze do pobierania nazw kategorii ---
  const getMainCategoryName = (id?: string | null): string | null => {
    if (!id) return null;
    return availableMainScoringCategories.find((cat) => cat.id === id)?.name || null;
  };

  const getSpecialCategoryNames = (ids?: string[]): string[] => {
    if (!ids || ids.length === 0) return [];
    return ids.map((id) => availableSpecialCategories.find((cat) => cat.id === id)?.name).filter(Boolean) as string[];
  };

  const getRoleIconAndLabel = (role?: 'organizer' | 'judge' | 'participant') => {
    switch (role) {
      case 'organizer':
        return { icon: <Edit className="h-3 w-3" />, label: 'Organizator' };
      case 'judge':
        return { icon: <ShieldCheck className="h-3 w-3" />, label: 'Sędzia' };
      case 'participant':
        return { icon: <UserCheck className="h-3 w-3" />, label: 'Uczestnik' };
      default:
        return { icon: null, label: '' };
    }
  };

  return (
    <div className="space-y-6">
      {/* Nagłówek strony (bez zmian) */}
      <div className="flex flex-col sm:flex-row justify-between items-center gap-4">
        <h1 className="text-2xl sm:text-3xl font-bold text-foreground">Moje Zawody</h1>
        <Link href="/competitions/add">
          <Button className="bg-accent text-accent-foreground hover:bg-accent/90 w-full sm:w-auto">
            <Plus className="mr-2 h-4 w-4" /> Stwórz Nowe Zawody
          </Button>
        </Link>
      </div>

      {/* Pasek Wyszukiwania i Filtrowania (bez zmian) */}
      <div className="flex flex-col sm:flex-row gap-2">
        <div className="relative flex-grow">
          <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
          <Input
            type="search"
            placeholder="Szukaj w moich zawodach..."
            className="w-full rounded-lg bg-card pl-9 border-border"
          />
        </div>
        <Button variant="outline">
          <Filter className="mr-2 h-4 w-4" /> Filtruj (np. po statusie, roli)
        </Button>
      </div>

      {/* Siatka Kart Moich Zawodów */}
      {myCompetitions.length > 0 ? (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 sm:gap-6">
          {myCompetitions.map((comp) => {
            const roleDisplay = getRoleIconAndLabel(comp.userRole);
            const mainCategoryName = getMainCategoryName(comp.main_scoring_category_id);
            const specialCategoryNames = getSpecialCategoryNames(comp.selected_special_category_ids);

            return (
              // Opakowujemy całą kartę w Link
              <Link key={comp.id} href={`/competitions/${comp.id}`} className="group flex flex-col">
                <div className="flex flex-col flex-grow overflow-hidden rounded-lg border border-border bg-card shadow transition-shadow group-hover:shadow-md">
                  {/* Nagłówek Karty */}
                  <div
                    className={`${cardHeaderBgClass} ${cardHeaderTextColorClass} relative flex h-10 flex-shrink-0 items-center justify-between p-3`}
                  >
                    <div className="flex items-center space-x-2">
                      {comp.imageUrl && (
                        <div className="absolute inset-0 z-0">
                          <Image src={comp.imageUrl} alt="" layout="fill" objectFit="cover" className="opacity-20" />
                        </div>
                      )}
                      <div className="relative z-10 flex items-center space-x-2">
                        <Trophy className="h-4 w-4" />
                        <span className="text-xs font-medium truncate">
                          {comp.status === 'upcoming'
                            ? 'Nadchodzące'
                            : comp.status === 'ongoing'
                              ? 'Trwające'
                              : 'Zakończone'}
                        </span>
                      </div>
                    </div>
                    {roleDisplay.icon && (
                      <div className="relative z-10 flex items-center space-x-1 rounded-full bg-black/20 px-2 py-0.5">
                        {roleDisplay.icon}
                        <span className="text-[10px] font-medium">{roleDisplay.label}</span>
                      </div>
                    )}
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
                            <span className="ml-1.5 line-clamp-2">{specialCategoryNames.join(', ')}</span>
                          </div>
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              </Link>
            );
          })}
        </div>
      ) : (
        <div className="mt-8 rounded-lg border border-dashed border-border bg-card p-8 text-center">
          <Trophy className="mx-auto h-12 w-12 text-muted-foreground mb-4" />
          <p className="text-muted-foreground mb-2">
            Nie jesteś jeszcze zapisany/a na żadne zawody ani żadnych nie organizujesz.
          </p>
          <Link href="/competitions" className="inline-block mr-2">
            <Button variant="outline">Przeglądaj Otwarte Zawody</Button>
          </Link>
          <Link href="/competitions/add" className="inline-block">
            <Button className="bg-accent text-accent-foreground hover:bg-accent/90">
              <Plus className="mr-2 h-4 w-4" /> Stwórz Zawody
            </Button>
          </Link>
        </div>
      )}
    </div>
  );
}
