//using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine;


public class Spawner : NetworkBehaviour
{
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private SpellsList AllSpells;
    [SerializeField] private ElementsList AllElements;

    [SerializeField] private List<GameObject> holdableSpells;
    private float InventorySpellsSpawned => PlayersInventory.GetStatValue(OwnerClientId, PlayerStat.SpawnedSpells);


    public void Spawn(List<ElementType> order)
    {
        //get spell 
        var spell = GetSpellFromOrder(order
            .Select(type => AllElements
            .First(el => el.Type == type)).ToList(),
            out var modifiers);
        if (spell == null) return;


        //server spawn and modify
        //SpawnServerServerRpc(spell.prefab,
        //    spell.castType,
        //    modifiers.Select(m => m.Type).ToList(),
        //    SpawnPoint.position,
        //    SpawnPoint.rotation);
        var bytes = objectToBytes(order);
        SpawnServerServerRpc(new SpawnData(bytes,
            SpawnPoint.position,
            SpawnPoint.rotation,
            NetworkManager.LocalClientId));
        //bytes.Dispose();

    }


    public void StopContinuousCast()
    {
        if (!IsOwner) return;
        DespawnSpellServerServerRpc();
        //SpellCastStopped?.Invoke();
    }

    private Spell GetSpellFromOrder(List<Element> order, out List<Element> modifiers)
    {
        if (order.Count == 0)
        {
            modifiers = null;
            return null;
        }
        var max = order.MinBy(el => el.Power);
        var sameTypeSpells = AllSpells.Where(spell => spell.elementsRequired.MinBy(el => el.Power).Power == max.Power).ToList();
        var neededSpell = sameTypeSpells.MaxBy(spell =>
        {
            int same = 0;
            foreach (var reqEl in spell.elementsRequired)
            {
                if (order.Contains(reqEl)) same++;
            }
            return same;
        });
        modifiers = order.ToList();

        if (neededSpell == null) return null;

        foreach (var el in neededSpell.elementsRequired)
        {
            modifiers.RemoveAt(modifiers.FindIndex(element => element.Type == el.Type));
        }
        return neededSpell;
    }

    [ServerRpc]
    private void SpawnServerServerRpc(SpawnData data)
    {
        List<ElementType> order = bytesToObject(data.values);
        var spell = GetSpellFromOrder(order
            .Select(type => AllElements
            .First(el => el.Type == type)).ToList(),
            out var modifiers);
        if (spell == null) return;


        if (holdableSpells.Count != 0) return;

        var multicast = (int)UnityEngine.Random.Range(1, InventorySpellsSpawned);
        for (var i = 0; i < multicast; i++)
        {
            var vector = spell.castType == CastType.Tap
                ? new Vector3(data.spawnPosition.x, 0, data.spawnPosition.z) + ShiftOnCast(i)
                : new Vector3(data.spawnPosition.x, 1, data.spawnPosition.z) + ShiftOnCast(i);
            var rot = ShiftOnCast(i, data.rotation);
            var spawned = spell.castType == CastType.Tap
            ? Instantiate(spell.prefab, vector, rot)
            : Instantiate(spell.prefab, vector, rot);
            var no = spawned.GetComponent<NetworkObject>();
            spawned.GetComponent<SpellConfigurator>()
                .SetModifiers(modifiers.Select(el => el.Type).ToList());
            no.SpawnWithOwnership(data.clientId, true);
            if (spell.castType == CastType.Hold)
            {
                no.TrySetParent(base.NetworkObject, true);
                holdableSpells.Add(spawned);

            }



        }




    }

    private Vector3 ShiftOnCast(int cast)
    {
        return new Vector3(UnityEngine.Random.Range(-1, 1) * cast * 3,
            0,
            UnityEngine.Random.Range(-1, 1) * cast * 3);
    }

    private Quaternion ShiftOnCast(int cast, Quaternion defaultRot)
    {
        var newRot = new Quaternion();
        newRot.eulerAngles = new Vector3(defaultRot.eulerAngles.x,
            UnityEngine.Random.Range(0, 30) * cast + defaultRot.eulerAngles.y,
            defaultRot.eulerAngles.z);
        return newRot;
    }


    [ServerRpc]
    private void DespawnSpellServerServerRpc()
    {
        if (holdableSpells.Count != 0)
        {
            foreach(var go in holdableSpells)
            {
                Destroy(go);
            }
        }
        holdableSpells.Clear();
    }

    private BinaryFormatter bf = new BinaryFormatter();

    private byte[] objectToBytes(List<ElementType> os)
    {
        MemoryStream stream = new MemoryStream();
        bf.Serialize(stream, os);
        return stream.ToArray();
    }

    private List<ElementType> bytesToObject(byte[] bytes)
    {
        MemoryStream stream = new MemoryStream(bytes);
        return (List<ElementType>)bf.Deserialize(stream);
    }

    private struct SpawnData : INetworkSerializable
    {
        public byte[] values;
        public Vector3 spawnPosition;
        public Quaternion rotation;
        public ulong clientId;

        public SpawnData(byte[] values, Vector3 spawnPosition, Quaternion rotation, ulong clientId)
        {
            this.values = values;
            this.spawnPosition = spawnPosition;
            this.rotation = rotation;
            this.clientId = clientId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref values);
            serializer.SerializeValue(ref spawnPosition);
            serializer.SerializeValue(ref rotation);
        }
    }
}
