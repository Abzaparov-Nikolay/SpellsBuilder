//using GameKit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRootConfigurator : SpellConfigurator
{
    [SerializeField] private Transform scalableTransform;
    [SerializeField] private DamageDealer dealer;
    [SerializeField] private StatusApplier statusApplier;
    [SerializeField] private Lifetime lifetime;


    protected override void HandleRootModifier(int count, Modifier buff)
    {
        base.HandleRootModifier(count, buff);
        var scale = scalableTransform.localScale;
        scalableTransform.localScale = new Vector3(scale.x * buff.Size * count / 100,
            scale.y,
            scale.z * buff.Size * count / 100);

        if (buff.UseStatusEffect)
            statusApplier.SetStatusEffect(buff.StatusEffect);
        statusApplier.AddPowerBonus(buff.StatusPower * count);
    }

    protected override void HandleFireModifier(int count, Modifier buff)
    {
        base.HandleFireModifier(count, buff);
        dealer.AddDamageBonus(buff.Damage * count);
        lifetime.AddTimeBonus(buff.Lifetime * count);
        statusApplier.AddPowerBonus(buff.StatusPower * count);
        if (buff.UseStatusEffect)
        {
            statusApplier.SetStatusEffect(buff.StatusEffect);
        }
    }

    protected override void HandleHarmonyModifier(int count, Modifier buff)
    {
        base.HandleRootModifier(count, buff);
        dealer.AddDamageBonus(buff.Damage * count, buff.Healing);
        lifetime.AddTimeBonus(buff.Lifetime * count);
    }
    protected override void OnModifiersApplied()
    {
        base.OnModifiersApplied();
        lifetime.AddTimeBonus(PlayersInventory.GetStatValue(OwnerClientId, PlayerStat.SpellLifetime) * 100 - 100);
    }
}
