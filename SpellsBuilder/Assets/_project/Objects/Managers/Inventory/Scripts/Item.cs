using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.Item + "Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public List<Buff> Buffs;
}
