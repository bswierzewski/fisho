import { BookOpenIcon, HomeIcon, MapPinIcon, TrophyIcon } from '@heroicons/react/24/solid';

export const navLinks = [
  { href: '/dashboard', label: 'Start', icon: HomeIcon, title: 'Start' },
  { href: '/competitions', label: 'Zawody', icon: TrophyIcon, title: 'Zawody' },
  { href: '/logbook', label: 'Dziennik', icon: BookOpenIcon, title: 'Dziennik' },
  { href: '/fisheries', label: 'Łowiska', icon: MapPinIcon, title: 'Łowiska' }
];
