using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Inventory
{
    protected UnitManager owner;

    public int numberOfActiveSlots = 1;
    protected int capacity = 20;

    protected List<ItemManager.ItemType> items = new List<ItemManager.ItemType>();
    public Inventory(UnitManager owner, int capacity)
    {
        this.owner = owner;

        this.capacity = capacity;
        //ebug.Log("capacity " + capacity);

        for (int i = 0; i < capacity + numberOfActiveSlots; ++i)
            items.Add(ItemManager.ItemType.Empty);
    }
    public Action onInventoryUpdate;

    public bool AddItem(ItemManager.ItemType item)
    {
        for (int i = numberOfActiveSlots; i < capacity + numberOfActiveSlots; ++i)
            if (items[i] == ItemManager.ItemType.Empty)
            {
                items[i] = item;
                if (onInventoryUpdate != null)
                    onInventoryUpdate();
                return true;
            }

        return false;
    }
    public void RemoveItem(int index)
    {
        if (index > items.Count)
        {
            Debug.Log("error: inventory index out of bound");
            return;
        }
        items[index] = ItemManager.ItemType.Empty;

        if (onInventoryUpdate != null)
            onInventoryUpdate();
    }
    public List<ItemManager.ItemType> GetItemList()
    {
        return items;
    }
}

