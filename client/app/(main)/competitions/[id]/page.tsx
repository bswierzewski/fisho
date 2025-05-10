// app/(main)/competitions/[id]/page.tsx
import { Competition } from '@/lib/definitions';
import { availableMainScoringCategories, availableSpecialCategories, staticCompetitions } from '@/lib/static-data';
import {
  Activity,
  Award,
  BarChart3,
  CalendarDays,
  Clock,
  Hourglass,
  Info,
  ListChecks,
  MapPin,
  Plus,
  ShieldCheck,
  Trophy,
  UserCheck,
  Users
} from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';
import { notFound } from 'next/navigation';

import { Button } from '@/components/ui/button';

// Style (dopasuj do reszty aplikacji)
const sectionHeaderBgClass = 'bg-slate-800';
const sectionHeaderTextColorClass = 'text-slate-100';
const cardBodyBgClass = 'bg-card';
const cardTextColorClass = 'text-foreground'; // Użyjemy tej dla głównego tekstu w sekcjach
const cardMutedTextColorClass = 'text-muted-foreground';

const formatDateTime = (date: Date) => {
  return date.toLocaleString('pl-PL', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
};

export default function CompetitionDetailPage({ params }: { params: { id: string } }) {
  const competition = staticCompetitions.find((c) => c.id === params.id);

  if (!competition) {
    notFound();
  }

  const getMainCategoryName = (id?: string | null): string | null => {
    if (!id) return null;
    return availableMainScoringCategories.find((cat) => cat.id === id)?.name || null;
  };

  const getSpecialCategoryNames = (ids?: string[]): string[] => {
    if (!ids || ids.length === 0) return [];
    return ids.map((id) => availableSpecialCategories.find((cat) => cat.id === id)?.name).filter(Boolean) as string[];
  };

  const mainCategoryName = getMainCategoryName(competition.main_scoring_category_id);
  const specialCategoryNames = getSpecialCategoryNames(competition.selected_special_category_ids);

  const organizerName = 'Jan Kowalski (Organizator)';
  const participants = ['Anna Nowak', 'Piotr Wiśniewski', 'Krzysztof Wójcik', 'Gość: Adam Mały', 'Gość: Ewa Zielona'];
  const judges = ['Marek Sędziowski (Sędzia)'];

  const canJoin = competition.type === 'open' && competition.status === 'upcoming';
  const canRegisterCatch = competition.status === 'ongoing';
  const canManage = true;

  return (
    <div className="space-y-6">
      {/* Nagłówek z obrazkiem i tekstem na obrazku (bez zmian) */}
      <div className="relative overflow-hidden rounded-lg border border-border shadow">
        {competition.imageUrl ? (
          <div className="relative h-48 sm:h-64 w-full">
            <Image
              src={competition.imageUrl}
              alt={`Zdjęcie dla ${competition.name}`}
              layout="fill"
              objectFit="cover"
              priority
            />
            <div className="absolute inset-0 bg-gradient-to-t from-black/70 via-black/40 to-transparent"></div>
            <div className="absolute bottom-0 left-0 right-0 p-4 sm:p-6 text-white">
              <h1 className="text-2xl sm:text-3xl lg:text-4xl font-bold mb-1 drop-shadow-md">{competition.name}</h1>
              <div className="flex flex-wrap gap-x-3 gap-y-1 text-xs sm:text-sm opacity-90 drop-shadow-sm">
                <span className="flex items-center">
                  <MapPin className="mr-1 h-3 w-3 sm:h-4 sm:w-4" /> {competition.location}
                </span>
                <span className="flex items-center">
                  <Clock className="mr-1 h-3 w-3 sm:h-4 sm:w-4" /> {formatDateTime(competition.startTime)}
                </span>
                <span className="flex items-center">
                  <Trophy className="mr-1 h-3 w-3 sm:h-4 sm:w-4" /> Typ:{' '}
                  {competition.type === 'open' ? 'Otwarte' : 'Zamknięte'}
                </span>
                <span
                  className={`flex items-center font-medium capitalize ${
                    competition.status === 'upcoming'
                      ? 'text-blue-300'
                      : competition.status === 'ongoing'
                        ? 'text-green-300'
                        : 'text-red-300'
                  }`}
                >
                  <CalendarDays className="mr-1 h-3 w-3 sm:h-4 sm:w-4" /> Status: {competition.status}
                </span>
              </div>
            </div>
          </div>
        ) : (
          <div className={`p-4 sm:p-6 bg-card text-foreground`}>
            <h1 className="text-2xl sm:text-3xl font-bold mb-2">{competition.name}</h1>
            <div className="flex flex-wrap gap-x-4 gap-y-1 text-sm text-muted-foreground">
              <span className="flex items-center">
                <MapPin className="mr-1 h-4 w-4" /> {competition.location}
              </span>
              <span className="flex items-center">
                <Clock className="mr-1 h-4 w-4" /> {formatDateTime(competition.startTime)}
              </span>
              <span className="flex items-center">
                <Trophy className="mr-1 h-4 w-4" /> Typ: {competition.type === 'open' ? 'Otwarte' : 'Zamknięte'}
              </span>
              <span
                className={`flex items-center font-medium capitalize ${
                  competition.status === 'upcoming'
                    ? 'text-blue-500'
                    : competition.status === 'ongoing'
                      ? 'text-green-500'
                      : 'text-red-500'
                }`}
              >
                <CalendarDays className="mr-1 h-4 w-4" /> Status: {competition.status}
              </span>
            </div>
          </div>
        )}
      </div>

      {/* Przyciski Akcji (bez zmian) */}
      <div className="flex flex-wrap gap-2">
        {canJoin && <Button className="bg-accent text-accent-foreground hover:bg-accent/90">Dołącz do Zawodów</Button>}
        {canRegisterCatch && (
          <Button className="bg-blue-600 text-white hover:bg-blue-700">
            <Plus className="mr-2 h-4 w-4" /> Zarejestruj Połów (Sędzia)
          </Button>
        )}
        {canManage && <Button variant="secondary">Zarządzaj Zawodami (Organizator)</Button>}
        {competition.resultsToken && (
          <Link href={`/results/${competition.resultsToken}`}>
            <Button variant="outline" className="border-primary text-primary hover:bg-primary/10 hover:text-primary">
              {competition.status === 'upcoming' && (
                <>
                  <Hourglass className="mr-2 h-4 w-4" /> Śledź Zawody
                </>
              )}
              {competition.status === 'ongoing' && (
                <>
                  <Activity className="mr-2 h-4 w-4 animate-pulse" /> Zobacz Wyniki na Żywo
                </>
              )}
              {competition.status === 'finished' && (
                <>
                  <BarChart3 className="mr-2 h-4 w-4" /> Zobacz Oficjalne Wyniki
                </>
              )}
            </Button>
          </Link>
        )}
      </div>

      {/* Główna Siatka Treści */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Kolumna Główna */}
        <div className="lg:col-span-2 space-y-6">
          {/* Sekcja Informacje / Regulamin / Kategorie */}
          <section>
            <div
              className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
            >
              <Info className="h-5 w-5" />
              <h2 className="text-lg font-semibold">Informacje o Zawodach</h2>
            </div>
            <div className={`p-4 border-x border-b rounded-b-lg ${cardBodyBgClass} space-y-4`}>
              {' '}
              {/* Dodano space-y-4 */}
              <div>
                <h3 className={`font-semibold mb-1 ${cardTextColorClass}`}>Organizator:</h3>
                <p className={`text-sm ${cardMutedTextColorClass}`}>{organizerName}</p>
              </div>
              {/* Wyświetlanie Głównej Kategorii Punktacji */}
              {mainCategoryName && (
                <div>
                  <h3 className={`font-semibold mb-1 ${cardTextColorClass} flex items-center`}>
                    <ListChecks className="mr-2 h-4 w-4 text-primary" /> Główna Kategoria Punktacji:
                  </h3>
                  <p className={`text-sm ${cardMutedTextColorClass}`}>{mainCategoryName}</p>
                </div>
              )}
              {/* Wyświetlanie Kategorii Specjalnych */}
              {specialCategoryNames.length > 0 && (
                <div>
                  <h3 className={`font-semibold mb-1 ${cardTextColorClass} flex items-center`}>
                    <Award className="mr-2 h-4 w-4 text-amber-500" /> Kategorie Specjalne:
                  </h3>
                  <ul className="list-disc list-inside space-y-0.5 pl-1">
                    {specialCategoryNames.map((name, index) => (
                      <li key={index} className={`text-sm ${cardMutedTextColorClass}`}>
                        {name}
                      </li>
                    ))}
                  </ul>
                </div>
              )}
              <div>
                <h3 className={`font-semibold mb-1 ${cardTextColorClass}`}>Regulamin:</h3>
                <p className={`text-sm whitespace-pre-wrap ${cardMutedTextColorClass}`}>
                  {competition.rules_text || 'Brak szczegółowego regulaminu.'}
                </p>
              </div>
            </div>
          </section>

          {/* Sekcja Ranking */}
          <section>
            <div
              className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
            >
              <ListChecks className="h-5 w-5" />
              <h2 className="text-lg font-semibold">Ranking / Tablica Wyników</h2>
            </div>
            <div className={`p-4 border-x border-b rounded-b-lg ${cardBodyBgClass}`}>
              {competition.status === 'upcoming' ? (
                <p className="text-sm text-muted-foreground">Ranking będzie dostępny po rozpoczęciu zawodów.</p>
              ) : (
                <p className="text-sm text-muted-foreground">Tabela wyników (placeholder)...</p>
              )}
            </div>
          </section>
        </div>

        {/* Kolumna Boczna */}
        <div className="space-y-6">
          {/* Sekcja Uczestnicy */}
          <section>
            <div
              className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
            >
              <Users className="h-5 w-5" />
              <h2 className="text-lg font-semibold">Uczestnicy ({participants.length})</h2>
            </div>
            <div className={`p-4 border-x border-b rounded-b-lg ${cardBodyBgClass}`}>
              <ul className="space-y-1 text-sm">
                {participants.map((name, index) => (
                  <li key={index} className={`flex items-center ${cardMutedTextColorClass}`}>
                    <UserCheck className="mr-2 h-4 w-4 flex-shrink-0" /> {name}
                  </li>
                ))}
              </ul>
              {/* W przyszłości dodać przycisk do sędziów, aby mogli dodawać połowy i zarządzać wynikami. */}
            </div>
          </section>

          {/* Sekcja Sędziowie */}
          <section>
            <div
              className={`flex items-center space-x-2 rounded-t-lg p-3 ${sectionHeaderBgClass} ${sectionHeaderTextColorClass}`}
            >
              <ShieldCheck className="h-5 w-5" />
              <h2 className="text-lg font-semibold">Sędziowie ({judges.length})</h2>
            </div>
            <div className={`p-4 border-x border-b rounded-b-lg ${cardBodyBgClass}`}>
              <ul className="space-y-1 text-sm">
                {judges.map((name, index) => (
                  <li key={index} className={`flex items-center ${cardMutedTextColorClass}`}>
                    <ShieldCheck className="mr-2 h-4 w-4 flex-shrink-0" /> {name}
                  </li>
                ))}
              </ul>
              {/* W przyszłości dodać przycisk do sędziów, aby mogli dodawać połowy i zarządzać wynikami. */}
            </div>
          </section>
        </div>
      </div>
    </div>
  );
}
