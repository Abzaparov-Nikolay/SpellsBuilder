//using FishNet.Managing.Scened;
using System.Collections;
using System.Collections.Generic;
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
        //Disconnect();
    }

    public void Disconnect()
    {
        //var scenesToClose = new string[] { Names.GameplayScene };
        //BootstrapNetworkManager.ChangeNetworkScene(Names.MainMenuScene, scenesToClose);
        BootstrapNetworkManager.Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(Names.MainMenuScene, LoadSceneMode.Additive);

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(Names.GameplayScene);

        //BootstrapManager.LeaveLobby();
    }
}
