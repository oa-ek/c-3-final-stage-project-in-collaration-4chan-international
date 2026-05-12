namespace YourDarkSoulsAssistant.ArticlesService.DTOs;

public record YouTubeGuideResponseDTO
{
    public required string Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ThumbnailUrl { get; init; } = string.Empty;
    public string VideoUrl { get; init; } = string.Empty;
    public string Duration { get; init; } = string.Empty;
    public string Category { get; init; } = "walkthrough";
    public string Game { get; init; } = "elden-ring";
    public string Author { get; init; } = string.Empty;
    public string? AuthorAvatar { get; init; }
    public ulong Views { get; init; }
    public ulong Likes { get; init; }
    public DateTime PublishedAt { get; init; }
    public List<string> Tags { get; init; } = new();
}
