'use client';

import { bottomNavItems } from '@/lib/navigation';
import Link from 'next/link';

export function BottomNavbar() {
  return (
    <nav className="fixed bottom-0 left-0 right-0 w-full bg-card border-t border-border z-50">
      <div className="container mx-auto flex justify-around items-center h-16 px-2">
        {bottomNavItems.map((item) => {
          return (
            <Link href={item.href} key={item.href}>
              <item.icon className="h-5 w-5 mb-1" />
            </Link>
          );
        })}
      </div>
    </nav>
  );
}
