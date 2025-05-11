using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Interacter : MonoBehaviour
{
    [SerializeField] private float radius = 3;
    
    private void Update()
    {
        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, radius, MaskProcessing.instance.interactable);
        //Debug.Log("length " + interactables.Length);
      
        if (interactables.Length > 0 && interactables[0].TryGetComponent(out Interactee interactee))
        {

            interactee.DisplayMessage();
   
            if (Input.GetKeyDown(KeyCode.E))
                interactee.Interact(transform);
        }
    }
}

