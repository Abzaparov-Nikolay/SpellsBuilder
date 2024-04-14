using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BlackHoleSpawnerConfigurator : SpellConfigurator
{
    [SerializeField] private ChargeSpawner spawner;
    [SerializeField] private Lifetime chargingTimer;

    protected override void OnModifiersApplied()
    {
        spawner.modifiers = modifiersList;
    }

    protected override void HandleTreeModifier(int count, Modifier buff)
    {
        base.HandleTreeModifier(count, buff);
        chargingTimer.AddTimeBonus(buff.Lifetime * count);
    }

    protected override void HandleHarmonyModifier(int count, Modifier buff)
    {
        base.HandleHarmonyModifier(count, buff);
        chargingTimer.AddTimeBonus(buff.Lifetime * count);
    }
}
