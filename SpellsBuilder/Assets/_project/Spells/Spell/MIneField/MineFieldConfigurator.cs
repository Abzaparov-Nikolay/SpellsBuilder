using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MineFieldConfigurator : SpellConfigurator
{
    public MineField field;

    protected override void OnModifiersApplied()
    {
        base.OnModifiersApplied();
        field.Configure(modifiersList);
    }
}
