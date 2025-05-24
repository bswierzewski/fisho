import './globals.css';
import TanstackQueryProvider from '@/providers/TanstackQueryProvider';
import { plPL } from '@clerk/localizations';
import { ClerkProvider } from '@clerk/nextjs';
import type { Metadata } from 'next';
import { Toaster } from 'react-hot-toast';

import { AxiosClientConfigurator } from '@/components/AxiosClientConfigurator';

export const metadata: Metadata = {
  title: 'Fishio',
  description: 'Aplikacja do organizacji zawodów wędkarskich'
};

export default function RootLayout({
  children
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <ClerkProvider localization={plPL}>
      <html lang="pl">
        <body>
          <AxiosClientConfigurator />
          <TanstackQueryProvider>
            {children}
            <Toaster />
          </TanstackQueryProvider>
        </body>
      </html>
    </ClerkProvider>
  );
}
