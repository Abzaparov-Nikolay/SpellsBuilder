using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.Spell + nameof(Spell))]
public class Spell : ScriptableObject
{
    public GameObject prefab;
    public List<Element> elementsRequired;
    public CastType castType;
}


public enum CastType
{
    Tap,
    Hold
}