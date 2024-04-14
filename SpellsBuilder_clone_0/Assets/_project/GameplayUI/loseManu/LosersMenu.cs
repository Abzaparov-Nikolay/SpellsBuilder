//using FishNet.Managing.Scened;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LosersMenu : MonoBehaviour
{
    private void Start()
    {
        GameStater.GameEnded += GameLost;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameStater.GameEnded -= GameLost;
    }

    public void GameLost()
    {
        this.gameObject.SetActive(true);
    }

    public void Disconnect()
    {
        BootstrapManager.Instance.Disconnect();
    }
}