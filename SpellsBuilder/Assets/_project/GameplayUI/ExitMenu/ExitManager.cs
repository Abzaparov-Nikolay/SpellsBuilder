using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    private void Start()
    {
        InputMenuProvider.EscapePressed += ChangeVisible;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        InputMenuProvider.EscapePressed -= ChangeVisible;

    }

    public void ChangeVisible()
    {
        this.gameObject.SetActive(!this.gameObject.activeInHierarchy);
    }

    public void Exit()
    {
        BootstrapManager.Instance.Disconnect();
    }
}
