'use client';

import { navLinks } from '@/lib/navigation';
import { useSidebarStore } from '@/lib/store/sidebarStore';
import Link from 'next/link';
import { usePathname } from 'next/navigation';

import { Button } from '@/components/ui/button';
import { Sheet, SheetClose, SheetContent, SheetFooter, SheetHeader, SheetTitle } from '@/components/ui/sheet';

export function Sidebar() {
  const { isOpen, setOpen } = useSidebarStore();
  const pathname = usePathname(); // Do podświetlania aktywnego linku

  return (
    <Sheet open={isOpen} onOpenChange={setOpen}>
      <SheetContent side="left" className="w-[250px] sm:w-[300px] p-0">
        {' '}
        {/* Dostosuj szerokość */}
        <SheetHeader>
          <SheetTitle>🐟 Fisho Menu</SheetTitle>
        </SheetHeader>
        <nav className="flex flex-col">
          {navLinks.map((link) => {
            const isActive = pathname.startsWith(link.href); // Proste sprawdzanie aktywnego linku
            return (
              // Użyj SheetClose, aby zamknąć sidebar po kliknięciu linku
              <SheetClose asChild key={link.href}>
                <Link href={link.href}>
                  <Button>
                    <link.icon className="mr-2 h-4 w-4" />
                    {link.label}
                  </Button>
                </Link>
              </SheetClose>
            );
          })}
        </nav>
      </SheetContent>
    </Sheet>
  );
}
