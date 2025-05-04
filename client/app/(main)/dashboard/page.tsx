'use client';

import Link from 'next/link';

export default function DashboardPage() {
  return (
    <div className="space-y-6">
      {/* Sekcja Nadchodzące Zawody (placeholder) */}
      <section>
        <h2 className="text-xl font-semibold mb-3">Nadchodzące Zawody</h2>
        <div className="p-4 border rounded bg-card">
          <p className="text-muted-foreground">Lista Twoich nadchodzących zawodów... (placeholder)</p>
          <Link href="/my-competitions" className="text-sm text-primary hover:underline mt-2 block">
            Zobacz wszystkie
          </Link>
        </div>
      </section>

      {/* Sekcja Ostatnie Połowy (placeholder) */}
      <section>
        <h2 className="text-xl font-semibold mb-3">Ostatnie Połowy</h2>
        <div className="p-4 border rounded bg-card">
          <p className="text-muted-foreground">Twoje ostatnie wpisy z dziennika... (placeholder)</p>
          <Link href="/logbook" className="text-sm text-primary hover:underline mt-2 block">
            Zobacz cały dziennik
          </Link>
        </div>
      </section>

      {/* Sekcja Odkryj Zawody (placeholder) */}
      <section>
        <h2 className="text-xl font-semibold mb-3">Odkryj Otwarte Zawody</h2>
        <div className="p-4 border rounded bg-card">
          <p className="text-muted-foreground">Lista otwartych zawodów do dołączenia... (placeholder)</p>
          <Link href="/competitions" className="text-sm text-primary hover:underline mt-2 block">
            Przeglądaj wszystkie
          </Link>
        </div>
      </section>

      {/* Sekcja Odkryj Zawody (placeholder) */}
      <section>
        <h2 className="text-xl font-semibold mb-3">Lista łowisk</h2>
        <div className="p-4 border rounded bg-card">
          <p className="text-muted-foreground">Lista łowisk... (placeholder)</p>
          <Link href="/fisheries" className="text-sm text-primary hover:underline mt-2 block">
            Przeglądaj wszystkie
          </Link>
        </div>
      </section>
    </div>
  );
}
