import { BookOpenIcon, CalendarIcon, HomeIcon, MapPinIcon, TrophyIcon } from '@heroicons/react/24/solid';

export const navLinks = [
  { href: '/dashboard', label: 'Start', icon: HomeIcon, title: 'Start' },
  { href: '/my-competitions', label: 'Moje zawody', icon: CalendarIcon, title: 'Moje zawody' },
  { href: '/competitions', label: 'Zawody', icon: TrophyIcon, title: 'Zawody' },
  { href: '/logbook', label: 'Dziennik', icon: BookOpenIcon, title: 'Dziennik' },
  { href: '/fisheries', label: 'Łowiska', icon: MapPinIcon, title: 'Łowiska' }
];
