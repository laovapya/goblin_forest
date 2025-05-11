using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance { get; private set; }
    public Transform itemContainer { private set; get; }
    public Transform activeItems { private set; get; }
    private Inventory inventory;

    private void Awake()
    {
        instance = this;
        itemContainer = transform.Find("item container");
        activeItems = transform.Find("active items");
        itemTemplate = PrefabReference.instance.itemSlot;
        if (itemContainer == null)
            Debug.Log("item container is not found");
        if (activeItems == null)
            Debug.Log("active item is not found");

        SetMeasurements();

        inventory = PlayerUnit.instance.SetInventory(GetCapacity(), RefreshInventory);

    }
    //private void Start()
    //{
    //    PlayerInventory.instance.onInventoryUpdate += RefreshInventory;
    //}
    private void Update()
    {
        MoveCarriedItem();
    }
    //public void SetInventory() //called from Player after inventory is set up 
    //{
    //    //playerInventory = PrefabsReference.Instance.player.GetComponent<UnitManager>().inventory;
    //    PlayerInventory.instance.onInventoryUpdate += RefreshInventory;
    //    RefreshInventory();
    //}
    Transform itemTemplate;
    private float slotWidth;
    private float slotHeight;
    private int cols;
    private int rows;
    private void SetMeasurements()
    {
        float boxWidth = itemContainer.GetComponent<RectTransform>().sizeDelta.x;
        float boxHeight = itemContainer.GetComponent<RectTransform>().sizeDelta.y;

        slotWidth = itemTemplate.GetComponent<RectTransform>().sizeDelta.x;
        slotHeight = itemTemplate.GetComponent<RectTransform>().sizeDelta.y;

        cols = Mathf.FloorToInt(boxWidth / slotWidth);
        rows = Mathf.FloorToInt(boxHeight / slotHeight);
    }
    public void RefreshInventory()
    {
        for (int i = 0; i < activeItems.childCount; ++i)
        {
            Transform item = activeItems.GetChild(i);
            Image image = item.GetComponent<Image>();
            image.sprite = ItemManager.GetSprite(inventory.GetItemList()[i]);

            //if (inventory.GetItemList()[i] == ItemManager.ItemType.Empty)
            //    MakeTransparent(image, true);
            //else
            //    MakeTransparent(image, false);

        }


        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < rows; ++i)
            for (int j = 0; j < cols; ++j)
            {
                Transform item = Instantiate(itemTemplate, itemContainer);
                item.GetComponent<RectTransform>().anchoredPosition = new Vector2(slotWidth / 2, -slotHeight / 2) + new Vector2(slotWidth * j, slotHeight * -i);
                int index = i * cols + j + inventory.numberOfActiveSlots;

                item.GetComponent<ItemSlot>().itemIndex = index;

                //if (index < playerInventory.GetItemList().Count)
                Image image = item.GetComponent<Image>();
                image.sprite = ItemManager.GetSprite(inventory.GetItemList()[index]);

                //if (inventory.GetItemList()[index] == ItemManager.ItemType.Empty)
                //    MakeTransparent(image, true);
            }
    }
    public int GetCapacity()
    {
        return cols * rows;
    }



    public ItemSlot carriedItem;
    private void MoveCarriedItem()
    {
        if (carriedItem != null)
            carriedItem.transform.position = Input.mousePosition;
    }

    //private void MakeTransparent(Image image, bool isTransparent)
    //{
    //    Color c = image.color;
    //    if (isTransparent)
    //        image.color = new Color(c.r, c.g, c.b, 0);
    //    else
    //        image.color = new Color(c.r, c.g, c.b, 1);
    //}
}
