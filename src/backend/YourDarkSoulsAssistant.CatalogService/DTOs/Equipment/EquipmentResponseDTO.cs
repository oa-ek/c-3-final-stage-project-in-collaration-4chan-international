namespace YourDarkSoulsAssistant.CatalogService.DTOs.Equipment;

using System;

public class EquipmentResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Назва з EquipmentType
    public string? Category { get; set; } // "weapon" | "armor" | "talisman" тощо
    public string? AttackType { get; set; }
    public string? FpCost { get; set; }
    public string Weight { get; set; } = string.Empty; // Фронт чекає string
    public string? Image { get; set; } // Мапимо з IconPath

    public AttackStats Attack { get; set; } = new();
    public GuardStats Guard { get; set; } = new();
    public ScalingStats Scaling { get; set; } = new();
    public RequiredStats Required { get; set; } = new();
}

public class AttackStats
{
    public string Physical { get; set; } = "0";
    public string Magic { get; set; } = "0";
    public string Fire { get; set; } = "0";
    public string Lightning { get; set; } = "0";
    public string Holy { get; set; } = "0";
    public string Critical { get; set; } = "0";
}

public class GuardStats
{
    public string Physical { get; set; } = "0";
    public string Magic { get; set; } = "0";
    public string Fire { get; set; } = "0";
    public string Lightning { get; set; } = "0";
    public string Holy { get; set; } = "0";
    public string Boost { get; set; } = "0";
}

public class ScalingStats
{
    public string Str { get; set; } = "-";
    public string Dex { get; set; } = "-";
    public string Int { get; set; } = "-";
    public string Fai { get; set; } = "-";
    public string Arc { get; set; } = "-";
}

public class RequiredStats
{
    public string Str { get; set; } = "0";
    public string Dex { get; set; } = "0";
    public string Int { get; set; } = "0";
    public string Fai { get; set; } = "0";
    public string Arc { get; set; } = "0";
}