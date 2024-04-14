using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBeamConfigurator : SpellConfigurator
{
    [SerializeField] DamageDealer dealer;

    protected override void HandleHarmonyModifier(int count, Modifier buff)
    {
        base.HandleHarmonyModifier(count, buff);
        dealer.AddDamageBonus(buff.Damage * count);
    }
}
