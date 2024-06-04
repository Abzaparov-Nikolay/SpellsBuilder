//using FishNet.Object;
//using GameKit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafConfigurator : SpellConfigurator
{
    public StatusApplier statusApplier;
    public Lifetime lifetime;
    public DamageDealer damageDealer;
    public Transform scalableTransform;
    public VusialChange visualChanger;
    protected override void OnModifiersApplied()
    {
        base.OnModifiersApplied();
        visualChanger.Attack();
        visualChanger.Play();
    }
    protected override void HandleFireModifier(int count, Modifier buff)
    {
        if (buff.UseStatusEffect)
            statusApplier.SetStatusEffect(buff.StatusEffect);
        //apply burn instead of bleed
        statusApplier.AddPowerBonus(buff.StatusPower * count);
        //bigger status power
        lifetime.AddTimeBonus(count * buff.Lifetime);
        visualChanger.Heal();
        //lower lifetime of spawned leaves
    }

    protected override void HandleHarmonyModifier(int count, Modifier buff)
    {
        //healing + more healing
        damageDealer.AddDamageBonus(count * buff.Damage, buff.Healing);
        //idea:leaf seeks out least healed player
    }

    protected override void HandleLeavesModifier(int count, Modifier buff)
    {
        //bigger leaves
        var scale = scalableTransform.localScale;
        scalableTransform.localScale = new Vector3(scale.x * (1 + count * buff.Size / 100),
            scale.y * (1 + count * buff.Size / 100),
            scale.z * (1 + count * buff.Size / 100));
        //bigger damage
        damageDealer.AddDamageBonus(count * buff.Damage);
        //bigger status power
        statusApplier.AddPowerBonus(count * buff.StatusPower);
    }
}
