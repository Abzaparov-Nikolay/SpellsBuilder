//using FishNet.Object;
//using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class MagicInputHandler : NetworkBehaviour
{
    public Action<List<Element>> OnOrderChange;

    [SerializeField] private InputMagicProvider inputMagicProvider;
    [SerializeField] private Spawner Spawner;
    public ElementsList Magics;

    public static Action SpellCastStarting;
    public static Action SpellCastStopped;


    public NetworkList<int> CurrentOrder;

    public void MagicInput(ElementType type)
    {
        if (!IsOwner) return;
        var magic = Magics.First(m => m.Type == type);
        if (!CanOrder(magic)) return;
        HandleCombinationsAndChangeOrder(magic);
    }

    public void CastStarted(CastTarget target)
    {
        if (!IsOwner) return;
        SpellCastStarting?.Invoke();
        Spawner.Spawn(GetElementTypesOrder());
        ChangeOrder(new());
    }

    public void CastStopped()
    {
        if (!IsOwner) return;
        SpellCastStopped?.Invoke();
        Spawner.StopContinuousCast();
    }

    private void Awake()
    {
        inputMagicProvider.CastStarted += CastStarted;
        inputMagicProvider.CastStopped += CastStopped;
        inputMagicProvider.ElementPressed += MagicInput;
        CurrentOrder = new NetworkList<int>(new List<int>(),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    }

    public override void OnDestroy()
    {
        inputMagicProvider.CastStarted -= CastStarted;
        inputMagicProvider.CastStopped -= CastStopped;
        inputMagicProvider.ElementPressed -= MagicInput;
    }

    private void HandleCombinationsAndChangeOrder(Element magic)
    {
        if (!magic.HasCombination(GetElementsOrder()))
            AddToOrder((int)magic.Type);
        else
        {
            var newOrder = magic.HandleCombination(GetElementsOrder());
            ChangeOrder(newOrder.Select(m => (int)m.Type).ToList());
        }

    }

    private bool CanOrder(Element type)
    {
        return type.CanOrder(GetElementsOrder());
    }

    private void AddToOrder(int type)
    {
        CurrentOrder.Add((int)type);
        NotifyServerRpc();
    }

    private void ChangeOrder(List<int> type)
    {
        CurrentOrder.Clear();
        foreach (var magic in type)
        {
            CurrentOrder.Add((int)magic);
        }
        NotifyServerRpc();
    }

    [ServerRpc]
    private void NotifyServerRpc()
    {
        NotifyClientRpc();
    }

    [ClientRpc]
    private void NotifyClientRpc()
    {
        OnOrderChange?.Invoke(GetElementsOrder());
    }

    private List<Element> GetElementsOrder()
    {
        var order = new List<Element>();
        foreach (var e in CurrentOrder)
        {
            order.Add(Magics.First(magic => magic.Type == (ElementType)e));
        }
        return order;
    }

    private List<ElementType> GetElementTypesOrder()
    {
        var order = new List<ElementType>();
        foreach (var e in CurrentOrder)
        {
            order.Add((ElementType)e);
        }
        return order;
    }
}
