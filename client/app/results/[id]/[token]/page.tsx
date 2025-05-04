// app/results/[id]/[token]/page.tsx
export default function PublicResultsPage({ params }: { params: { id: string; token: string } }) {
  // Tutaj normalnie byłaby logika weryfikacji tokena i pobierania danych
  return (
    <div className="container mx-auto px-4 py-10">
      <h1 className="text-3xl font-bold mb-6">Wyniki Zawodów (Publiczne - Placeholder)</h1>
      <p className="text-muted-foreground mb-4">ID Zawodów: {params.id}</p>
      <div className="bg-card p-6 rounded border">
        <p className="text-muted-foreground">Tabela wyników dla zawodów o ID {params.id} pojawi się tutaj.</p>
      </div>
    </div>
  );
}
