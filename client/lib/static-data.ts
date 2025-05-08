// lib/static-data.ts
import { Competition, Fishery, LogbookEntry } from './definitions';

// --- Dane Zawodów ---
export const staticCompetitions: Competition[] = [
  {
    id: 'comp-1',
    name: 'Puchar Wiosny 2025',
    startTime: new Date('2025-05-25T07:00:00'),
    location: 'Jezioro Duże, Wędkowo',
    status: 'upcoming',
    type: 'open',
    // Zdjęcie wędkarza o wschodzie słońca
    imageUrl: 'https://cdn.pixabay.com/photo/2020/07/05/09/08/fishing-5372266_1280.jpg'
  },
  {
    id: 'comp-2',
    name: 'Nocny Maraton Karpiowy',
    startTime: new Date('2025-06-14T20:00:00'),
    location: 'Rzeka Tajemnicza, Odc. 3',
    status: 'upcoming',
    type: 'closed',
    // Zdjęcie wędkarza w nocy/wieczorem
    imageUrl: 'https://cdn.pixabay.com/photo/2021/12/08/16/29/sea-6856232_1280.jpg'
  },
  {
    id: 'comp-3',
    name: 'Zawody Spławikowe Juniorów',
    startTime: new Date('2025-07-12T09:00:00'),
    location: 'Staw Parkowy',
    status: 'upcoming',
    type: 'open',
    // Zdjęcie dzieci/młodzieży wędkującej
    imageUrl: 'https://cdn.pixabay.com/photo/2018/12/10/22/57/child-fishing-3867994_1280.jpg'
  },
  {
    id: 'comp-4',
    name: 'Puchar Lata (Zakończone)',
    startTime: new Date('2024-08-15T06:00:00'),
    location: 'Jezioro Glinianki',
    status: 'finished',
    type: 'open',
    // Zdjęcie ogólne wędkarskie
    imageUrl: 'https://cdn.pixabay.com/photo/2019/03/13/00/09/carp-4052148_1280.jpg'
  }
];

// --- Dane Dziennika Połowów ---
export const staticLogbookEntries: LogbookEntry[] = [
  {
    id: 'log-1',
    species: 'Szczupak',
    catchTime: new Date('2025-05-01T16:45:00'),
    // Zdjęcie szczupaka
    photoUrl: 'https://cdn.pixabay.com/photo/2014/10/05/18/56/wels-475656_1280.jpg',
    lengthCm: 72.5,
    weightKg: 3.1,
    fisheryId: 'fish-2' // Powiązane z Rzeką Szybką
  },
  {
    id: 'log-2',
    species: 'Okoń',
    catchTime: new Date('2025-04-28T09:10:00'),
    // Zdjęcie okonia
    photoUrl: 'https://cdn.pixabay.com/photo/2017/10/05/18/01/pike-2820330_1280.jpg',
    lengthCm: 31,
    weightKg: null,
    fisheryId: 'fish-1' // Powiązane z Jeziorem Miejskim
  },
  {
    id: 'log-3',
    species: 'Leszcz',
    catchTime: new Date('2025-05-05T07:20:00'),
    // Zdjęcie leszcza (lub podobnej ryby)
    photoUrl: 'https://cdn.pixabay.com/photo/2016/01/01/13/37/nature-1116553_1280.jpg',
    lengthCm: null,
    weightKg: 1.8,
    fisheryId: 'fish-3' // Powiązane ze Stawem Rekord
  }
];

// --- Dane Łowisk ---
export const staticFisheries: Fishery[] = [
  {
    id: 'fish-1',
    name: 'Jezioro Miejskie',
    location: 'Miastowo, ul. Parkowa',
    // Zdjęcie jeziora
    imageUrl: 'https://cdn.pixabay.com/photo/2020/06/24/20/41/truebsee-5337646_1280.jpg'
  },
  {
    id: 'fish-2',
    name: 'Rzeka Szybka - Odcinek PZW',
    location: 'Wioskowo, Most Północny',
    // Zdjęcie rzeki
    imageUrl: 'https://cdn.pixabay.com/photo/2023/09/27/12/15/river-8279466_1280.jpg'
  },
  {
    id: 'fish-3',
    name: 'Staw Komercyjny "Rekord"',
    location: 'Rekordowo 15',
    // Zdjęcie stawu/łowiska komercyjnego
    imageUrl: 'https://cdn.pixabay.com/photo/2022/01/16/22/10/pond-6943316_1280.jpg'
  },
  {
    id: 'fish-4',
    name: 'Kanał Żeglugowy',
    location: 'Portowo',
    // Zdjęcie kanału
    imageUrl: 'https://cdn.pixabay.com/photo/2018/10/06/06/56/river-3727391_1280.jpg'
  }
];
