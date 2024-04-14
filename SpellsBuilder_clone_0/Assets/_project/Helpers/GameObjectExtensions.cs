using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    public static bool TryGetComponentInParent<T>(this GameObject self, out T component) where T : Component
    {
        component = self.GetComponentInParent<T>();
        return component != null;
    }
}
