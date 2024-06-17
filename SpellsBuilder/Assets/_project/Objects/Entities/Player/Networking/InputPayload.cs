using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct InputPayload : INetworkSerializable
{
    public int tick;
    public DateTime timeStamp;
    public ulong networkObjectId; 
    public Vector2 inputVector;
    public Vector3 position;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref tick);
        serializer.SerializeValue(ref timeStamp);
        serializer.SerializeValue(ref networkObjectId);
        serializer.SerializeValue(ref inputVector);
        serializer.SerializeValue(ref position);
    }
}

public struct StatePayload : INetworkSerializable
{
    public int tick;
    public ulong networkObjectId;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref tick);
        serializer.SerializeValue(ref networkObjectId);
        serializer.SerializeValue(ref position);
        serializer.SerializeValue(ref rotation);
        serializer.SerializeValue(ref velocity);
        serializer.SerializeValue(ref angularVelocity);
    }
}
