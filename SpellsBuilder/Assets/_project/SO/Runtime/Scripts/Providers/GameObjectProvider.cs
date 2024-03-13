using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectProvider : MonoBehaviour
{
    [SerializeField] private Variable<GameObject> variable;

    private void Awake()
    {
        variable.Set(this.gameObject);
    }
}
