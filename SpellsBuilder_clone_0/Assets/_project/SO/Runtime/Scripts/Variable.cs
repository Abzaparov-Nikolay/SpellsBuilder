using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Variable<T> : ScriptableObject
{
    [SerializeField] protected T initialValue;

    [Header(Headers.Runtime)]
    [SerializeField] protected T value;
    [SerializeField] private bool resetBetweenScenes = true;
    public event Action OnChanged;
    public event Action<T> OnChangedWithOldValue;

    private bool initialized;
    public virtual T Value
    {
        get => Get();

        set
        {
            Set(value);
        }
    }

    public virtual T Get()
    {
        if (!initialized)
        {
            value = initialValue;
            initialized = true;
        }
        return value;
    }

    public void Set(T newValue)
    {
        if (!initialized)
        {
            value = initialValue;
            initialized = true;
        }
        var oldValue = value;
        value = newValue;
        OnChanged?.Invoke();
        OnChangedWithOldValue?.Invoke(oldValue);
    }

    private void OnEnable()
    {
        Set(initialValue);
        SceneManager.sceneUnloaded += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene scene)
    {
        if (resetBetweenScenes)
            initialized = false;
    }

    public static implicit operator T(Variable<T> v) => v.Get();

    protected void RaiseOnChanged()
    {
        OnChanged?.Invoke();
    }
}