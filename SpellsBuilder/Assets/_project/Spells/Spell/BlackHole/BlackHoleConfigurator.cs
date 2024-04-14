using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleConfigurator : SpellConfigurator
{
    [SerializeField] private AreaTracker PullArea;
    [SerializeField] private DamageDealer damageDealer;
    [SerializeField] private Puller puller;
    [SerializeField] private Lifetime lifetime;
    [SerializeField] private BlackHoleController controller;
    [SerializeField] private StatusApplier statusApplier;

    protected override void HandleMoonModifier(int count, Modifier buff)
    {
        //bigger sucktion radius
        base.HandleMoonModifier(count, buff);
        PullArea.AddBonus(buff.PullArea * count);
        lifetime.AddTimeBonus(buff.Lifetime * count);
    }

    protected override void HandleVoidModifier(int count, Modifier buff)
    {
        //bigger damage + sucktion power
        base.HandleVoidModifier(count, buff);
        damageDealer.AddDamageBonus(buff.Damage * count);
        puller.AddPullBonus(buff.PullForce * count);
    }

    protected override void HandleHarmonyModifier(int count, Modifier buff)
    {
        //pull out
        base.HandleHarmonyModifier(count, buff);
        puller.SetPull(buff.PullOut);
        lifetime.AddTimeBonus(buff.Lifetime * count);
    }

    protected override void HandleTreeModifier(int count, Modifier buff)
    {
        base.HandleTreeModifier(count, buff);
        controller.StopMoving();
        //stand still
    }

    protected override void HandleFireModifier(int count, Modifier buff)
    {
        base.HandleFireModifier(count, buff);
        //shorter lifetime
        lifetime.AddTimeBonus(buff.Lifetime * count);
        //deal more damage
        damageDealer.AddDamageBonus(buff.Damage * count);
        //status type on fire
        statusApplier.SetStatusEffect(buff.StatusEffect);
    }

    protected override void HandleLeavesModifier(int count, Modifier buff)
    {
        //deal more damage
        damageDealer.AddDamageBonus(buff.Damage * count);
        //status bleed
        statusApplier.SetStatusEffect(buff.StatusEffect);
    }

    protected override void HandleRootModifier(int count, Modifier buff)
    {
        base.HandleRootModifier(count, buff);
        //less damage
        damageDealer.AddDamageBonus(buff.Damage * count);
        //status rooted
        statusApplier.SetStatusEffect(buff.StatusEffect);
        //move randomly
        controller.SetMoveDirection(Vector3.zero);
    }

    protected override void OnModifiersApplied()
    {
        base.OnModifiersApplied();
        lifetime.AddTimeBonus(PlayersInventory.GetStatValue(OwnerClientId, PlayerStat.SpellLifetime) * 100 - 100);
    }
}
