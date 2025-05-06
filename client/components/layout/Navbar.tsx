'use client';

import { useSidebarStore } from '@/lib/store/sidebarStore';
import { UserButton } from '@clerk/nextjs';
import { Menu } from 'lucide-react';
import { usePathname } from 'next/navigation';

import { navLinks } from '@/lib/config';

export default function Navbar() {
  const { setOpen } = useSidebarStore();
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
    <nav className="p-4 bg-card z-50 shadow-md">
      <div className="container mx-auto flex justify-between items-center">
        <Menu onClick={() => setOpen(true)} />

        <span className="text-lg font-semibold uppercase tracking-widest">{pageTitle}</span>

        <UserButton />
      </div>
    </nav>
  );
}
