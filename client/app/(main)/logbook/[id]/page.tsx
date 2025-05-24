// app/(main)/logbook/[catchId]/page.tsx
"use client"; // Required for using client-side hooks

import { useDeleteExistingLogbookEntry, useGetLogbookEntryDetailsById, getGetCurrentUserLogbookEntriesQueryKey } from '@/lib/api/endpoints/logbook';
import { LogbookEntryDto, HttpValidationProblemDetails, ProblemDetails } from '@/lib/api/models';
import { ArrowLeft, CalendarDays, Edit, MapPin, Ruler, Trash2, Weight } from 'lucide-react';
import Image from 'next/image';
import Link from 'next/link';
import { notFound, useRouter, useParams } from 'next/navigation'; // Import useParams & useRouter
import { toast } from 'react-hot-toast';
import { useQueryClient } from '@tanstack/react-query'; // Import useQueryClient

import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton'; // For loading state
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"; // For error state
import { Terminal } from "lucide-react"; // For error state icon

// Ikony

// Style (dopasuj do reszty aplikacji)
const cardBodyBgClass = 'bg-card';
const cardTextColorClass = 'text-foreground';
const cardMutedTextColorClass = 'text-muted-foreground';

const formatDate = (date: Date | string) => {
  const dateObj = typeof date === 'string' ? new Date(date) : date;
  return dateObj.toLocaleString('pl-PL', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });
};

