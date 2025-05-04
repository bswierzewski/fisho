export default function LoadingCompetitionsList() {
  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Otwarte Zawody</h1>
      <p className="text-muted-foreground mb-4">Wyszukiwanie dostępnych zawodów...</p>
      {/* Szkielet listy kart */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {[1, 2, 3, 4, 5, 6].map((i) => (
          <div key={i} className="p-4 border rounded bg-card animate-pulse">
            <div className="h-6 bg-muted rounded w-3/4 mb-3"></div> {/* Nazwa */}
            <div className="h-4 bg-muted rounded w-1/2 mb-2"></div> {/* Lokalizacja */}
            <div className="h-4 bg-muted rounded w-5/6 mb-2"></div> {/* Data */}
            <div className="h-4 bg-muted rounded w-1/4 mt-3"></div> {/* Status */}
          </div>
        ))}
      </div>
    </div>
  );
}
