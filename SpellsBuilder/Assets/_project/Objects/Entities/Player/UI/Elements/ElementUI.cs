using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementUI : MonoBehaviour
{
    [SerializeField] private Image Imageg;
    //[SerializeField] private SpriteRenderer SpriteRend;

    public void SetFromImage(Material mat)
    {
        if(Imageg != null)
        {
            Imageg.material = mat;
            //Imageg.material.SetTexture("_MainTex", image.texture);
            //Imageg.material.SetTexture("_GlowTex", image.texture);
            //Imageg.material.SetColor("_GlowColor", Color.green);
        }
        //else
        //{
        //    SpriteRend.sprite = image;
        //    SpriteRend.material.SetTexture("_MainTex", image.texture);
        //    SpriteRend.material.SetTexture("_GlowTex", image.texture);
        //}
    }
}
