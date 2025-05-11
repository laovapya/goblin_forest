using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerDownHandler
{
    public int itemIndex;
    public Vector2 originalPos { private set; get; }

    public CanvasGroup canvasGroup;
    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        originalPos = GetComponent<RectTransform>().anchoredPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)  //clicked with left mouse button
        {
            
            List<ItemManager.ItemType> playerItems = PlayerUnit.instance.inventory.GetItemList();

            ItemSlot carriedItem = InventoryUI.instance.carriedItem;
            if (carriedItem == null)
            {
                if (playerItems[itemIndex] == ItemManager.ItemType.Empty)
                {
                    Debug.Log("empty item");
                    return;
                }

                InventoryUI.instance.carriedItem = this;
                canvasGroup.blocksRaycasts = false;
                //transform.SetAsLastSibling();  !!!!!!!!!!!!!!!!!!!!!!!!!!!!

            }
            else
            {
                ItemManager.ItemType temp = playerItems[itemIndex];
                playerItems[itemIndex] = playerItems[carriedItem.itemIndex];
                playerItems[carriedItem.itemIndex] = temp;

                InventoryUI.instance.RefreshInventory();
                //PlayerUnit.instance.inventory.ReequipItems();

                StopCarryingItem();


            }
        }
      
    }

    public static void StopCarryingItem()
    {
        ItemSlot carriedItem = InventoryUI.instance.carriedItem;
        if (carriedItem == null)
            return;

        carriedItem.canvasGroup.blocksRaycasts = true;
        carriedItem.GetComponent<RectTransform>().anchoredPosition = carriedItem.originalPos;
        InventoryUI.instance.carriedItem = null;
    }

}
