import { useEffect, useState } from 'react';

export function useMediaQuery(query: string): boolean {
  const [matches, setMatches] = useState<boolean>(false); // Domyślnie false na serwerze

  useEffect(() => {
    // Sprawdź tylko po stronie klienta
    if (typeof window !== 'undefined') {
      const mediaQueryList = window.matchMedia(query);

      // Funkcja do aktualizacji stanu
      const listener = (event: MediaQueryListEvent) => {
        setMatches(event.matches);
      };

      // Ustaw początkowy stan na kliencie
      setMatches(mediaQueryList.matches);

      // Dodaj listenera dla zmian
      // Używamy nowszego addEventListener zamiast deprecated addListener
      try {
        mediaQueryList.addEventListener('change', listener);
      } catch (e) {
        // Fallback dla starszych przeglądarek
        console.error('useMediaQuery: addListener not supported', e);
      }

      // Funkcja czyszcząca - usuń listenera przy odmontowaniu komponentu
      return () => {
        try {
          mediaQueryList.removeEventListener('change', listener);
        } catch (e) {
          console.error('useMediaQuery: removeListener not supported', e);
        }
      };
    }
  }, [query]); // Efekt uruchamia się ponownie tylko, gdy zmieni się query

  return matches;
}
