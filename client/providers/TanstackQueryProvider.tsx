'use client';

import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import React, { useState } from 'react';

export default function TanstackQueryProvider({ children }: React.PropsWithChildren) {
  const [queryClient] = useState(
    () =>
      new QueryClient({
        defaultOptions: {
          queries: {
            // Opcjonalnie: ustaw domyślny czas nieaktualności danych (staleTime)
            // na przykład na 5 minut, aby uniknąć zbyt częstych zapytań
            staleTime: 1000 * 60 * 5
          }
        }
      })
  );

  return <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>;
}
