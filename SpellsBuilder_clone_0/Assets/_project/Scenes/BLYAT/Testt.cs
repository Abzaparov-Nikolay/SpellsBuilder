using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testt : MonoBehaviour
{
    public bool ENabled;
    private void Awake()
    {
        Debug.developerConsoleEnabled= true;
        //Debug.developerConsoleVisible = true;
        //DontDestroyOnLoad(gameObject);
    }

    public void ChangeVisibility()
    {
        ENabled = !Debug.developerConsoleVisible;
        Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
    }

}
