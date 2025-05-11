using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerIn : Interactee
{
    [SerializeField] private GameObject shop;

    public override void Interact(Transform player)
    {
        player.position = shop.transform.position;
    }

    protected override void SetMessage()
    {
        message = "[E] : Enter";
    }
}
