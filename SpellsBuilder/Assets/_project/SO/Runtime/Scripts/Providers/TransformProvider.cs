using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformProvider : MonoBehaviour
{
    [SerializeField] private Variable<Transform> variable;

    void Awake()
    {
        variable.Set(transform);
    }
}
