'use client';

import { UserButton } from '@clerk/nextjs';
import Link from 'next/link';
import { usePathname } from 'next/navigation';

import { navLinks } from '@/lib/config';

export default function BottomNavbar() {
  const pathname = usePathname();

  return (
    <>
      <nav className="container fixed inset-x-0 bottom-0 z-50 mx-auto mb-2 ">
        <div className="mx-2 flex items-center justify-around rounded-full bg-background p-2 shadow-lg">
          {navLinks.map((link) => {
            const IconComponent = link.icon;
            const isActive = pathname === link.href;

            return (
              <Link
                key={link.href}
                href={link.href}
                title={link.title}
                className={`flex h-10 w-10 items-center justify-center rounded-full
                  ${
                    isActive
                      ? 'bg-card-foreground text-card'
                      : 'opacity-50 hover:bg-accent hover:text-accent-foreground'
                  }
                `}
              >
                <IconComponent className={`h-6 w-6`} />
              </Link>
            );
          })}

          <UserButton />
        </div>
      </nav>
    </>
  );
}
