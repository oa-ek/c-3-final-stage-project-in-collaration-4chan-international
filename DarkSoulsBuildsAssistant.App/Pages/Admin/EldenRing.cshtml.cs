using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;

namespace DarkSoulsBuildsAssistant.App.Pages.Admin;

public class EldenRingModel(ICatalogService catalogService) : PageModel
{
    // Створюємо властивість, до якої будемо звертатися з HTML
    public List<EquipmentDTO> Equipments { get; set; } = new();

    // Інжектуємо наш сервіс

    // Змінюємо OnGet на асинхронний OnGetAsync
    public async Task OnGetAsync()
    {
// 1. Завантажуємо зброю
        var weapons = await catalogService.GetAllWeaponsAsync();
        
        // 2. Завантажуємо броню
        var armors = await catalogService.GetAllArmorAsync();

        // 3. Об'єднуємо їх в один список і сортуємо за назвою за алфавітом
        Equipments = weapons.Concat(armors)
            .OrderBy(e => e.Name)
            .ToList();
    }
}