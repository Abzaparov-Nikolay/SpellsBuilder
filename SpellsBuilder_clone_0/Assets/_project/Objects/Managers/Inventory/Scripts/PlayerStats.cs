using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    private Dictionary<PlayerStat, Multiplier> stats = new() {
        { PlayerStat.Speed, new() },
        { PlayerStat.MaxHealth, new() },
        { PlayerStat.Defence, new() },
        { PlayerStat.Damage, new() },
        { PlayerStat.StatusPower, new() },
        { PlayerStat.SpawnedSpells, new() },
        { PlayerStat.Luck, new() },
        { PlayerStat.SpellSize, new() },
        { PlayerStat.SpellLifetime, new() },
    };

    public float GetValue(PlayerStat stat)
    {
        return stats[stat];
    }

    public void AddBonus(PlayerStat stat, float percentage)
    {
        stats[stat].Add(percentage);
    }

    
}

public enum PlayerStat
{
    Speed,
    MaxHealth,
    Defence,
    Damage,
    StatusPower,
    SpawnedSpells,
    Luck,
    SpellSize,
    SpellLifetime
}
