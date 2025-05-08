// lib/definitions.ts
export interface Competition {
  id: string;
  name: string;
  startTime: Date;
  location: string;
  status: 'upcoming' | 'ongoing' | 'finished';
  type: 'open' | 'closed';
  imageUrl?: string; // Dodano opcjonalny URL obrazka
}

export interface LogbookEntry {
  id: string;
  species: string;
  catchTime: Date;
  photoUrl: string; // Uznajemy za wymagane dla UI
  lengthCm?: number | null; // Opcjonalna długość
  weightKg?: number | null; // Opcjonalna waga
  fisheryId?: string | null; // Opcjonalne powiązanie z łowiskiem
}

export interface Fishery {
  id: string;
  name: string;
  location: string;
  imageUrl?: string; // Dodano opcjonalny URL obrazka
}
