using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inscryption : Interactee
{
    public override void Interact(Transform player)
    {
        message = "Dear traveller, I require yet another [Potion]. Be quick about it. ";
        tmpText.text = message;
    }

    protected override void SetMessage()
    {
        message = "[E] : Read Inscryption";
    }


}
