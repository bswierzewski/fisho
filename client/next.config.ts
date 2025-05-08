import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
  /* config options here */
  devIndicators: false,
  images: {
    remotePatterns: [
      {
        protocol: 'https', // Protokół używany przez Pixabay
        hostname: 'cdn.pixabay.com', // Hostname, z którego chcesz ładować obrazy
        port: '', // Zostaw puste dla standardowych portów (80, 443)
        pathname: '/**' // Pozwól na dowolną ścieżkę na tym hoście (np. /photo/...)
      }
      // Możesz tu dodać inne dozwolone hostnames w przyszłości
      // np. dla Cloudinary, jeśli będziesz go używać
      // {
      //   protocol: 'https',
      //   hostname: 'res.cloudinary.com',
      //   port: '',
      //   pathname: '/TWOJA_NAZWA_CHMURY/**', // Dostosuj do swojej nazwy chmury
      // },
    ]
  }
};

export default nextConfig;
