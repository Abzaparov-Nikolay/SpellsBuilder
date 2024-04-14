using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.SpellModifiers + nameof(Modifier))]
public class Modifier : ScriptableObject
{
    public ElementType ModifierType;
    public int ModifierPower;

    public bool Healing;
    public float Damage;

    public float Size;
    public float Health;
    public float Lifetime;

    public float StatusPower;
    public bool UseStatusEffect;
    public StatusEffect StatusEffect;

    public float Firerate;
    public float Accuracy;
    public float SpawnImpulse;
    public GameObject FiredPrefab;
    public AttackSpawnType AttackSpawnType;

    public bool PullOut;
    public float PullForce;
    public float PullArea;
}
