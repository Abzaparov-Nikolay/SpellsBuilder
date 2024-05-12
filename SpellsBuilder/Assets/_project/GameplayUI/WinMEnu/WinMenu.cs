using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    private void Start()
    {
        GameStater.GameWon += GameWon;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameStater.GameWon -= GameWon;
    }

    private void GameWon()
    {
        this.gameObject.SetActive(true);
    }

    public void Disconnect()
    {
        BootstrapManager.Instance.Disconnect();
    }
}
