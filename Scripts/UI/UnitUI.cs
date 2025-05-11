using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UnitUI : MonoBehaviour
{
    UnitManager unit;
    protected virtual void Awake()
    {
        unit = GetComponent<UnitManager>();

    }
    protected virtual void Start()
    {
        unit.OnAddHealth += ChangeHealth;
    }
    protected virtual void Update()
    {
        ;
    }
    [SerializeField] private Slider healthBar;

    protected virtual void ChangeHealth(object sender, UnitManager.OnResourceChangeArgs e)
    {
        if (healthBar != null) healthBar.value = (float)e.currentResource / e.maxResource;
    }
    
}
