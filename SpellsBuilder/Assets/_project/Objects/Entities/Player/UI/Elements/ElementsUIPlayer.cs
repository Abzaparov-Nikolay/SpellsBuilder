//using FishNet.Managing.Statistic;
//using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsUIPlayer : MonoBehaviour
{
    [SerializeField] private MagicInputHandler handler;

    //public static Action<float, float> OnHealthChange;
    [SerializeField] private GameObject elementsParent;
    [SerializeField] private GameObject elementUIPrefab;


    public void ElementCasted(List<Element> elements)
    {
        //clear parent
        foreach(Transform child in elementsParent.transform)
        {
            Destroy(child.gameObject);
        }
        //spwn images in parent
        foreach(var el in elements)
        {
            var elem= Instantiate(elementUIPrefab, elementsParent.transform);
            elem.GetComponent<ElementUI>().SetFromImage(el.Image);
        }
    }

    private void OnEnable()
    {
        handler.OnOrderChange += ElementCasted;
    }

    private void OnDisable()
    {
        handler.OnOrderChange -= ElementCasted;
    }
}
