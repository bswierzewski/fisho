// app/(main)/fisheries/add/page.tsx
export default function AddFisheryPage() {
  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Dodaj Nowe Łowisko</h1>
      <div className="bg-card p-6 rounded border">
        <p className="text-muted-foreground">Formularz dodawania łowiska pojawi się tutaj (React Hook Form).</p>
        {/* Placeholder dla formularza */}
        <button className="mt-4 bg-accent text-accent-foreground px-4 py-2 rounded hover:opacity-90">
          Dodaj Łowisko (Placeholder)
        </button>
      </div>
    </div>
  );
}
