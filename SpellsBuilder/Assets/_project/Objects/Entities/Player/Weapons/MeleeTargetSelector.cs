using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeTargetSelector : TargetSelector
{
    [SerializeField] private AreaTracker tracker;

    public override bool TryGetTarget(out Transform[] targets)
    {
        targets = tracker.Where(t => t != null).ToArray();
        return targets.Length != 0;
    }
}
