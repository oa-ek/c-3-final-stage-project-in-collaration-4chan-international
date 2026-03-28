using System.ComponentModel.DataAnnotations;

namespace DarkSoulsBuildsAssistant.Core.DTOs.Equipment;

/// <summary>
/// Об'єкт передачі даних (DTO) для створення нової броні з форми на Razor-сторінці.
/// </summary>
public class ArmorDTO
{
    public int Id { get; set; }
    // --- Основні характеристики броні ---
    public string Name { get; set; } = string.Empty;
    
    public int EquipmentTypeId { get; set; } // Випадаючий список Piece Type (1=Head, 2=Chest і т.д.)

    public string? IconPath { get; set; }
    
    public decimal? Weight { get; set; }
    
    public decimal? Poise { get; set; }

    // --- Характеристики захисту (Influences) ---
    // Ми робимо їх окремими полями для зручного "зв'язування" (model binding) з HTML формою.

    public double? Physical { get; set; }
    public double? Strike { get; set; }
    public double? Slash { get; set; }
    public double? Pierce { get; set; }
    public double? Magic { get; set; }
    public double? Fire { get; set; }
    public double? Lightning { get; set; }
    public double? Holy { get; set; }
}
