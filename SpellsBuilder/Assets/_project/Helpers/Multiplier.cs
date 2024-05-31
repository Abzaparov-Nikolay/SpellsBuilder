//using FishNet.Object.Synchronizing;
//using FishNet.Object.Synchronizing.Internal;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Multiplier : IEnumerable<float>
{
    public Action OnChange;

    private List<float> Multipliers = new() { 100 };

    public IEnumerator<float> GetEnumerator()
    {
        return Multipliers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator float(Multiplier list) => list.Sum() / 100;

    public void Add(float percentage)
    {
        Multipliers.Add(percentage);
        OnChange?.Invoke();
    }

    public object GetSerializedType()
    {
        return typeof(List<float>);
    }

    //public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    //{
    //    serializer.SerializeValue(ref this.Multipliers);
    //}
}
