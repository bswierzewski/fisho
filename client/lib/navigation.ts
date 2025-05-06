import { BookOpen, Home, MapPin, Trophy } from 'lucide-react';

export const navLinks = [
  { href: '/dashboard', label: 'Start', icon: Home, title: 'Panel Główny' },
  { href: '/competitions', label: 'Zawody', icon: Trophy, title: 'Zawody' },
  { href: '/logbook', label: 'Dziennik', icon: BookOpen, title: 'Dziennik Połowów' },
  { href: '/fisheries', label: 'Łowiska', icon: MapPin, title: 'Łowiska' }
];
