using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersInventory : NetworkBehaviour
{
    private static PlayersInventory _instance;
    [SerializeField] private ItemsList AddOnSpawn;
    private Dictionary<ulong, List<Item>> playersItems = new();
    private Dictionary<ulong, PlayerStats> playersStats = new();

    private void Awake()
    {
        _instance = this;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _instance = null;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        OnFirstSpanw.OnObjectServerSpawn += CreateInventory;
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        OnFirstSpanw.OnObjectServerSpawn -= CreateInventory;

    }



    private void CreateInventory(Transform playerTr)
    {
        var ownerid = playerTr.GetComponent<NetworkObject>().OwnerClientId;
        playersItems[ownerid] = new();
        playersStats[ownerid] = new();
        foreach(var item in AddOnSpawn)
        {
            GiveItem(ownerid, item);
        }
    }

    public static void GiveItem(ulong player, Item item)
    {
        _instance.playersItems[player].Add(item);
        foreach(var buff in item.Buffs)
        {
            _instance.playersStats[player].AddBonus(buff.Stat, buff.Value);
        }
    }

    public static float GetStatValue(ulong player, PlayerStat stat)
    {
        if (_instance == null) return 1;
        return _instance.playersStats[player].GetValue(stat);
    }
}










