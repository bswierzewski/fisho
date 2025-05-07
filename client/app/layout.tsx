import './globals.css';
import { plPL } from '@clerk/localizations';
import { ClerkProvider } from '@clerk/nextjs';
import type { Metadata } from 'next';

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
        <body>{children}</body>
      </html>
    </ClerkProvider>
  );
}
