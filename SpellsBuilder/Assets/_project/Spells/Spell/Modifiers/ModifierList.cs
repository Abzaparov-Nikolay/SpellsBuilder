using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.SpellModifierList+nameof(ModifierList))]
public class ModifierList : ScriptableObject, IEnumerable<Modifier>
{
    public List<Modifier> ModifiersList;

    public IEnumerator<Modifier> GetEnumerator()
    {
        return ModifiersList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
