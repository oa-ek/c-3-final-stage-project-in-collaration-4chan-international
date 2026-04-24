// lib/content-utils.ts

const CONTENT_API_URL = "/api/content/ContentItem";

/**
 * Генерує URL для отримання зображення.
 * Також додає параметр для скидання кешу браузера (cache-buster),
 * щоб при завантаженні нового фото воно оновлювалося миттєво.
 */
export function getImageUrl(route?: string | null, cacheBuster?: number): string {
    if (!route) return "/placeholder-user.jpg"; // Стандартна заглушка

    const baseUrl = `${CONTENT_API_URL}/${route}`;
    return cacheBuster ? `${baseUrl}?t=${cacheBuster}` : baseUrl;
}