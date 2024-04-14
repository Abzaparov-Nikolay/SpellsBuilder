using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeAttackConfigurator : SpellConfigurator
{
    [SerializeField] private DamageDealer dealer;
    [SerializeField] private StatusApplier statusApplier;

    protected override void HandleHarmonyModifier(int count, Modifier buff)
    {
        base.HandleHarmonyModifier(count, buff);
        dealer.AddDamageBonus(count * buff.Damage, buff.Healing);
    }

    protected override void HandleFireModifier(int count, Modifier buff)
    {
        base.HandleHarmonyModifier(count, buff);
        if (buff.UseStatusEffect)
            statusApplier.SetStatusEffect(buff.StatusEffect);
        statusApplier.AddPowerBonus(buff.StatusPower * count);
    }


}
