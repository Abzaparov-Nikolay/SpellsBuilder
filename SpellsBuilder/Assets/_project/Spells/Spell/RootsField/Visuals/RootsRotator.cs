using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsRotator : MonoBehaviour
{
    public List<Transform> RotatableTransforms;
    [SerializeField] private float speed;

    private void Update()
    {
        for(var i = 0; i < RotatableTransforms.Count; i++)
        {
            RotatableTransforms[i].rotation *= Quaternion.Euler(speed * Time.deltaTime, 0, 0);
        }
    }
}
