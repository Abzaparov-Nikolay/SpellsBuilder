using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DamageTextData
{
    public float number;
    public Color color;
    public Vector3 velocity;

    public DamageTextData(float number, Color color, Vector3 velocity)
    {
        this.number = number;
        this.color = color;
        this.velocity = velocity;
    }
    //public DamageTextData() { }
}
