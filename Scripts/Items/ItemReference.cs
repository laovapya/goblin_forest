using UnityEngine;

public class ItemReference : MonoBehaviour
{
    public static ItemReference Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] public ItemData[] itemDataArray;

}

[System.Serializable]
public class ItemData
{
    [field: SerializeField] public ItemManager.ItemType type { get; private set; }
    [field: SerializeField] public GameObject prefab { get; private set; }
    [field: SerializeField] public Sprite sprite { get; private set; }

    [field: SerializeField] public int price { get; private set; }
}