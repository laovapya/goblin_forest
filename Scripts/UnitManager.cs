using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UnitManager : MonoBehaviour
{
    public Action onUpdate;
    public MovementForces mf { private set; get; }
 
    static public void Spawn(Transform prefab, Vector2 position, Quaternion rotation)
    {
        UnitManager unit = Instantiate(prefab, position, rotation).GetComponent<UnitManager>();
        //... 
    }
    protected virtual void Awake()
    {
        //inventory = new Inventory(this);
        mf = GetComponent<MovementForces>();
    
    }
    protected virtual void Start()
    {


        AddHealth(maxHealth);


        //healthRegenInterval = 1 / healthRegenRate;
    

    }
    protected virtual void Update()
    {
        if (onUpdate != null)
            onUpdate();

        //Regen();

        //SetActiveItem();
        //TryItem();
        //CheckTriedItems();

        SetUnitDesiredDirection();
    }

    [field: SerializeField] public Transform itemHolder { protected set; get; }
    public Vector2 GetBodyCenter()
    {
        return new Vector2(transform.position.x, transform.position.y + 0.5f);
    }                    
    public virtual Vector2 GetEyePosition()
    {                    
        return new Vector2(transform.position.x, transform.position.y + 1);
    }
    [field: SerializeField] public int health { protected set; get; } = 0;
    [field: SerializeField] public int maxHealth { get; protected set; } = 40;
    //[field: SerializeField] public float healthRegenRate { get; protected set; } = 10;
    //private float nextHealthTime = 0;
    //private float healthRegenInterval;
    

    public float itemCooldown { private set; get; } = 0;
    public float currentMaxCooldown { private set; get; } = 1; //for ui, dont set to zero, avoid division by 0
    public void SetItemCooldown(float cooldown)
    {
        itemCooldown = cooldown;
        currentMaxCooldown = cooldown;
    }

    public class OnResourceChangeArgs
    {
        public int currentResource;
        public int maxResource;
    }

    public event EventHandler<OnResourceChangeArgs> OnAddHealth;

    public Action<int> onHealthChange;
    public void AddHealth(int amount)
    {
        if (health + amount <= 0)
            Die();


        health = Mathf.Clamp(health + amount, 0, maxHealth);
        onHealthChange?.Invoke(health);
        //Debug.Log("health " + health);
        //if (OnAddHealth != null) OnAddHealth(this, new OnResourceChangeArgs { currentResource = health, maxResource = maxHealth });
    }

    //private void Regen()
    //{
    //    if (Time.time > nextHealthTime)
    //    {
    //        nextHealthTime = Time.time + healthRegenInterval;
    //        AddHealth(1);
    //    }

    //    itemCooldown -= Time.deltaTime;
    //}

    protected virtual void Die()
    {
        Destroy(gameObject);
        Debug.Log(transform + " died");
    }

    public virtual Vector2 GetFacingDirection()
    {
        return Vector2.zero;
    }

    public Inventory inventory { get; protected set; }

    [SerializeField] protected bool isTryingItem;
    //private bool hasTriedItem;
    //private bool isItemDown; //must be true on first frame
    //private bool isItemUp;
    protected int activeSlot = 0;
    //public ItemWorld activeItem;
    //protected virtual void TryItem() { isTryingItem = false; }
    //protected virtual void SetActiveItem()
    //{
    //    activeSlot = 0;
    //    if (inventory.GetItemList().Count > 0)
    //        activeItem = inventory.itemPrefabs[activeSlot];
    //}



    [field:SerializeField]public Vector2 DesiredUnitDirection { get; protected set; }
    protected abstract void SetUnitDesiredDirection();
    public bool GetIsTryingToMove() //direction has to be set to zero for unit not to move
    {
        return DesiredUnitDirection != Vector2.zero;
    }
}
