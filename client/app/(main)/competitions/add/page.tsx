// app/(main)/competitions/add/page.tsx
export default function AddCompetitionPage() {
  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Stwórz Nowe Zawody</h1>
      <div className="bg-card p-6 rounded border">
        <p className="text-muted-foreground">Formularz dodawania zawodów pojawi się tutaj (React Hook Form).</p>
        {/* Placeholder dla formularza */}
        <button className="mt-4 bg-accent text-accent-foreground px-4 py-2 rounded hover:opacity-90">
          Stwórz Zawody (Placeholder)
        </button>
      </div>
    </div>
  );
}
