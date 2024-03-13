using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =MenuNames.SpellDefaultValues + nameof(SpawnAttacker))]
public class SpawnAttackerBase : ScriptableObject
{
    public List<SpawnTypeAttackRate> attackRates;

}
