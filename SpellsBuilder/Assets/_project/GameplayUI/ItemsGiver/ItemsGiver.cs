using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemsGiver : NetworkBehaviour
{
    private int playersReady = 0;
    [SerializeField] private ItemsList allItems;
    [SerializeField] private ItemMenuManager itemsMenuManager;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        ExperienceManager.OnLvlUp += LevelGained;

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        ExperienceManager.OnLvlUp -= LevelGained;

    }

    public void LevelGained(int newLevel)
    {
        if (!IsServer) return;
        GameStater.StopGame();
        ShowPlayersItems();
    }

    public void ChoosesMade()
    {
        if (!IsServer) return;
        GameStater.Continue();
    }

    public void ShowPlayersItems()
    {
        if (!IsServer) return;

        (int first, int second, int third) = (Random.Range(0, allItems.Items.Count),
            Random.Range(0, allItems.Items.Count),
            Random.Range(0, allItems.Items.Count));

        playersReady = 0;
        ShowItemsClientRpc(first, second, third);
    }

    [ClientRpc]
    public void ShowItemsClientRpc(int first, int second, int third)
    {
        itemsMenuManager.CreateItems(new(){allItems.Items[first],
            allItems.Items[second],
            allItems.Items[third]});
    }

    public void MyManPickedThisItem(Item item)
    {
        GiveItemServerRpc(base.OwnerClientId, allItems.Items.FindIndex(s => s == item));
    }

    [ServerRpc(RequireOwnership = false)]
    public void GiveItemServerRpc(ulong clientId, int index)
    {
        PlayersInventory.GiveItem(clientId, allItems.Items[index]);
        playersReady += 1;
        if (playersReady >= NetworkManager.ConnectedClientsIds.Count)
        {
            ChoosesMade();
        }
    }
}
