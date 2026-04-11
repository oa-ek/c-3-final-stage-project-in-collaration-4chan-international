using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DarkSoulsBuildsAssistant.Core.DTOs.Equipment;
using DarkSoulsBuildsAssistant.Core.Interfaces.Services.Business;
using YourDarkSoulsAssistant.Services;

namespace YourDarkSoulsAssistant.App.Pages.Admin;

public class EldenRingModel(ICatalogService catalogService) : PageModel
{
    public IEnumerable<EquipmentDTO> Equipments { get; set; } = new List<EquipmentDTO>();
    public string CurrentCategory { get; set; } = "armor";
    
    [BindProperty]
    public ArmorDTO ArmorInput { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(string category)
    {
        await LoadDataAsync(category);
        return Page();
    }

    // 3. НОВИЙ ОБРОБНИК спеціально для форми броні
    public async Task<IActionResult> OnPostSaveArmorAsync(string category)
    {
        if (!ModelState.IsValid) { await LoadDataAsync(category); return Page(); }

        if (ArmorInput.Id == 0)
        {
            await catalogService.AddArmorAsync(ArmorInput); // Створення
        }
        else
        {
            // Виклик методу оновлення (через сервіс -> UnitOfWork -> Repository)
            await catalogService.UpdateArmorAsync(ArmorInput); // Оновлення
        }

        return RedirectToPage(new { category = "armor" });
    }
    
    [BindProperty]
    public WeaponDTO WeaponInput { get; set; } = new();

    public async Task<IActionResult> OnPostSaveWeaponAsync(string category)
    {
        if (!ModelState.IsValid) { await LoadDataAsync(category); return Page(); }

        if (WeaponInput.Id == 0)
        {
            await catalogService.AddWeaponAsync(WeaponInput);
        }
        else
        {
            await catalogService.UpdateWeaponAsync(WeaponInput);
        }

        return RedirectToPage(new { category = "weapons" });
    }

// --- ВИДАЛЕННЯ ---
    public async Task<IActionResult> OnPostDeleteAsync(int id, string category)
    {
        // У EquipmentRepository вже є DeleteByIdAsync
        await catalogService.DeleteEquipmentAsync(id); // Твій сервіс має викликати _unitOfWork.EquipmentRepository.DeleteByIdAsync(id) і SaveChanges
        return RedirectToPage(new { category = category });
    }

    // --- Допоміжний метод для завантаження даних ---
    private async Task LoadDataAsync(string category)
    {
        if (string.IsNullOrEmpty(category)) category = "armor";
        CurrentCategory = category.ToLower();

        switch (CurrentCategory)
        {
            case "weapons": Equipments = await catalogService.GetAllWeaponsAsync(); break;
            case "armor": Equipments = await catalogService.GetAllArmorAsync(); break;
            default:
                CurrentCategory = "armor";
                Equipments = await catalogService.GetAllArmorAsync(); 
                break;
        }

        Equipments = Equipments.OrderBy(e => e.Name).ToList();
    }
}