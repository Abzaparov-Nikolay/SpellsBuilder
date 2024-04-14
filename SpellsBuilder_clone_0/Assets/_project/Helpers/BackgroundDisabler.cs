using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BackgroundDisabler : MonoBehaviour
{
    private void Start()
    {
        NetworkManager.Singleton.OnClientStarted += DisableBackground;
        NetworkManager.Singleton.OnClientStopped += EnableBackground;
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientStarted -= DisableBackground;
            NetworkManager.Singleton.OnClientStopped -= EnableBackground;
        }
    }

    private void DisableBackground()
    {
        this.gameObject.SetActive(false);
    }

    private void EnableBackground(bool isHost)
    {
        this.gameObject.SetActive(true);
    }
}
