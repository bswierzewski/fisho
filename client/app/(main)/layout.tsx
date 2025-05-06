import Bottombar from '@/components/layout/BottomNavbar';
import Navbar from '@/components/layout/Navbar';
import { Sidebar } from '@/components/shared/Sidebar';

export default function MainLayout({ children }: { children: React.ReactNode }) {
  return (
    <div className="flex flex-col min-h-screen">
      <Sidebar />
      <Navbar />
      <main className="container mx-auto px-4 pt-4 pb-20 flex-grow">{children}</main>
      <BottomNavbar />
    </div>
  );
}
