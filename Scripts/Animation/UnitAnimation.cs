using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    Animator animator;
    UnitManager unit;
    void Start()
    {
        animator = GetComponent<Animator>();
        unit = GetComponent<UnitManager>();
    }
    private string walkState = "isWalking";

    void Update()
    {
        if (unit.GetIsTryingToMove())
        {
            animator.SetBool(walkState, true);
        }
        else
        {
            animator.SetBool(walkState, false);
        }

    }

}
