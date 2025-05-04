// app/(main)/logbook/add/page.tsx
export default function AddLogbookEntryPage() {
  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Dodaj Połów do Dziennika</h1>
      <div className="bg-card p-6 rounded border">
        <p className="text-muted-foreground">Formularz dodawania połowu pojawi się tutaj (React Hook Form).</p>
        {/* Placeholder dla formularza */}
        <button className="mt-4 bg-accent text-accent-foreground px-4 py-2 rounded hover:opacity-90">
          Zapisz w Dzienniku (Placeholder)
        </button>
      </div>
    </div>
  );
}
