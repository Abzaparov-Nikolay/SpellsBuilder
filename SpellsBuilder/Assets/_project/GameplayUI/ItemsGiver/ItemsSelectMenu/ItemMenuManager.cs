using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuManager : MonoBehaviour
{
    public GameObject[] slots = new GameObject[3];
    [SerializeField] private ItemsGiver itemsGiver;
    [SerializeField] private GameObject itemUiCardPrefab;

    public void CreateItems(List<Item> itemsToCreate)
    {
        gameObject.SetActive(true);
        for (var i = 0; i < slots.Length; i++)
        {
            var firstItem = Instantiate(itemUiCardPrefab, slots[i].transform);
            firstItem.GetComponent<ItemUiManager>().SetFromItem(itemsToCreate[i]);
            firstItem.GetComponent<ItemUiManager>().manager = this;
        }
    }

    public void ItemChosen(Item item)
    {
        itemsGiver.MyManPickedThisItem(item);
        ClearItems();
    }

    public void ClearItems()
    {
        foreach (var slot in slots)
        {
            foreach (Transform child in slot.transform)
            {
                Destroy(child.gameObject);
            }
        }
        this.gameObject.SetActive(false);
    }
}
