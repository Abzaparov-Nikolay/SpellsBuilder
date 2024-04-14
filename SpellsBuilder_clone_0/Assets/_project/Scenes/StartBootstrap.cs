using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBootstrap : MonoBehaviour
{
    
    // Start is called before the first frame update
    private void Awake()
    {

        if (Bootstraper.Instance == null || !Bootstraper.Instance.bootstraped)
        {
            Debug.Log("didnt bootstraped");
            var current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(Names.BootstrapScene, LoadSceneMode.Single);
            //SceneManager.UnloadSceneAsync(current);
            Debug.Log("bootstraped mb successfully");
            //bootstraped = true;
        }
        else
        {
            Debug.Log("already bootstraped");
            Destroy(gameObject);
        }
    }

    

}
