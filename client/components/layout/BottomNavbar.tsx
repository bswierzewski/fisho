'use client';

import Link from 'next/link';
import { usePathname } from 'next/navigation';

import { navLinks } from '@/lib/config';

export default function BottomNavbar() {
  const pathname = usePathname();

  return (
    <>
      <nav className="container fixed inset-x-0 bottom-0 z-50 mx-auto mb-2">
        <div className="mx-2 flex h-[42px] items-center justify-around rounded-full bg-card shadow-xl">
          {navLinks.map((link) => {
            const IconComponent = link.icon;
            const isActive = pathname !== link.href;

            return (
              <Link key={link.href} href={link.href} title={link.title}>
                <IconComponent className={`h-6 w-6 ${isActive && 'text-gray-400'}`} aria-hidden="true" />
              </Link>
            );
          })}
        </div>
      </nav>
    </>
  );
}
