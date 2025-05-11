using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerUI : UnitUI
{
    public static PlayerUI instance;
    private PlayerUnit player;// = PlayerUnit.instance;
    protected override void Awake()
    {
        base.Awake();
        instance = this;
        //Cursor.lockState = CursorLockMode.None;

    }
    protected override void Start()
    {
        base.Start();
        player = PlayerUnit.instance;
        PlayerUnit.instance.onHealthChange += ChangeHealth;
        PlayerUnit.instance.onGoldChange += ChangeGold;
        //ChangeHealth(PlayerUnit.instance.maxHealth); //init hearts 
    }
    protected override void Update()
    {
        base.Update();
        CloseShop();
        if (Input.GetKeyDown(KeyCode.Escape))
            StartCoroutine(ToMainMenu(0));
    }

    //public bool isInventoryToggled { private set; get; } = false;
    [SerializeField] private Transform inventory;
    //private void ToggleInventory()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //        if (isInventoryToggled)
    //        {
    //            isInventoryToggled = false;
    //            inventory.gameObject.SetActive(false);

    //            ItemSlot.StopCarryingItem();
    //        }
    //        else
    //        {
    //            isInventoryToggled = true;
    //            inventory.gameObject.SetActive(true);

    //        }
    //}


    [SerializeField] private TextMeshProUGUI goldText;
    public void ChangeGold(int gold)
    {
        if (goldText != null) goldText.text = gold.ToString();
    }
    [SerializeField] private Transform health;
    private float healthOffset = 100;
    private void ChangeHealth(int h)
    {
        Debug.Log("fired change health");
        foreach(Transform child in health)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < h; ++i)
        {
            RectTransform heart = Instantiate(PrefabReference.instance.heart, health).GetComponent<RectTransform>();
            heart.localPosition = new Vector2(healthOffset * i, 0);
            
        }
    }


    [SerializeField] private Transform fireballSlot;
    public void BuyFireball()
    {
        if (PlayerUnit.instance.gold < 20)
        {
            Debug.Log("not enough gold ");
            return;
        }
        PlayerUnit.instance.AddGold(-20);
        PlayerUnit.instance.EnableFireball();

        Destroy(fireballSlot.gameObject);

    }
    [SerializeField] private Transform missileSlot;


    public void BuyMissile()
    {
        if (PlayerUnit.instance.gold < 140)
        {
            Debug.Log("not enough gold ");
            return;
        }
        player.AddGold(-140);
        player.EnableMissile();

        Destroy(missileSlot.gameObject);

    }

    public void Sell()
    {
        Inventory inventory = player.inventory;
        player.AddGold(ItemManager.GetPrice(inventory.GetItemList()[0]));
        inventory.RemoveItem(0);
    }
    [SerializeField] private Transform shopUI;
    public bool isInShop = false;
    private void CloseShop()
    {
        if (Input.GetKeyDown(KeyCode.I)) //&& shopUI.gameObject.activeInHierarchy
        {
            shopUI.gameObject.SetActive(false);
            isInShop = false;


            Inventory inventory = player.inventory;
            ItemManager.ItemType type = inventory.GetItemList()[0];
            inventory.RemoveItem(0);
            inventory.AddItem(type);
        }
    }
    public void OpenShop()
    {
        shopUI.gameObject.SetActive(true);
        isInShop = true;
    }

    private IEnumerator ToMainMenu(float waitTime)
    {
        
        yield return new WaitForSeconds( waitTime);
        SceneManager.LoadScene(0);
        //defeat.gameObject.SetActive(false);
        //victory.gameObject.SetActive(false);
        
       
    }
    [SerializeField] private Transform victory;
    [SerializeField] private Transform defeat;
    [SerializeField] private float resetWaitTime = 2;
    public void ShowVictory()
    {
        victory.gameObject.SetActive(true);
        StartCoroutine(ToMainMenu(resetWaitTime));
    }
    public void ShowDefeat()
    {
        defeat.gameObject.SetActive(true);
        StartCoroutine(ToMainMenu(resetWaitTime));
    }
}



