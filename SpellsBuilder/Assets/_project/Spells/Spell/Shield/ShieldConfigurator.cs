using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldConfigurator : SpellConfigurator
{
    [SerializeField] private FollowMode follower;

    protected override void OnOwnerSet()
    {
        base.OnOwnerSet();
        follower.SetOwner(TrueOwnerId);
    }
}
