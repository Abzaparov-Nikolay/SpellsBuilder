using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Doer : MonoBehaviour
{
    [SerializeField] private bool doOnStart = true;
    public UnityEvent Work;

    private void Start()
    {
        if (doOnStart)
        {
            Work?.Invoke();
        }
    }

    public void Do()
    {
        Work?.Invoke();
    }
}
