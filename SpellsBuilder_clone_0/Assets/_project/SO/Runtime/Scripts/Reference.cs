using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reference<T>
{
    [SerializeField] private bool useLocal;
    [SerializeField] private T localValue;
    [SerializeField] private Variable<T> variable;

    private event Action LocalOnChanged;
    private event Action<T> LocalOnChangedWithOldValue;

    public event Action OnChanged
    {
        add
        {
            if (useLocal)
                LocalOnChanged += value;
            else
                variable.OnChanged += value;
        }
        remove
        {
            if (useLocal)
                LocalOnChanged -= value;
            else
                variable.OnChanged -= value;
        }
    }

    public event Action<T> OnChangedWithOldValue
    {
        add
        {
            if (useLocal)
                LocalOnChangedWithOldValue += value;
            else
                variable.OnChangedWithOldValue += value;
        }
        remove
        {
            if (useLocal)
                LocalOnChangedWithOldValue -= value;
            else
                variable.OnChangedWithOldValue -= value;
        }
    }

    public T Value
    {
        get => useLocal ? localValue : variable;

        set
        {
            var oldValue = Value;
            if (useLocal)
            {
                localValue = value;
                LocalOnChanged?.Invoke();
                LocalOnChangedWithOldValue?.Invoke(oldValue);
            }
            else
            {
                variable.Value = value;
            }
        }
    }
    public T Get() => useLocal ? localValue : variable;

    public void Set(T newValue)
    {
        var oldValue = Get();
        if (useLocal)
        {
            localValue = newValue;
            LocalOnChanged?.Invoke();
            LocalOnChangedWithOldValue?.Invoke(oldValue);
        }
        else
        {
            variable.Set(newValue);
        }
    }

    public static implicit operator T(Reference<T> v) => v.Get();

    public bool IsValid()
    {
        if (useLocal)
        {
            return localValue != null;
        }
        else
        {
            return variable != null;
        }
    }
}
