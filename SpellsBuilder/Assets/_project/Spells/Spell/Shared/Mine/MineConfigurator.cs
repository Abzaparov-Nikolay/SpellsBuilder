using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineConfigurator : SpellConfigurator
{
    [SerializeField] private AoeSpawner spawner;

    protected override void OnModifiersApplied()
    {
        base.OnModifiersApplied();
        spawner.modifiersToPass = modifiersList.ToList();
    }
}
