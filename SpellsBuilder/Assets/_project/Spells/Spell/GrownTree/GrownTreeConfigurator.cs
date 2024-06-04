using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrownTreeConfigurator : SpellConfigurator
{
    [SerializeField] private SpawnAttacker attacker;
    [SerializeField] private EnemyAggrer aggrer;
    [SerializeField] private Lifetime lifetime;
    [SerializeField] private VusialChange visualChanger;

    [SerializeField] private Health health;


    //derevo attract nearby enemies
    protected override void HandleTreeModifier(int count, Modifier buff)
    {
        //bigger hp
        health.AddBonus(buff.Health * count);
    }

    protected override void HandleRootModifier(int count, Modifier buff)
    {
        //attack with roots
        attacker.Configure(buff.Firerate * count,
            buff.FiredPrefab,
            buff.AttackSpawnType,
            modifiersList);
    }

    protected override void HandleLeavesModifier(int count, Modifier buff)
    {
        //attack with leaves
        attacker.Configure(buff.Firerate * count,
            buff.FiredPrefab,
            buff.AttackSpawnType,
            modifiersList);
    }

    protected override void HandleFireModifier(int count, Modifier buff)
    {
        //go into leaves and roots
        //or
        //attack with fire aoe attack
        if (!modifiersList.Any(type => type == ElementType.Roots || type == ElementType.Leaves))
        {
            visualChanger.Attack();
            attacker.Configure(buff.Firerate * count,
                buff.FiredPrefab,
                buff.AttackSpawnType,
                modifiersList);
        }
    }

    protected override void HandleHarmonyModifier(int count, Modifier buff)
    {
        //go into leaves or roots
        //or
        // attack with harmony aoe attack
        lifetime.AddTimeBonus(buff.Lifetime * count);
        health.AddBonus(buff.Health * count);

        if (!modifiersList.Any(type => type == ElementType.Roots || type == ElementType.Leaves))
        {
            attacker.Configure(buff.Firerate * count,
                buff.FiredPrefab,
                buff.AttackSpawnType,
                modifiersList);
            visualChanger.Heal();
        }
    }

    protected override void OnModifiersApplied()
    {
        base.OnModifiersApplied();
        lifetime.AddTimeBonus(PlayersInventory.GetStatValue(OwnerClientId, PlayerStat.SpellLifetime) * 100 - 100);
        visualChanger.Play();
    }
}
