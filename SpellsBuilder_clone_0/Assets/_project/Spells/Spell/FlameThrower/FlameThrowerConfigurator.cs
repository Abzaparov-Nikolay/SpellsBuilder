using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerConfigurator : SpellConfigurator
{
    [SerializeField] private DamageDealer dealer;
    [SerializeField] private StatusApplier statusApplier;

    protected override void HandleFireModifier(int count, Modifier buff)
    {
        base.HandleFireModifier(count, buff);
        dealer.AddDamageBonus(buff.Damage * count);
        statusApplier.AddPowerBonus(buff.StatusPower * count);
    }
}
