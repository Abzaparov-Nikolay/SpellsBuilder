using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30;
    private void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,
            transform.eulerAngles.y,
            transform.eulerAngles.z + rotationSpeed * Time.deltaTime);

    }
}
