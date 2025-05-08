'use client';

import { ClerkLoaded, ClerkLoading } from '@clerk/clerk-react';
import { UserButton } from '@clerk/nextjs';
import Image from 'next/image';
import { usePathname } from 'next/navigation';

import { navLinks } from '@/lib/config';

export default function Navbar() {
  const pathname = usePathname();

  const getCurrentPageTitle = (): string => {
    // Sort links from the longest href to the shortest.
    // This ensures that more specific paths (e.g., "/competitions/create")
    // are checked before more general ones (e.g., "/competitions").
    const sortedLinks = [...navLinks].sort((a, b) => b.href.length - a.href.length);

    for (const link of sortedLinks) {
      if (pathname.startsWith(link.href)) {
        return link.title;
      }
    }

    return 'Fishio'; // Default title if no match is found
  };

  const pageTitle = getCurrentPageTitle();

  return (
    <nav className="px-5 p-2 z-50 shadow bg-secondary rounded-b-md">
      <div className="container mx-auto flex justify-between items-center">
        <div className="h-7 w-7">
          <Image src={'/logo.svg'} alt="logo" width={40} height={40} className="" />
        </div>

        <span className="font-bold uppercase text-background tracking-widest">{pageTitle}</span>

        <ClerkLoading>
          <div className="h-7 w-7 animate-pulse rounded-full bg-secondary" />
        </ClerkLoading>
        <ClerkLoaded>
          <UserButton />
        </ClerkLoaded>
      </div>
    </nav>
  );
}
