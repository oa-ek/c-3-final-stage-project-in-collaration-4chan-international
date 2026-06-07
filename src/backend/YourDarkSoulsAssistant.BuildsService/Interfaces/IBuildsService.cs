using YourDarkSoulsAssistant.BuildsService.DTOs.Builds;

namespace YourDarkSoulsAssistant.BuildsService.Interfaces;

public interface IBuildsService
{
    Task<IEnumerable<BuildResponseDto>> GetAllBuildsAsync(string? userId = null);
    Task<BuildResponseDto> CreateBuildAsync(CreateBuildRequestDto request);
    Task<BuildResponseDto?> UpdateBuildAsync(Guid buildId, UpdateBuildRequestDto request);
}
