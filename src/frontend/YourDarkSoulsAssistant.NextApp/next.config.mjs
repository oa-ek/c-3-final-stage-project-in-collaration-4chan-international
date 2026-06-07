/** @type {import('next').NextConfig} */
const nextConfig = {
  typescript: {
    ignoreBuildErrors: true,
  },
  images: {
    unoptimized: true,
  },
  async rewrites() {
    const API_URL = process.env.API_GATEWAY_URL || 'http://127.0.0.1:5281';

    return [
      {
        source: '/api/users/:path*',
        destination: `${API_URL}/api/users/:path*`,
      },
      {
        source: '/api/content/:path*',
        destination: `${API_URL}/api/content/:path*`,
      },
      {
        source: '/api/catalog/:path*',
        destination: `${API_URL}/api/catalog/:path*`,
      },
      {
        source: '/api/articles/:path*',
        destination: `${API_URL}/api/articles/:path*`,
      },
    ];
  },
}

export default nextConfig
