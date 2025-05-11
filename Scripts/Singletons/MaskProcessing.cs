using System.Collections.Generic;
using UnityEngine;

public class MaskProcessing : MonoBehaviour
{
    public static MaskProcessing instance { get; private set; }


    private void Awake()
    {
        instance = this;


    }
    [field: SerializeField] public LayerMask unit { get; private set; }
    [field: SerializeField] public LayerMask interactable { get; private set; }
    [field: SerializeField] public LayerMask background { get; private set; }
    //[field: SerializeField] public LayerMask ProjectileMask { get; private set; }
    //public int enemyLayer { get; private set; }
    //public int projectileLayer { get; private set; }
    //public int rigidProjectileLayer { get; private set; }

    public static LayerMask CombineMasks(LayerMask m1, LayerMask m2)
    {
        return m1 | m2;
    }
    public static LayerMask SubtractMask(LayerMask m1, LayerMask m2)
    {
        return m1 & ~m2;
    }
    public bool MaskContainsLayer(int layer, int mask)
    {
        return (mask | 1 << layer) == mask;
    }
    public int InvertLayerMask(int mask)
    {
        return ~mask & ((1 << 32) - 1); // Invert mask and clamp to 32 bits
    }

}

