using UnityEngine;

public static class ItemManager
{
    public enum ItemType
    {
        Empty,
        Rune,
        Head,
        Skull,
        Potion,
        Plant,
        Egg
    }



    public static Sprite GetSprite(ItemType type)
    {
        foreach (ItemData item in ItemReference.Instance.itemDataArray)
        {
            if (item.type == type)
                return item.sprite;
        }
        Debug.LogError("sprite of " + type + " not found");
        return null;
    }
    public static GameObject GetPrefab(ItemType type)
    {
        foreach (ItemData item in ItemReference.Instance.itemDataArray)
        {
            if (item.type == type)
                return item.prefab;
        }
        Debug.LogError("prefab of " + type + " not found");
        return null;
    }
    public static int GetPrice(ItemType type)
    {
        foreach (ItemData item in ItemReference.Instance.itemDataArray)
        {
            if (item.type == type)
                return item.price;
        }
        Debug.LogError("price of " + type + " not found");
        return 0;
    }
    //public static ItemType GetType(GameObject prefab)
    //{
    //    foreach (ItemData item in ItemReference.Instance.itemDataArray)
    //    {
    //        if (item.prefab == prefab)
    //            return item.type;
    //    }
    //    Debug.LogError("type of " + prefab + " not found");
    //    return 0;
    //}
}







