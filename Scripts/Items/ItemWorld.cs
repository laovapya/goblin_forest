using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{


    private float angle;
    [SerializeField] private float wobbleRange = 0.5f;
    [SerializeField] private float wobbleSpeed = 1;
    private Vector2 wobble;
    private Vector2 initialPos;

    [SerializeField] private ItemManager.ItemType type;
    private void Start()
    {
        initialPos = transform.position;
    }
    void Update()
    {
        wobble = Vector2.up * wobbleRange * Mathf.Sin(wobbleSpeed * (angle += Time.deltaTime));
        transform.position = initialPos + wobble;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("player"))
        {
            if (PlayerUnit.instance.inventory.AddItem(type))
                Destroy(gameObject);
        }
           
        
    }


    public static GameObject Spawn(ItemManager.ItemType type, Vector2 pos, Transform parent)
    {
        GameObject item = Instantiate(ItemManager.GetPrefab(type), parent);
        item.transform.position = pos;
        return item;
    }
}
