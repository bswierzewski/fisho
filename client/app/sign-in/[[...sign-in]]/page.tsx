import { SignIn } from '@clerk/nextjs';

export default function SignInPage() {
  return (
    <div className="flex h-screen w-full items-center justify-center bg-gray-100 align-middle">
      <SignIn />
    </div>
  );
}
