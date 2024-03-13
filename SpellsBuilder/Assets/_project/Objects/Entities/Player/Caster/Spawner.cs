//using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private SpellsList AllSpells;
    [SerializeField] private ElementsList AllElements;

    [SerializeField] private GameObject holdableSpell;



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

        SpawnServerServerRpc(new SpawnData(objectToBytes(order), SpawnPoint.position, SpawnPoint.rotation));
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
        var sameTypeSpells = AllSpells.Where(spell => spell.elementsRequired.MinBy(el => el.Power) == max).ToList();
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
    //GameObject spell, CastType castType, List<ElementType> modifiers, Vector3 spawnPosition, Quaternion rotation)
    {
        List<ElementType> order = bytesToObject(data.values);
        var spell = GetSpellFromOrder(order
            .Select(type => AllElements
            .First(el => el.Type == type)).ToList(),
            out var modifiers);
        if (spell == null) return;


        if (holdableSpell != null) return;
        var spawned = spell.castType == CastType.Tap
            ? Instantiate(spell.prefab, new Vector3(data.spawnPosition.x, 0, data.spawnPosition.z), data.rotation)
            : Instantiate(spell.prefab, this.transform);
        spawned.GetComponent<NetworkObject>().Spawn(true);
        //ServerManager.Spawn(spawned);
        spawned.GetComponent<SpellConfigurator>()
            .SetModifiers(modifiers.Select(el => el.Type).ToList());
        if (spell.castType == CastType.Hold)
        {
            holdableSpell = spawned;
        }
    }



    [ServerRpc]
    private void DespawnSpellServerServerRpc()
    {
        if (holdableSpell != null /*&& !holdableSpell.IsDestroyed()*/)
        {
            Destroy(holdableSpell);
            //ServerManager.Despawn(holdableSpell);
        }
        holdableSpell = null;
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

        public SpawnData(byte[] values, Vector3 spawnPosition, Quaternion rotation)
        {
            this.values = values;
            this.spawnPosition = spawnPosition;
            this.rotation = rotation;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref values);
            serializer.SerializeValue(ref spawnPosition);
            serializer.SerializeValue(ref rotation);
        }
    }
}
