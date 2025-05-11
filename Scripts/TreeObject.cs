using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private float pushForce = 15;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (MaskProcessing.instance.MaskContainsLayer(other.gameObject.layer, MaskProcessing.instance.unit))
        {
            other.GetComponent<MovementForces>().AddImpulse((other.transform.position - transform.position).normalized * pushForce);
    
        }
           
    }
}
