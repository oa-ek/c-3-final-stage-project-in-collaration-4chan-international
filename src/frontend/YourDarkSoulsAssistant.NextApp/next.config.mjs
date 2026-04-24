/** @type {import('next').NextConfig} */
const nextConfig = {
  typescript: {
    ignoreBuildErrors: true,
  },
  images: {
    unoptimized: true,
  },
  async rewrites() {
    return [
      {
        source: '/api/user/:path*',
        destination: 'http://api-gateway:8080/api/user/:path*'
      },
      {
        source: '/api/content/:path*',
        destination: 'http://api-gateway:8080/api/content/:path*'
      }
    ];
  },
}

export default nextConfig
