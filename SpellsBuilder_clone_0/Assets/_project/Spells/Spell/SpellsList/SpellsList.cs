using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.Spell + nameof(SpellsList))]
public class SpellsList : ScriptableObject, IEnumerable<Spell>
{
    public List<Spell> SpellList;

    public IEnumerator<Spell> GetEnumerator()
    {
        return SpellList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
