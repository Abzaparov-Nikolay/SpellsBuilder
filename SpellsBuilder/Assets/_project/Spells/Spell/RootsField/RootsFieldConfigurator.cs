//using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RootsFieldConfigurator : SpellConfigurator
{

    [SerializeField] private Transform scalableTransform;
    [SerializeField] private Lifetime lifetime;
    [SerializeField] private DamageDealer damageDealer;
    [SerializeField] private StatusApplier statusApplier;
    [SerializeField] private MeleeAttacker attacker;

    //[SerializeField] private RootsModifiersData buffs;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) { return; }
        ApplyModifiers();
    }

    protected override void HandleRootModifier(int count, Modifier buff)
    {
        //bigger
        scalableTransform.localScale += new Vector3(buff.Size * count / 100,
            0,
            buff.Size * count / 100);
        //bigger damage
        damageDealer.AddDamageBonus(buff.Damage * count);
        //element status bigger
        statusApplier.AddPowerBonus(buff.StatusPower * count);
    }

    protected override void HandleFireModifier(int count, Modifier buff)
    {
        //damage big
        damageDealer.AddDamageBonus(buff.Damage * count);
        //lifetime shorter
        lifetime.AddTimeBonus(buff.Lifetime * count);
        //applies burn instead of slow
        statusApplier.SetStatusEffect(buff.StatusEffect);
    }

    protected override void HandleHarmonyModifier(int count, Modifier buff)
    {
        //damage negative
        damageDealer.AddDamageBonus(buff.Damage * count, true);
        // livetime bigger
        lifetime.AddTimeBonus(buff.Lifetime * count);
    }
}
