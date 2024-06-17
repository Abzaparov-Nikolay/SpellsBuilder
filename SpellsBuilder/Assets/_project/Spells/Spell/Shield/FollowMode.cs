using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FollowMode : NetworkBehaviour
{
    public void SetOwner(ulong ownerId)
    {
        WHAT(ownerId);
    }
    
    private void WHAT(ulong owner)
    {
        NetworkObject.TrySetParent(NetworkManager.Singleton.ConnectedClients[owner].PlayerObject.transform, true);
        transform.localPosition = Vector3.zero;
    }
}
