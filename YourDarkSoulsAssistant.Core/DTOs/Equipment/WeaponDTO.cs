namespace DarkSoulsBuildsAssistant.Core.DTOs.Equipment;

public class WeaponDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public int EquipmentTypeId { get; set; } // Тип зброї (Мечі, Сокири тощо)

    public string? IconPath { get; set; }
    public decimal? Weight { get; set; }
        
    // --- Специфічні поля для зброї ---
    public decimal? Damage { get; set; }
    public int? ReqStrength { get; set; }
    public int? ReqDexterity { get; set; }
    public int? ReqIntelligence { get; set; }
    public int? ReqFaith { get; set; }

    // --- Характеристики скейлінгів або шкоди (Influences) ---
    public double? Physical { get; set; }
    public double? Strike { get; set; }
    public double? Slash { get; set; }
    public double? Pierce { get; set; }
    public double? Magic { get; set; }
    public double? Fire { get; set; }
    public double? Lightning { get; set; }
    public double? Holy { get; set; }
}