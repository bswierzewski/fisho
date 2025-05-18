'use client';

import { useAuth, useClerk } from '@clerk/nextjs';
import { useEffect } from 'react';

import {
  axiosInstance,
  // Zaimportuj instancję Axios
  setupClerkInterceptors // Zaimportuj funkcję konfigurującą
} from '@/lib/api/axios';

export function AxiosClientConfigurator() {
  const { getToken } = useAuth();
  const { signOut } = useClerk();

  useEffect(() => {
    // `getToken` i `signOut` są zazwyczaj stabilnymi referencjami z Clerk,
    // ale dodanie ich do tablicy zależności jest dobrą praktyką.
    // console.log("ApiClientConfigurator: Setting up Axios interceptors...");
    setupClerkInterceptors(axiosInstance, getToken, signOut);

    // Funkcja czyszcząca nie jest ściśle konieczna, jeśli interceptory
    // mają pozostać aktywne przez cały czas życia aplikacji,
    // ale `setupClerkInterceptors` teraz obsługuje usuwanie starych,
    // co pomaga przy HMR.
  }, [getToken, signOut]);

  return null; // Ten komponent nie renderuje niczego w DOM
}
