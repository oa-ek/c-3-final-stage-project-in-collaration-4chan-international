const CONTENT_API_URL = "/api/content/ContentItem";

export function getImageUrl(route?: string | null, cacheBuster?: number): string {
    if (!route) return "/placeholder-user.jpg";

    const baseUrl = `${CONTENT_API_URL}/${route}`;
    return cacheBuster ? `${baseUrl}?t=${cacheBuster}` : baseUrl;
}