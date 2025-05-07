'use client';

import { BellAlertIcon } from '@heroicons/react/24/outline';
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

    return 'Fisho'; // Default title if no match is found
  };

  const pageTitle = getCurrentPageTitle();

  return (
    <nav className="p-2 bg-card z-50 shadow-md ">
      <div className="container mx-auto flex justify-between items-center">
        <Image src="/logo_shark.svg" alt="logo" width={32} height={32} />
        <span className="text-lg font-semibold uppercase">{pageTitle}</span>

        <BellAlertIcon className="h-7" />
      </div>
    </nav>
  );
}
