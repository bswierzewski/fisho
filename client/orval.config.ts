// orval.config.js
module.exports = {
  fishio: {
    input: {
      // Upewnij się, że ten URL jest dostępny, gdy uruchamiasz Orval
      target: 'http://localhost:7000/swagger/v1/swagger.json'
    },
    output: {
      mode: 'tags-split', // Dobre dla organizacji
      client: 'react-query', // Używamy TanStack Query (v4 lub v5)
      target: 'lib/api/endpoints', // Gdzie generować hooki i funkcje endpointów
      schemas: 'lib/api/models', // Gdzie generować typy/schematy
      override: {
        mutator: {
          path: 'lib/api/axios.ts', // Ścieżka do Twojego pliku
          name: 'customInstance' // Nazwa eksportowanej funkcji mutatora
        }
      }
    }
    // Opcjonalnie: Hooki do modyfikacji wygenerowanego kodu, jeśli potrzebne
    // hooks: {
    //   afterAllFilesWrite: 'prettier --write', // Formatowanie po generacji
    // }
  }
};
