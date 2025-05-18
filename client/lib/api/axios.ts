import Axios, { AxiosError, AxiosRequestConfig, AxiosResponse, InternalAxiosRequestConfig } from 'axios';

// Dla Next.js, importujemy useAuth z @clerk/nextjs, ale pamiętaj, że nie można go użyć bezpośrednio tutaj.
// import { useAuth } from "@clerk/nextjs";
// Zamiast tego, będziemy polegać na globalnej instancji Clerk lub innym mechanizmie.

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:7000'; // Ustaw na bazowy URL API

export const AXIOS_INSTANCE = Axios.create({ baseURL: API_URL });

export const customInstance = <T>(
  config: AxiosRequestConfig,
  options?: AxiosRequestConfig
): Promise<AxiosResponse<T>> => {
  const source = Axios.CancelToken.source();
  const promise = AXIOS_INSTANCE({
    ...config,
    ...options,
    cancelToken: source.token
  }).then(({ data }) => data);

  // @ts-ignore
  promise.cancel = () => {
    source.cancel('Query was cancelled by React Query');
  };

  return promise;
};

// --- Interceptory Axios ---

const requestInterceptor = async (config: InternalAxiosRequestConfig): Promise<InternalAxiosRequestConfig> => {
  // W Next.js z @clerk/nextjs, dostęp do tokenu po stronie klienta
  // jest zazwyczaj przez hook `useAuth().getToken()`.
  // Ponieważ jesteśmy poza komponentem React, musimy użyć innego podejścia.

  // Podejście 1: Sprawdź, czy Clerk automatycznie dołącza tokeny.
  // Jeśli Twoje API (API_URL) jest skonfigurowane jako chroniony zasób w Clerk
  // i używasz <ClerkProvider>, tokeny mogą być dołączane automatycznie.
  // W takim przypadku ten interceptor może nie być potrzebny do dodawania tokenu.

  // Podejście 2: Użycie globalnej instancji Clerk (jeśli dostępna)
  // @clerk/nextjs używa @clerk/clerk-js pod spodem.
  if (typeof window !== 'undefined' && (window as any).Clerk) {
    const clerkInstance = (window as any).Clerk;
    if (clerkInstance.session) {
      try {
        // `getToken` może przyjmować opcje, np. szablon, jeśli masz wiele backendów
        const token = await clerkInstance.session.getToken();
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
          console.log('Token added to request by Axios interceptor (via window.Clerk)');
        } else {
          console.warn('Clerk session active, but no token returned by getToken().');
        }
      } catch (error) {
        console.error('Error getting Clerk token in Axios interceptor:', error);
        // Rozważ, czy żądanie powinno być kontynuowane bez tokenu, czy przerwane
      }
    } else {
      console.warn('Clerk instance found, but no active session. Token not added.');
    }
  } else {
    console.warn('Clerk instance not found on window object. Token cannot be added by Axios interceptor in this way.');
  }
  return config;
};

const requestErrorInterceptor = (error: AxiosError): Promise<AxiosError> => {
  console.error('Axios Request Setup Error:', error.message, error.config);
  return Promise.reject(error);
};

const responseInterceptor = (response: AxiosResponse): AxiosResponse => {
  return response;
};

const responseErrorInterceptor = async (error: AxiosError): Promise<AxiosError> => {
  const { response, request, message, config } = error;

  if (response) {
    // Serwer odpowiedział statusem błędu
    const statusCode = response.status;
    const problemDetails = response.data as any; // Zakładamy, że API zwraca ProblemDetails

    console.error(
      `API Error ${statusCode} for ${config?.method?.toUpperCase()} ${config?.url}:`,
      problemDetails || response.data // Loguj problemDetails jeśli dostępne
    );

    // Twój CustomExceptionHandler na backendzie zwraca ProblemDetails lub ValidationProblemDetails.
    // Możemy to wykorzystać na frontendzie.
    if (statusCode === 400 && problemDetails?.errors) {
      // To jest ValidationProblemDetails z Twojego API
      console.warn('Validation errors:', problemDetails.errors);
      // Możesz przekazać te błędy do formularza lub globalnego stanu błędów.
      // React Query's onError callback otrzyma ten `error` obiekt.
    } else if (statusCode === 401) {
      console.warn('Unauthorized (401). Token may be invalid or session expired.');
      // Clerk zazwyczaj obsługuje to przez przekierowanie do logowania,
      // ale możesz dodać dodatkową logikę, np. wylogowanie z aplikacji.
      // if (typeof window !== "undefined" && (window as any).Clerk) {
      //   (window as any).Clerk.signOut(() => { window.location.href = '/sign-in'; });
      // }
    } else if (statusCode === 403) {
      console.warn('Forbidden (403). User lacks permission.');
      // Pokaż użytkownikowi odpowiedni komunikat.
    } else if (statusCode === 404) {
      console.warn('Resource not found (404).', problemDetails?.detail);
    } else if (statusCode >= 500) {
      console.error('Server error (5xx).', problemDetails?.detail);
      // Pokaż ogólny komunikat o błędzie serwera.
    }
    // Możesz dodać niestandardowe pola do obiektu błędu, jeśli chcesz przekazać więcej informacji
    // error.customData = problemDetails;
  } else if (request) {
    console.error('Network error or no response from server:', message, config?.url);
  } else {
    console.error('Axios setup error:', message);
  }

  return Promise.reject(error);
};

AXIOS_INSTANCE.interceptors.request.use(requestInterceptor, requestErrorInterceptor);
AXIOS_INSTANCE.interceptors.response.use(responseInterceptor, responseErrorInterceptor);

export interface ErrorType<ErrorData = any> extends AxiosError<ErrorData> {
  // Możesz rozszerzyć typ błędu, jeśli Twoje API zwraca specyficzny format
  // np. jeśli CustomExceptionHandler zawsze zwraca ProblemDetails
  response?: AxiosResponse<
    {
      type?: string;
      title?: string;
      status?: number;
      detail?: string;
      errors?: Record<string, string[]>; // Dla ValidationProblemDetails
      // inne pola z ProblemDetails
    } & ErrorData
  >;
}

export default customInstance;
