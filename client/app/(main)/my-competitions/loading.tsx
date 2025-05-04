// app/(main)/my-competitions/loading.tsx

export default function LoadingMyCompetitions() {
  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Moje Zawody</h1>
      <p className="text-muted-foreground mb-4">Ładowanie listy Twoich zawodów...</p>
      {/* Szkielet listy */}
      <div className="space-y-4">
        {[1, 2].map((i) => (
          <div key={i} className="p-4 border rounded bg-card animate-pulse">
            <div className="h-6 bg-muted rounded w-1/2 mb-2"></div> {/* Nazwa */}
            <div className="h-4 bg-muted rounded w-3/4"></div> {/* Info */}
          </div>
        ))}
      </div>
    </div>
  );
}
