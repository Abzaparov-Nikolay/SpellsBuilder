using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class upnpChecker : MonoBehaviour
{
    [SerializeField] private Image image;
    private void Start()
    {
        StartCoroutine(StartChecking());
    }

    private void OnDestroy()
    {

    }

    

    private void Check()
    {
        if (uPnPHelper.GetNATType() == uPnPHelper.NatType.Closed
            || uPnPHelper.GetNATType() == uPnPHelper.NatType.Failed)
        {
            image.color = Color.red;
        }
        else if (uPnPHelper.GetNATType() == uPnPHelper.NatType.Open)
        {
            image.color = Color.green;
        }
    }

    IEnumerator StartChecking()
    {
        while (true)
        {
            Check();
            yield return new WaitForSecondsRealtime(3);
        }
    }
}
