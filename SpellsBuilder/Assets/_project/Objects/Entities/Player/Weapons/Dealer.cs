//using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Dealer : NetworkBehaviour
{
    public abstract void Deal(GameObject target);
}
