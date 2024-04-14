using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.Magic + "List")]
public class ElementsList : ScriptableObject, IEnumerable<Element>
{
    public List<Element> ElementList;

    public IEnumerator<Element> GetEnumerator()
    {
        return ElementList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
