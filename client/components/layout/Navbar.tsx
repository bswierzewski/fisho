'use client';

import { UserButton } from '@clerk/nextjs';

export default function Navbar() {
  return (
    <nav className="p-4 z-50">
      <div className="container mx-auto flex justify-between items-center">
        <span>Strona główna</span>

        <div className="flex items-center">
          <UserButton />
        </div>
      </div>
    </nav>
  );
}
