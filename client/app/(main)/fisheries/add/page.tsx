'use client';

// Formularze zazwyczaj wymagają stanu po stronie klienta
import { availableFishSpecies } from '@/lib/static-data';
import { ArrowLeft, ImagePlus, ListChecks, MapPin } from 'lucide-react';
import Link from 'next/link';
import { useState } from 'react';

import { Button } from '@/components/ui/button';
import { Checkbox } from '@/components/ui/checkbox';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';

// Style (dopasuj do reszty aplikacji)
const cardBodyBgClass = 'bg-card';
const cardTextColorClass = 'text-foreground';
const cardMutedTextColorClass = 'text-muted-foreground';

export default function AddFisheryPage() {
  const [selectedImagePreview, setSelectedImagePreview] = useState<string | null>(null);
  const [selectedSpeciesIds, setSelectedSpeciesIds] = useState<number[]>([]);

  // Symulacja obsługi zmiany pliku
  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      setSelectedImagePreview(URL.createObjectURL(file));
      // W przyszłości: logika uploadu pliku
    } else {
      setSelectedImagePreview(null);
    }
  };

  const handleSpeciesChange = (speciesId: number) => {
    setSelectedSpeciesIds((prev) =>
      prev.includes(speciesId) ? prev.filter((id) => id !== speciesId) : [...prev, speciesId]
    );
  };

  // Symulacja wysłania formularza
  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    // W przyszłości: logika walidacji i wysyłania danych
    console.log('Formularz dodawania łowiska wysłany (placeholder)');
    alert('Łowisko dodane (symulacja)!');
    // Przekierowanie po sukcesie (np. do /fisheries)
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <Link href="/fisheries">
          <Button variant="outline" size="sm">
            <ArrowLeft className="mr-2 h-4 w-4" /> Wróć do Listy Łowisk
          </Button>
        </Link>
        <h1 className={`text-xl sm:text-2xl font-bold ${cardTextColorClass}`}>Dodaj Nowe Łowisko</h1>
        <div>{/* Pusty div dla wyrównania */}</div>
      </div>

      <form
        onSubmit={handleSubmit}
        className={`p-4 sm:p-6 rounded-lg border border-border shadow ${cardBodyBgClass} space-y-6`}
      >
        {/* --- Sekcja Nazwa Łowiska --- */}
        <div>
          <Label htmlFor="fishery-name" className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}>
            <MapPin className="mr-2 h-5 w-5" /> Nazwa Łowiska (Wymagane)
          </Label>
          <Input
            id="fishery-name"
            name="fishery-name"
            type="text"
            placeholder="Np. Jezioro Słoneczne, Rzeka Wędkarska"
            className="bg-card border-border"
            required
          />
        </div>

        {/* --- Sekcja Lokalizacja --- */}
        <div>
          <Label htmlFor="location" className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-1`}>
            <MapPin className="mr-2 h-5 w-5 opacity-70" /> Lokalizacja (Wymagane)
          </Label>
          <Input
            id="location"
            name="location"
            type="text"
            placeholder="Np. Miastowo, ul. Nadbrzeżna lub opis dojazdu"
            className="bg-card border-border"
            required
          />
        </div>

        {/* --- Sekcja Występujące Gatunki --- */}
        <div>
          <Label className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-2`}>
            <ListChecks className="mr-2 h-5 w-5" /> Występujące Gatunki (Opcjonalne)
          </Label>
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-x-4 gap-y-2 max-h-60 overflow-y-auto p-2 border border-border rounded-md bg-card">
            {availableFishSpecies.map((species) => (
              <div key={species.id} className="flex items-center space-x-2">
                <Checkbox
                  id={`species-${species.id}`}
                  checked={selectedSpeciesIds.includes(species.id)}
                  onCheckedChange={() => handleSpeciesChange(species.id)}
                  className="border-border data-[state=checked]:border-primary"
                />
                <Label htmlFor={`species-${species.id}`} className="text-sm font-normal cursor-pointer">
                  {species.name}
                </Label>
              </div>
            ))}
          </div>
          <p className={`text-xs mt-1 ${cardMutedTextColorClass}`}>Zaznacz gatunki występujące na tym łowisku.</p>
        </div>

        {/* --- Sekcja Zdjęcia Łowiska (Opcjonalne) --- */}
        <div>
          <Label htmlFor="fishery-photo" className={`text-sm font-medium ${cardTextColorClass} flex items-center mb-2`}>
            <ImagePlus className="mr-2 h-5 w-5" /> Zdjęcie Łowiska (Opcjonalne)
          </Label>
          <div className="mt-1 flex justify-center rounded-md border-2 border-dashed border-border px-6 pt-5 pb-6 hover:border-primary transition-colors">
            <div className="space-y-1 text-center">
              {selectedImagePreview ? (
                <img
                  src={selectedImagePreview}
                  alt="Podgląd zdjęcia łowiska"
                  className="mx-auto h-32 w-auto rounded-md object-contain"
                />
              ) : (
                <ImagePlus className={`mx-auto h-12 w-12 ${cardMutedTextColorClass}`} />
              )}
              <div className="flex text-sm text-muted-foreground">
                <label
                  htmlFor="fishery-photo-input"
                  className="relative cursor-pointer rounded-md bg-card font-medium text-primary focus-within:outline-none focus-within:ring-2 focus-within:ring-primary focus-within:ring-offset-2 focus-within:ring-offset-card hover:text-primary/80"
                >
                  <span>Załaduj plik</span>
                  <input
                    id="fishery-photo-input"
                    name="fishery-photo"
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

        {/* --- Przycisk Zapisu --- */}
        <div className="pt-2">
          <Button type="submit" className="w-full bg-primary text-primary-foreground hover:bg-primary/90">
            Dodaj Łowisko
          </Button>
        </div>
      </form>
    </div>
  );
}
