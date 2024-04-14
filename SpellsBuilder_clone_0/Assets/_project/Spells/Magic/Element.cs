using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.Magic + nameof(Element))]
public class Element : ScriptableObject
{
    [SerializeField] protected List<ElementType> CanceledBy;
    [SerializeField] protected List<ElementCombination> UsedIn;
    public ElementType Type;
    public int Power;
    public Sprite Image;

    public bool CanOrder(List<Element> previous)
    {
        return previous.All(magic => !IsCanceledBy(magic.Type));
    }

    public List<Element> HandleCombination(List<Element> order)
    {
        var newMagics = new List<Element>();
        Element combined = null;
        bool combinationFound = false;

        if (!HasCombination(order)) return null;

        for (var i = 0; i < order.Count; i++)
        {
            if (combinationFound && i != order.Count)
            {
                newMagics.Add(order[i]);
                continue;
            }
            var combination = UsedIn.Find(combination => combination.Uses(order[i], this));
            if (combination == null)
            {
                newMagics.Add(order[i]);
            }
            else
            {
                combined = combination.Result;
                combinationFound = true;
            }
        }
        if (combinationFound)
        {
            newMagics.Add(combined);
        }
        else
        {
            newMagics.Add(this);
        }
        //CombinationFound = combinationFound;
        return newMagics;
    }

    public bool HasCombination(List<Element> order)
    {
        return order.Any(magic => UsedIn.Any(combination => combination.Uses(magic, this)));
    }

    protected bool IsCanceledBy(ElementType other)
    {
        return CanceledBy.Contains(other);
    }
}

public enum ElementType
{
    Stars,
    Sun,
    Void,
    Moon,
    Tree,
    Leaves,
    Roots,
    Harmony,
    Fire
}
