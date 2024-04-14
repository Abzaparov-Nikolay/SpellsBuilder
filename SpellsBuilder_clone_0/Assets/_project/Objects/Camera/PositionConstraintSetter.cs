using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PositionConstraintSetter : MonoBehaviour
{

    [SerializeField] private PositionConstraint constraint;
    private void Awake()
    {
        OnFirstSpanw.OnObjectClienSpawn += Set;
    }

    private void OnDestroy()
    {
        OnFirstSpanw.OnObjectClienSpawn -= Set;
    }


    public void Set(Transform t)
    {
        var source = new ConstraintSource();
        source.sourceTransform = t;
        source.weight = 1;
        constraint.AddSource(source);
    }
}
