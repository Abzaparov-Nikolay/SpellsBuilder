using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.Item + "List")]
public class ItemsList : ScriptableObject, IEnumerable<Item>
{
    [SerializeField] private List<Item> items;

    public List<Item> Items => items;

    public IEnumerator<Item> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