export default function LogbookEntryDetailPage() {
  const params = useParams();
  const router = useRouter();
  const queryClient = useQueryClient(); // Get queryClient instance
  const id = params.id as string;

  const { data: entry, isLoading, isError, error } = useGetLogbookEntryDetailsById(parseInt(id, 10), {
    query: {
      enabled: !!id,
      refetchOnMount: "always",
    }
  });

  const {
    mutate: deleteLogbookEntry,
    isPending: isDeletingEntry,
  } = useDeleteExistingLogbookEntry();

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <Skeleton className="h-9 w-36" /> {/* Back button placeholder */}
          <div className="flex gap-2">
            <Skeleton className="h-9 w-9" /> {/* Edit button placeholder */}
            <Skeleton className="h-9 w-9" /> {/* Delete button placeholder */}
          </div>
        </div>
        <div className={`overflow-hidden rounded-lg border border-border shadow ${cardBodyBgClass}`}>
          <Skeleton className="relative w-full aspect-[4/3] sm:aspect-video bg-muted" /> {/* Image placeholder */}
          <div className="p-4 sm:p-6 space-y-4">
            <Skeleton className="h-8 w-3/4" /> {/* Species name placeholder */}
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-x-6 gap-y-3 text-sm">
              <Skeleton className="h-5 w-1/2" /> {/* Date placeholder */}
              <Skeleton className="h-5 w-1/2" /> {/* Length placeholder */}
              <Skeleton className="h-5 w-1/2" /> {/* Weight placeholder */}
              <Skeleton className="h-5 w-full sm:col-span-2" /> {/* Fishery placeholder */}
            </div>
            <div>
              <Skeleton className="h-6 w-1/4 mb-1" /> {/* Notes title placeholder */}
              <Skeleton className="h-10 w-full" /> {/* Notes content placeholder */}
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (isError) {
    let errorMessage = 'Spróbuj ponownie później.';
    if (error) {
      if (typeof error === 'object' && error !== null) {
        if ('detail' in error && typeof (error as any).detail === 'string') {
          errorMessage = (error as any).detail;
        } else if ('title' in error && typeof (error as any).title === 'string') {
          errorMessage = (error as any).title;
        } else if ('message' in error && typeof (error as any).message === 'string') {
          errorMessage = (error as any).message;
        }
      }
    }

    return (
      <Alert variant="destructive" className="mt-4">
        <Terminal className="h-4 w-4" />
        <AlertTitle>Błąd</AlertTitle>
        <AlertDescription>
          Nie udało się załadować szczegółów wpisu. {errorMessage}
        </AlertDescription>
      </Alert>
    );
  }


  if (!entry) {
    notFound();
  }

  // Znajdź powiązane łowisko, jeśli istnieje - This logic might need adjustment based on API response
  // const fishery = entry.fisheryId ? staticFisheries.find((f) => f.id === entry.fisheryId) : null; // Removed
  // For now, assuming fishery details are part of the entry or fetched separately if needed.
  const fisheryName = entry.fisheryName;
  const fisheryId = entry.fisheryId;

  const handleDeleteEntry = () => {
    if (!entry || !entry.id) {
      toast.error('Błąd: ID wpisu jest nieznane lub wpis nie istnieje.');
      return;
    }

    if (window.confirm('Czy na pewno chcesz usunąć ten wpis z dziennika? Tej operacji nie można cofnąć.')) {
      deleteLogbookEntry(
        { id: entry.id },
        {
          onSuccess: () => {
            toast.success('Wpis w dzienniku został pomyślnie usunięty!');
            // Invalidate the query for the logbook entries list
            queryClient.invalidateQueries({
              queryKey: getGetCurrentUserLogbookEntriesQueryKey({ PageNumber: 1, PageSize: 20 }),
            });
            router.push('/logbook');
          },
          onError: (error: HttpValidationProblemDetails | ProblemDetails) => {
            console.error('Error deleting logbook entry:', error);
            let errorMessage = 'Nie udało się usunąć wpisu. Spróbuj ponownie.';
            if ('title' in error && error.title) {
              errorMessage = error.title;
            } else if ('detail' in error && error.detail) {
              errorMessage = error.detail;
            } else if ('message' in error && typeof error.message === 'string') {
              errorMessage = error.message;
            }
            toast.error(errorMessage);
          },
        }
      );
    }
  };

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
            <Button 
              variant="destructive" 
              size="icon" 
              onClick={handleDeleteEntry}
              disabled={isDeletingEntry || isLoading} // Disable if deleting or initial load in progress
            >
              {isDeletingEntry ? (
                <span className="animate-spin h-4 w-4 border-2 border-current border-t-transparent rounded-full" />
              ) : (
                <Trash2 className="h-4 w-4" />
              )}
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
            src={entry.imageUrl || '/placeholder-image.jpg'} // Changed to imageUrl and added fallback
            alt={`Zdjęcie ${entry.fishSpeciesName || 'ryby'}`} // Changed to fishSpeciesName and added fallback
            layout="fill"
            objectFit="contain" // Użyj 'contain', aby cała ryba była widoczna
            priority
          />
        </div>

        {/* Informacje o Połowie */}
        <div className="p-4 sm:p-6 space-y-4">
          <h1 className={`text-2xl sm:text-3xl font-bold ${cardTextColorClass}`}>{entry.fishSpeciesName || 'Nieznany gatunek'}</h1> {/* Changed to fishSpeciesName and added fallback */}

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-x-6 gap-y-3 text-sm">
            <div className="flex items-center">
              <CalendarDays className={`mr-2 h-5 w-5 ${cardMutedTextColorClass}`} />
              <span className={cardTextColorClass}>Data połowu:</span>
              <span className={`ml-1 font-medium ${cardTextColorClass}`}>{entry.catchTime ? formatDate(entry.catchTime) : 'Brak danych'}</span>
            </div>

            {entry.lengthInCm && (
              <div className="flex items-center">
                <Ruler className={`mr-2 h-5 w-5 ${cardMutedTextColorClass}`} />
                <span className={cardTextColorClass}>Długość:</span>
                <span className={`ml-1 font-medium ${cardTextColorClass}`}>{entry.lengthInCm} cm</span> {/* Changed to lengthInCm */}
              </div>
            )}

            {entry.weightInKg && (
              <div className="flex items-center">
                <Weight className={`mr-2 h-5 w-5 ${cardMutedTextColorClass}`} />
                <span className={cardTextColorClass}>Waga:</span>
                <span className={`ml-1 font-medium ${cardTextColorClass}`}>{entry.weightInKg} kg</span> {/* Changed to weightInKg */}
              </div>
            )}

            {fisheryName && fisheryId && (
              <div className="flex items-center sm:col-span-2">
                {' '}
                {/* Łowisko może zająć całą szerokość */}
                <MapPin className={`mr-2 h-5 w-5 ${cardMutedTextColorClass}`} />
                <span className={cardTextColorClass}>Łowisko:</span>
                <Link href={`/fisheries/${fisheryId}`} className="ml-1 font-medium text-primary hover:underline">
                  {fisheryName}
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
