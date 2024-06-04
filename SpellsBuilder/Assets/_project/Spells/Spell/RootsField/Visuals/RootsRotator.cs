using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsRotator : MonoBehaviour
{
    public List<Transform> RotatableTransforms;
    [SerializeField] private float speed;

    [SerializeField] private bool yRot;

    private void Update()
    {
        for(var i = 0; i < RotatableTransforms.Count; i++)
        {
            if (yRot)
                RotatableTransforms[i].rotation *= Quaternion.Euler(speed * Time.deltaTime, speed * Time.deltaTime, 0);
            else
                RotatableTransforms[i].rotation *= Quaternion.Euler(speed * Time.deltaTime, 0, 0);

        }
    }
}
