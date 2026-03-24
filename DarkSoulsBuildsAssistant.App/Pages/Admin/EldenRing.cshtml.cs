using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;

namespace DarkSoulsBuildsAssistant.App.Pages.Admin;

public class EldenRingModel(ICatalogService catalogService) : PageModel
{
    public IEnumerable<EquipmentDTO> Equipments { get; set; } = new List<EquipmentDTO>();
    public string CurrentCategory { get; set; } = "armor";

    // Змінили Task на Task<IActionResult> і прибрали дефолтне значення
    public async Task<IActionResult> OnGetAsync(string category)
    {
        // Якщо параметр порожній (перейшли з бокового меню)
        if (string.IsNullOrEmpty(category))
        {
            // Переспрямовуємо на цю ж сторінку, але з чітким параметром
            return RedirectToPage(new { category = "armor" });
        }

        CurrentCategory = category.ToLower();

        switch (CurrentCategory)
        {
            case "weapons":
                Equipments = await catalogService.GetAllWeaponsAsync();
                break;
            case "armor":
                Equipments = await catalogService.GetAllArmorAsync();
                break;
            case "talismans":
            case "spells":
                Equipments = new List<EquipmentDTO>();
                break;
            default:
                // Якщо користувач ввів якусь нісенітницю в URL (наприклад ?category=qwerty)
                return RedirectToPage(new { category = "armor" });
        }

        Equipments = Equipments.OrderBy(e => e.Name).ToList();

        // Повертаємо відрендерену сторінку, якщо все добре
        return Page(); 
    }
}
