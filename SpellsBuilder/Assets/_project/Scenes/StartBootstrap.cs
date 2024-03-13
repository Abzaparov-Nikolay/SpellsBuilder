using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBootstrap : MonoBehaviour
{
    private static StartBootstrap instance;
    public static bool bootstraped;
    // Start is called before the first frame update
    private void Start()
    {
        if (!bootstraped)
        {
            bootstraped = true;
            Debug.Log("didnt bootstraped");
            var current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(Names.BootstrapScene, LoadSceneMode.Single);
            SceneManager.UnloadSceneAsync(current);
            Debug.Log("bootstraped mb successfully");
            //bootstraped = true;
        }
        else
        {
            Debug.Log("already bootstraped");
        }
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
