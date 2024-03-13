using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = MenuNames.Magic + nameof(ElementCombination))]
public class ElementCombination : ScriptableObject
{
    public Element Summand1;
    public Element Summand2;

    public Element Result;

    public bool Uses(Element input)
    {
        return input == Summand1 || input == Summand2;
    }

    public bool Uses(Element magic1, Element magic2)
    {
        return Summand1 == magic1 && Summand2 == magic2
            || Summand1 == magic2 && Summand2 == magic1;
    }
}
