using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MovementForces : MonoBehaviour
{
    protected UnitManager unit;
    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        unit = GetComponent<UnitManager>();

        //velocity saturation equation: speed = acceleration / drag - acceleration * fixedDeltaTime 
        //inputDrag = inputAcceleration / (inputMaxSpeed + inputAcceleration * Time.fixedDeltaTime);

        SetInputParameters(groundMaxSpeed, groundAcceleration);
    }
    protected virtual void FixedUpdate()
    {

        SetUnitMovementDirection(); 

   
        ApplyFriction();

        ApplyChangingVelocity();

    }
    protected virtual void Update()
    {
        CheckGround();
    }
    public Vector2 unitMovementDirection { get; protected set; }
    protected virtual void SetUnitMovementDirection()
    {
        unitMovementDirection = unit.DesiredUnitDirection.normalized;

    }


    [Header("Friction")]
    //[SerializeField] public bool isUsingFriction;
    //[field: SerializeField] public float surfaceFriction { get; protected set; } = 80;
    [SerializeField] public float airFriction = 0.86f;

    public float surfaceMultiplier { get; protected set; } = 1;

    protected void ApplyFriction()
    {
        if (hasFullVelocityControl)
            return;

        surfaceMultiplier = 1;

        //if (false)
        //{
        //    Vector3 v = Vector3.ProjectOnPlane(velocity, gw.planeNormal);
        //    float frictionForce = surfaceFriction * surfaceMultiplier * Time.fixedDeltaTime;
        //    if (v.magnitude >= frictionForce)
        //        velocity += -1 * frictionForce * v.normalized;
        //    else
        //        velocity -= v;
        //}
        //else
        //{
        //if (isUsingAirYFriction)
        //    ApplyDrag(airFriction);
        //else
        //{
        //    Vector3 y = Vector3.up * velocity.y;
        //    Vector3 xz = velocity - y;
        //    xz *= (1 - airFriction * Time.fixedDeltaTime);
        //    velocity = xz + y;
        //}
        //}
        ApplyDrag(airFriction);
      
    }
    //-------------------------------------------------------------------------------------------------------------------------------------------------------

    [field: SerializeField] public float groundAcceleration { get; protected set; } = 150;
    [field: SerializeField] public float groundMaxSpeed { get; protected set; } = 10;
    [field: SerializeField] public float waterAcceleration { get; protected set; } = 60;
    [field: SerializeField] public float waterMaxSpeed { get; protected set; } = 6;
    [field: SerializeField] public float inputAcceleration { get; protected set; } = 150;
    [field: SerializeField] public float inputMaxSpeed { get; protected set; } = 10;
    private float inputDrag;


    protected void SetInputParameters(float speed, float acceleration)
    {
        inputMaxSpeed = speed;
        inputAcceleration = acceleration;

        //velocity saturation equation: speed = acceleration / drag - acceleration * fixedDeltaTime 
        inputDrag = inputAcceleration / (inputMaxSpeed + inputAcceleration * Time.fixedDeltaTime);
    }



    public bool isOnGround { get; protected set; }
    private bool hasHitGround;
    private bool hasHitWater;
    private void CheckGround()
    {
        isOnGround = true;
        RaycastHit2D hit = Physics2D.Raycast(unit.GetBodyCenter(), Vector2.down, 0.1f, MaskProcessing.instance.background);
        if (hit.collider != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("water")) isOnGround = false;


        if (isOnGround)
        {
            hasHitWater = false;
            if (!hasHitGround)
            {
                hasHitGround = true;
   
                //Debug.Log("ground hit");
                SetInputParameters(groundMaxSpeed, groundAcceleration);
            }
        }
        else
        {
            hasHitGround = false;
            if (!hasHitWater)
            {
                //Debug.Log("water hit");
                hasHitWater = true;
              
                SetInputParameters(waterMaxSpeed, waterAcceleration);
            }
        }
        
    }














    [field:SerializeField] public bool hasFullVelocityControl { get; protected set; }


    protected Vector2 changingVelocity;


    protected virtual void ApplyChangingVelocity()
    {
        hasFullVelocityControl = rb.velocity.magnitude <= inputMaxSpeed;

        if (hasFullVelocityControl)
        {
            rb.velocity += unitMovementDirection * inputAcceleration * Time.fixedDeltaTime;
            rb.velocity *= (1 - inputDrag * Time.fixedDeltaTime);
        }
        else
            RedirectAndDecreaseVelocity();
        
    }

    protected float redirectionMultiplier = 2;
    protected virtual void RedirectAndDecreaseVelocity()
    {
        Vector2 inputForce = unitMovementDirection * inputMaxSpeed * redirectionMultiplier * Time.fixedDeltaTime;
        Vector2 wouldBeVelocity = rb.velocity + inputForce;

        if ((wouldBeVelocity).magnitude < rb.velocity.magnitude)
            rb.velocity += inputForce;
        else
            rb.velocity =  wouldBeVelocity.normalized * rb.velocity.magnitude;
    }


    //----------------------------------------------------------
    public Action onAddForce;
    public Action OnAddImpulse;

    public void AddForce(Vector2 force)//over time
    {
        if (onAddForce != null)
            onAddForce();
        rb.velocity += force * Time.deltaTime;
    }
    public void AddImpulse(Vector2 impulse)// once
    {
        if (OnAddImpulse != null)
            OnAddImpulse();
        rb.velocity += impulse;
    }
    public void AddFriendImpulse(Vector2 impulse)// once
    {
        rb.velocity += impulse;
    }
    public void NormalizeVelocity(float speed)
    {
        if (rb.velocity.magnitude > speed)
            rb.velocity = rb.velocity.normalized * speed;
    }

    public void Stun()
    {
        if (hasFullVelocityControl)
        {
            rb.velocity -= Vector2.zero;
            changingVelocity = Vector2.zero;
        }
    }

    public void ApplyDrag(float drag)
    {
        rb.velocity *= (1 - drag * Time.fixedDeltaTime);
    }
    public void ApplyYDrag(float drag)
    {
        Vector2 y = Vector3.up * rb.velocity.y;
        Vector2 xz = rb.velocity - y;
        y *= (1 - drag * Time.fixedDeltaTime);
        rb.velocity = xz + y;
    }
    public void ApplyXZDrag(float drag)
    {
        Vector2 y = Vector3.up * rb.velocity.y;
        Vector2 xz = rb.velocity - y;
        xz *= (1 - drag * Time.fixedDeltaTime);
        rb.velocity = xz + y;
    }
}

