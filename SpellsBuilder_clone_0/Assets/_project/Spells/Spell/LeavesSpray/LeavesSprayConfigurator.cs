//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeavesSprayConfigurator : SpellConfigurator
{
    [SerializeField] private LeavesSprayer sprayer;

    protected override void OnModifiersApplied()
    {
        sprayer.leavesModificators = modifiersList;
    }

    protected override void HandleFireModifier(int count, Modifier buff)
    {

    }

    protected override void HandleHarmonyModifier(int count, Modifier buff)
    {

    }

    protected override void HandleLeavesModifier(int count, Modifier buff)
    {
        sprayer.AddAccuracyBonus(buff.Accuracy * count / 100);
        //lower rasbros
        sprayer.AddFirerateBonus(buff.Firerate * count / 100);
        //higher firerate 
        sprayer.AddLeafImpulseBonus(buff.SpawnImpulse * count / 100);
        //leafs fly faster
    }
}
