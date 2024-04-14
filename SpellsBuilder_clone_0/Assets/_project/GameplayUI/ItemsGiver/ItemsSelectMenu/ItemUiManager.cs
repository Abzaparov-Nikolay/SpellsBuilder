using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUiManager : MonoBehaviour
{
    private Item item;
    [SerializeField]private Image image;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemBuffs;

    [HideInInspector] public ItemMenuManager manager;

    public void SetFromItem(Item item)
    {
        image.sprite = item.Sprite;
        itemName.text = item.Name;
        itemBuffs.text = "";
        this.item = item;
        foreach (var b in item.Buffs)
        {
            itemBuffs.text += b.GetString();
        }
    }

    public void OnChoose()
    {
        manager.ItemChosen(item);
    }
}
