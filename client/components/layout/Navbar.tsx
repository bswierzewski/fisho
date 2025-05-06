'use client';

import { useSidebarStore } from '@/lib/store/sidebarStore';
import { UserButton } from '@clerk/nextjs';
import { Menu } from 'lucide-react';

export default function Navbar() {
  const { setOpen } = useSidebarStore();
  return (
    <nav className="p-4 z-50 shadow-md">
      <div className="container mx-auto flex justify-between items-center">
        <Menu onClick={() => setOpen(true)} />

        <span>Strona główna</span>

        <UserButton />
      </div>
    </nav>
  );
}
