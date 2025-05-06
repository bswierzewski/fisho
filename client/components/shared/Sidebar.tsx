'use client';

import { useSidebarStore } from '@/lib/store/sidebarStore';

import { Sheet, SheetContent, SheetHeader, SheetTitle } from '@/components/ui/sheet';

export function Sidebar() {
  const { isOpen, setOpen } = useSidebarStore();

  return (
    <Sheet open={isOpen} onOpenChange={setOpen}>
      <SheetContent side="left" className="w-[250px] sm:w-[300px] p-0">
        <SheetHeader>
          <SheetTitle>Menu</SheetTitle>
        </SheetHeader>
      </SheetContent>
    </Sheet>
  );
}
