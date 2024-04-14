using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClosestTargetSelector : TargetSelector
{
    [SerializeField] private AreaTracker tracker;

    public override bool TryGetTarget(out Transform[] targets)
    {
        targets = new Transform[] { tracker.Where(entity => entity != null
        && entity.gameObject.activeInHierarchy)
            .MinBy(entity => Vector3.Distance(entity.position, transform.position)) };
        return targets[0] != null;
    }
}
