// app/(main)/competitions/add/page.tsx
'use client';

import { availableMainScoringCategories, availableSpecialCategories } from '@/lib/static-data';
// Import Checkbox
import {
  ArrowLeft,
  Award,
  Calendar,
  FileText,
  ImagePlus,
  ListChecks,
  MapPin,
  ShieldCheck,
  Trophy,
  Users
} from 'lucide-react';
import Link from 'next/link';
import { useState } from 'react';

import { Button } from '@/components/ui/button';
import { Checkbox } from '@/components/ui/checkbox';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Textarea } from '@/components/ui/textarea';

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// app/(main)/competitions/add/page.tsx

// Importuj kategorie

const staticUsersForJudges = [
  { id: 'user1', name: 'Marek Sędziowski' },
  { id: 'user2', name: 'Anna Sprawiedliwa' }
];

const cardBodyBgClass = 'bg-card';
const cardTextColorClass = 'text-foreground';
const cardMutedTextColorClass = 'text-muted-foreground';

export default function AddCompetitionPage() {
  const [selectedImagePreview, setSelectedImagePreview] = useState<string | null>(null);
  const [selectedSpecialCategories, setSelectedSpecialCategories] = useState<string[]>([]);

  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      setSelectedImagePreview(URL.createObjectURL(file));
    } else {
      setSelectedImagePreview(null);
    }
  };

  const handleSpecialCategoryChange = (categoryId: string) => {
    setSelectedSpecialCategories((prev) =>
      prev.includes(categoryId) ? prev.filter((id) => id !== categoryId) : [...prev, categoryId]
    );
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const data = Object.fromEntries(formData.entries());
    // Dodaj wybrane kategorie specjalne do danych
    const submissionData = {
      ...data,
      selectedSpecialCategories: selectedSpecialCategories
    };
    console.log('Formularz dodawania zawodów wysłany:', submissionData);
    alert('Zawody dodane (symulacja)!');
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <Link href="/competitions">
          <Button variant="outline" size="sm">
            <ArrowLeft className="mr-2 h-4 w-4" /> Wróć do Listy Zawodów
          </Button>
        </Link>
        <h1 className={`text-xl sm:text-2xl font-bold ${cardTextColorClass}`}>Stwórz Nowe Zawody</h1>
        <div></div>
      </div>

      <form
        onSubmit={handleSubmit}
        className={`p-4 sm:p-6 rounded-lg border border-border shadow ${cardBodyBgClass} space-y-6`}
      >
        {/* ... (Pola: Nazwa, Data, Lokalizacja, Typ, Regulamin - bez zmian) ... */}
        <div>
          <Label
            htmlFor="competition-name"
            className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}
          >
            <Trophy className="mr-2 h-5 w-5" /> Nazwa Zawodów (Wymagane)
          </Label>
          <Input
            id="competition-name"
            name="competition_name"
            type="text"
            placeholder="Np. Puchar Wiosny"
            className="bg-card border-border"
            required
          />
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div>
            <Label htmlFor="start-time" className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}>
              <Calendar className="mr-2 h-5 w-5" /> Data i Czas Rozpoczęcia (Wymagane)
            </Label>
            <Input
              id="start-time"
              name="start_time"
              type="datetime-local"
              className="bg-card border-border"
              defaultValue={new Date().toISOString().substring(0, 16)}
              required
            />
          </div>
          <div>
            <Label htmlFor="end-time" className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}>
              <Calendar className="mr-2 h-5 w-5 opacity-70" /> Data i Czas Zakończenia (Wymagane)
            </Label>
            <Input id="end-time" name="end_time" type="datetime-local" className="bg-card border-border" required />
          </div>
        </div>

        <div>
          <Label htmlFor="location" className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}>
            <MapPin className="mr-2 h-5 w-5" /> Lokalizacja (Wymagane)
          </Label>
          <Input
            id="location"
            name="location"
            type="text"
            placeholder="Np. Jezioro Miejskie"
            className="bg-card border-border"
            required
          />
        </div>

        <div>
          <Label
            htmlFor="competition-type"
            className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}
          >
            <Users className="mr-2 h-5 w-5" /> Typ Zawodów (Wymagane)
          </Label>
          <Select name="type" defaultValue="open" required>
            <SelectTrigger className="w-full bg-card border-border">
              <SelectValue placeholder="Wybierz typ zawodów..." />
            </SelectTrigger>
            <SelectContent className="bg-popover">
              <SelectItem value="open">Otwarte</SelectItem>
              <SelectItem value="closed">Zamknięte</SelectItem>
            </SelectContent>
          </Select>
        </div>

        <div>
          <Label htmlFor="rules" className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}>
            <FileText className="mr-2 h-5 w-5" /> Regulamin (Opcjonalne)
          </Label>
          <Textarea
            id="rules"
            name="rules_text"
            placeholder="Wpisz regulamin zawodów..."
            className="bg-card border-border min-h-[100px]"
          />
        </div>

        {/* --- NOWE: Sekcja Główna Kategoria Punktacji --- */}
        <div>
          <Label
            htmlFor="main-scoring-category"
            className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}
          >
            <ListChecks className="mr-2 h-5 w-5" /> Główna Kategoria Punktacji (Wymagane)
          </Label>
          <Select name="main_scoring_category_id" required>
            <SelectTrigger className="w-full bg-card border-border">
              <SelectValue placeholder="Wybierz główną metodę klasyfikacji..." />
            </SelectTrigger>
            <SelectContent className="bg-popover">
              {availableMainScoringCategories.map((category) => (
                <SelectItem key={category.id} value={category.id}>
                  {category.name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        {/* --- NOWE: Sekcja Kategorie Specjalne --- */}
        <div>
          <Label className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-2`}>
            <Award className="mr-2 h-5 w-5" /> Kategorie Specjalne (Opcjonalne)
          </Label>
          <div className="space-2">
            {/* Zwiększono nieco odstęp */}
            {availableSpecialCategories.map((category) => (
              <div key={category.id} className="flex items-center space-x-3 p-2 rounded-md">
                {/* Dodano padding i hover dla całego wiersza */}
                <Checkbox
                  id={`special-cat-${category.id}`}
                  onCheckedChange={(checked) => {
                    if (typeof checked === 'boolean') {
                      handleSpecialCategoryChange(category.id);
                    }
                  }}
                  checked={selectedSpecialCategories.includes(category.id)}
                  // Dodajemy klasę dla obramowania, gdy nie jest zaznaczony
                  // Możesz dostosować kolor i grubość obramowania
                  className={
                    !selectedSpecialCategories.includes(category.id)
                      ? 'border border-border data-[state=checked]:border-primary'
                      : 'data-[state=checked]:border-primary'
                  }
                />
                <Label htmlFor={`special-cat-${category.id}`} className="text-sm font-normal cursor-pointer flex-grow">
                  {/* Dodano cursor-pointer i flex-grow */}
                  {category.name}
                  {category.description && (
                    <span className={`block text-xs ${cardMutedTextColorClass}`}>{category.description}</span>
                  )}
                </Label>
              </div>
            ))}
          </div>
        </div>

        {/* ... (Pola: Wyznacz Sędziego, Zdjęcie Zawodów - bez zmian) ... */}
        <div>
          <Label htmlFor="judge" className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}>
            <ShieldCheck className="mr-2 h-5 w-5" /> Wyznacz Sędziego (Opcjonalne)
          </Label>
          <Select name="judge_id">
            <SelectTrigger className="w-full bg-card border-border">
              <SelectValue placeholder="Wybierz sędziego..." />
            </SelectTrigger>
            <SelectContent className="bg-popover">
              <SelectItem value="none">Nie wyznaczaj teraz</SelectItem>
              {staticUsersForJudges.map((user) => (
                <SelectItem key={user.id} value={user.id}>
                  {user.name}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </div>

        <div>
          <Label
            htmlFor="competition-photo"
            className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-2`}
          >
            <ImagePlus className="mr-2 h-5 w-5" /> Zdjęcie Zawodów (Opcjonalne)
          </Label>
          <div className="mt-1 flex justify-center rounded-md border-2 border-dashed border-border px-6 pt-5 pb-6 hover:border-primary transition-colors">
            {/* ... (logika uploadu zdjęcia bez zmian) ... */}
            <div className="space-y-1 text-center">
              {selectedImagePreview ? (
                <img
                  src={selectedImagePreview}
                  alt="Podgląd zdjęcia zawodów"
                  className="mx-auto h-32 w-auto rounded-md object-contain"
                />
              ) : (
                <ImagePlus className={`mx-auto h-12 w-12 ${cardMutedTextColorClass}`} />
              )}
              <div className="flex text-sm text-muted-foreground">
                <label
                  htmlFor="competition-photo-input"
                  className="relative cursor-pointer rounded-md bg-card font-medium text-primary focus-within:outline-none focus-within:ring-2 focus-within:ring-primary focus-within:ring-offset-2 focus-within:ring-offset-card hover:text-primary/80"
                >
                  <span>Załaduj plik</span>
                  <input
                    id="competition-photo-input"
                    name="image_url"
                    type="file"
                    className="sr-only"
                    accept="image/*"
                    onChange={handleImageChange}
                  />
                </label>
                <p className="pl-1">lub przeciągnij i upuść</p>
              </div>
              <p className="text-xs text-muted-foreground">PNG, JPG, GIF do 10MB</p>
            </div>
          </div>
        </div>

        <div className="pt-2">
          <Button type="submit" className="w-full bg-primary text-primary-foreground hover:bg-primary/90">
            Stwórz Zawody
          </Button>
        </div>
      </form>
    </div>
  );
}
