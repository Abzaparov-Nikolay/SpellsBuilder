using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementUI : MonoBehaviour
{
    [SerializeField] private Image Imageg;

    public void SetFromImage(Sprite image)
    {
        Imageg.sprite = image;
    }
}
