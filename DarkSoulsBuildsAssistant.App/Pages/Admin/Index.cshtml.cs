using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json; // Потрібно для роботи з JSON

namespace DarkSoulsBuildsAssistant.App.Pages.Admin;

public class IndexModel : PageModel
{
    // Дані для першого графіка (Ролі/Класи)
    public List<string> ClassLabels { get; set; } = new();
    public List<int> ClassStats { get; set; } = new();

    // Дані для другого графіка (Зброя)
    public List<string> WeaponLabels { get; set; } = new();
    public List<int> WeaponStats { get; set; } = new();

    public void OnGet()
    {
        // ТУТ БУДЕ ВАША ЛОГІКА З БАЗИ ДАНИХ
        // Наприклад: ClassStats = _userService.GetClassStatistics();

        // Імітуємо завантаження нових даних (я трохи змінив цифри, щоб ви побачили, що дані динамічні)
        ClassLabels = new List<string> { "Knight", "Sorcerer", "Warrior", "Thief", "Cleric" };
        ClassStats = new List<int> { 300, 150, 180, 45, 90 };

        WeaponLabels = new List<string> { "Claymore", "Moonlight GS", "Uchigatana" };
        WeaponStats = new List<int> { 2500, 800, 1500 };
    }
}
