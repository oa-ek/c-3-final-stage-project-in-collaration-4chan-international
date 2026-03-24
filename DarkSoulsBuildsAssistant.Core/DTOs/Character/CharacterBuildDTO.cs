namespace DarkSoulsBuildsAssistant.Core.DTOs;

public record CharacterBuildDTO
{
    // --- ВАГА ТА ПЕРЕКАТ (Вже було) ---
    public decimal TotalWeight { get; set; }
    public decimal MaxEquipLoad { get; set; }
    public decimal LoadPercentage { get; set; }
    public string RollType { get; set; } = string.Empty;

    // --- ЖИТТЄВІ ПОКАЗНИКИ (Вже було) ---
    public int HP { get; set; }
    public int Stamina { get; set; }
    public int FocusPoints { get; set; }

    // --- НОВЕ: АТАКА (AR - Attack Rating) ---
    public int AttackPhysical { get; set; }
    public int AttackMagic { get; set; }
    public int AttackFire { get; set; }
    public int AttackLightning { get; set; }
    public int AttackCritical { get; set; }
    public int RWeaponCritical { get; set; }


    // --- НОВЕ: ЗАХИСТ (Defense / Absorption) ---
    public double DefensePhysical { get; set; }
    public double DefenseMagic { get; set; }
    public double DefenseFire { get; set; }
    public double DefenseLightning { get; set; }

    // --- НОВЕ: СТІЙКІСТЬ (Poise) ---
    public decimal Poise { get; set; }
}

