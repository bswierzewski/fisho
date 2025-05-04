import { clerkMiddleware, createRouteMatcher } from '@clerk/nextjs/server';
import { NextResponse } from 'next/server';

const isPublicRoute = createRouteMatcher([
  '/', // Strona główna (Landing Page) JEST TERAZ PUBLICZNA
  '/sign-in(.*)', // Strony logowania Clerk
  '/sign-up(.*)', // Strony rejestracji Clerk
  '/results/(.*)', // Publiczne wyniki zawodów
  '/api/webhooks/clerk' // Webhooki Clerk (jeśli używasz)
]);

const isSignInOrSignUpRoute = createRouteMatcher(['/sign-in(.*)', '/sign-up(.*)']);

export default clerkMiddleware(async (auth, req) => {
  const { userId } = await auth();
  const url = req.nextUrl;

  // Użytkownik jest zalogowany
  if (userId) {
    // Jeśli zalogowany użytkownik próbuje wejść na stronę logowania/rejestracji,
    // przekieruj go na dashboard '/dashboard'
    if (isSignInOrSignUpRoute(req)) {
      const dashboardUrl = new URL('/dashboard', req.url);
      return NextResponse.redirect(dashboardUrl);
    }
    // Jeśli zalogowany użytkownik próbuje wejść na stronę główną '/',
    // przekieruj go na dashboard '/dashboard'
    if (url.pathname === '/') {
      const dashboardUrl = new URL('/dashboard', req.url);
      return NextResponse.redirect(dashboardUrl);
    }
    // Pozwól zalogowanemu użytkownikowi kontynuować na inne strony (w tym /dashboard)
    return NextResponse.next();
  }

  // Użytkownik NIE jest zalogowany
  if (!userId) {
    // Jeśli niezalogowany użytkownik próbuje wejść na stronę CHRONIONĄ
    // (czyli nie jest to strona publiczna z listy isPublicRoute)
    if (!isPublicRoute(req)) {
      // Przekieruj go na stronę logowania '/sign-in'
      const signInUrl = new URL('/sign-in', req.url);
      signInUrl.searchParams.set('redirect_url', url.pathname); // Zapamiętaj, gdzie chciał iść
      console.log(`Redirecting unauthenticated user from ${url.pathname} to /sign-in`);
      return NextResponse.redirect(signInUrl);
    }

    // Jeśli niezalogowany użytkownik wchodzi na stronę PUBLICZNĄ (np. '/', /sign-in),
    // pozwól mu kontynuować.
    return NextResponse.next();
  }

  // Domyślnie, pozwól kontynuować.
  return NextResponse.next();
});

export const config = {
  matcher: ['/((?!.*\\..*|_next).*)', '/', '/(api|trpc)(.*)']
};
