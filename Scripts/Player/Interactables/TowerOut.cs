using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerOut : Interactee
{
    [SerializeField] private GameObject tower;

    public override void Interact(Transform player)
    {
        player.position = tower.transform.position;
    }

    protected override void SetMessage()
    {
        message = "[E] : Exit";
    }
}