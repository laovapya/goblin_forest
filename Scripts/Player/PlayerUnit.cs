using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUnit : UnitManager
{
    public static PlayerUnit instance;
    //[field: SerializeField] public PlayerCamera cam { private set; get; }


    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
    public Inventory SetInventory(int capacity, Action RefreshInventory) //called from iventoryUI, inits PlayerInventory, PlayerInventory inits inventory. 
    {
        inventory = new Inventory(this, capacity);
        inventory.onInventoryUpdate += RefreshInventory;
        return inventory;
    }
    protected override void Start()
    {
        base.Start();
        
    }
    
    

    protected override void Update()
    {
        base.Update();

        //Pause();
        //Exit();
        if (PlayerUI.instance.isInShop)
            return;
        Fireball();
        Missile();
    }

   
   

    //private void Pause()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        Debug.Break();
    //    }
    //}
    //private void Exit()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        UnityEditor.EditorApplication.isPlaying = false;
    //    }
    //}

    public override Vector2 GetFacingDirection()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        return (mousePos - GetBodyCenter()).normalized; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }


    //protected override void SetActiveItem() //doesnt know which is the active item --------------------------------------------------------------------------------------------------------------
    //{
    //    if (Input.GetKeyDown(KeyCode.LeftShift)) activeSlot = 0;
    //    else if (Input.GetKeyDown(KeyCode.E)) activeSlot = 1;
    //    else if (Input.GetKeyDown(KeyCode.Q)) activeSlot = 2;
    //    //if (inventory.GetItemList().Count > 0)
    //    //    activeItem = inventory.itemPrefabs[activeSlot];
    //}
    //protected override void TryItem() { isTryingItem = Input.GetKey(ItemKey) || Input.GetKeyDown(ItemKey); }

    protected override void SetUnitDesiredDirection()
    {
        DesiredUnitDirection = Vector2.zero;
        if (PlayerUI.instance.isInShop)
            return;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 wasdVector = (transform.right * x + transform.up * y).normalized;
        DesiredUnitDirection = wasdVector;
    }





    //public PathFinding.PlayerNodeGrid grid;


    protected override void Die()
    {
        // health = maxHealth;
        PlayerUI.instance.ShowDefeat();
    }



    public int gold { get; protected set; }
    public Action<int> onGoldChange;
    public void AddGold(int gold)
    {
        this.gold += gold;
        if (this.gold < 0) this.gold = 0;
        onGoldChange?.Invoke(this.gold);
       
    }
    [SerializeField] private Transform fireball;
    [SerializeField] private float fireballCooldown = 1;
    private bool isFireballReady = true;
    private bool isFireballEnabled = false;
    private void Fireball()
    {
        if (isFireballEnabled && Input.GetKeyDown(KeyCode.Mouse0) && isFireballReady)
        {
         
            
            isFireballReady = false;
            StartCoroutine(timer());

            Vector2 dir = GetFacingDirection();
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
          
            Fireball fireball = Instantiate(this.fireball, GetBodyCenter() + dir, Quaternion.Euler(0f, 0f, angle + 90)).GetComponent<Fireball>();
            fireball.direction = dir;
     
        }
        IEnumerator timer()
        {
            yield return new WaitForSeconds(fireballCooldown);
            isFireballReady = true;
        }
    }
    public void EnableFireball()
    {
        isFireballEnabled = true;
    }





    [SerializeField] private Transform missile;
    [SerializeField] private float missileCooldown = 1;
    private bool isMissileReady = true;
    private bool isMissileEnabled = false;
    private void Missile()
    {
        if (isMissileEnabled && Input.GetKeyDown(KeyCode.Space) && isMissileReady)
        {
            isMissileReady = false;
            StartCoroutine(timer());

            Vector2 dir = GetFacingDirection();
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Missile missile = Instantiate(this.missile, GetBodyCenter() + dir, Quaternion.Euler(0f, 0f, angle + 90)).GetComponent<Missile>();
            missile.direction = dir;

        }
        IEnumerator timer()
        {
            yield return new WaitForSeconds(missileCooldown);
            isMissileReady = true;
        }
    }
    public void EnableMissile()
    {
        isMissileEnabled = true;
    }
}
