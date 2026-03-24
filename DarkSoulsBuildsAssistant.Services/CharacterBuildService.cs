using DarkSoulsBuildsAssistant.Core.DTOs.Character;
using DarkSoulsBuildsAssistant.Core.Interfaces.Repositories;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;

namespace DarkSoulsBuildsAssistant.Services;

public class CharacterBuildService(IUnitOfWork unitOfWork) : ICharacterBuildService
{
    // Тепер ми працюємо з UnitOfWork, а не з конкретним репозиторієм

    public async Task<IEnumerable<CharacterBuildDTO>> GetAllBuildsAsync()
    {
        // Отримуємо всі білди через UnitOfWork
        return await unitOfWork.CharacterBuilds.GetAllAsync();
    }
    
    public async Task<CharacterBuildDTO?> GetBuildByIdAsync(int id)
    {
        return await unitOfWork.CharacterBuilds.GetByIdAsync(id);
    }

    public async Task SaveBuildAsync(CharacterBuildDTO buildDto)
    {
        if (buildDto.Id == 0)
        {
            // Якщо ID = 0, значить це новий білд
            await unitOfWork.CharacterBuilds.AddAsync(buildDto);
        }
        else
        {
            // Якщо ID є, значить ми його оновлюємо
            await unitOfWork.CharacterBuilds.UpdateAsync(buildDto);
        }
        
        // Обов'язково викликаємо CompleteAsync, щоб зберегти зміни в БД!
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteBuildAsync(int id)
    {
        // Оскільки наш GenericRepository приймає DTO для видалення,
        // спочатку знайдемо цей запис
        var buildDto = await unitOfWork.CharacterBuilds.GetByIdAsync(id);
        
        if (buildDto != null)
        {
            await unitOfWork.CharacterBuilds.RemoveAsync(buildDto);
            await unitOfWork.CompleteAsync(); // Зберігаємо видалення
        }
    }
}
