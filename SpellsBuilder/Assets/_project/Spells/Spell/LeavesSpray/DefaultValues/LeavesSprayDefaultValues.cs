using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.SpellDefaultValues + nameof(LeavesSprayDefaultValues))]
public class LeavesSprayDefaultValues : ScriptableObject
{
    public float fireRate;
    public float firerateMultiplier;
    public float degreesOfAttack;
    public float degreesOfAttackMultiplier;
    public float leafImpulse;
    public float leafImpulseMultiplier;
}
