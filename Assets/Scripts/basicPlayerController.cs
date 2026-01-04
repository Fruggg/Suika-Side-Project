using System;
using System.Collections;
using UnityEngine;

public class basicPlayerController : MonoBehaviour
{
    [SerializeField] AnimationCurve dashCurve;
    [SerializeField] int maxDashCharges = 3;
    [SerializeField] float groundTouchDistance = 2;
    [SerializeField] float moveSpeed;
    [SerializeField] float dampingRate;
    [SerializeField] float maxHorizontalSpeed;
    [SerializeField] float maxJumpCooldown = 0.25f;
    [SerializeField] float dashPower;
    [SerializeField] float jumpPower;
    [SerializeField] HeartDisplayManager dashDisplay;
    [SerializeField] Transform groundCheck;
    [SerializeField] GameObject bat;
    [SerializeField] Rigidbody2D rb;
    PlayerControls controls;
    [SerializeField] Transform arrow;

    [SerializeField] LayerMask groundLayerMask;
    private float currentJumpCooldown;
    [SerializeField] private bool canJump = true;
    private int currentDashAmount;
    string dashCoName = nameof(DashCoroutine);

    /**
       * Controls are crazy simple:
       * The left stick will move in any direction, jumping if help sufficiently upward
       * The right stick will aim the bat
       * Both sticks are used, and the triggers (L2, R2) will respectively dash in the direction of the left stick and 
       * charge the swing of the bat; the bat will swing on release.
       * 
      **/

    // Control variables

    Vector2 r3 = Vector2.zero;
    Vector2 l3 = Vector2.zero;
    float l3Float = 0;



    IEnumerator DashCoroutine(Vector2 direction)
    {

        float cachedGravity = rb.gravityScale;
        //float cachedVel = 0.25f *  Mathf.Min(15f, rb.linearVelocity.magnitude);
        float slices = 35;
        float duration = 0.15f;
        for (int i = 0; i < slices; i++)
        {
        rb.gravityScale = 0;
        float adjust = 1 / slices;
        rb.linearVelocity =  direction * dashPower * ( 0.25f + dashCurve.Evaluate(i * adjust));
        yield return new WaitForSeconds(duration/slices);

        }
        rb.gravityScale = cachedGravity;
        yield return null;
    }
    private void Dash()
    {
        bool canDash = (currentDashAmount != 0);
        if (canDash)
        {
            Vector2 normAim = r3.normalized;

            //First dash version
            //rb.AddForce(normAim * dashPower);

            //second dash version
            StopCoroutine(dashCoName);
            StartCoroutine(dashCoName, normAim);
            
            Debug.Log("Dashed!");
            --currentDashAmount;
            dashDisplay.SetCount(currentDashAmount);
        }
    }
    
    private void MoveL3()
    {

    }

    private void MoveL2()
    {

    }
    private void MoveR3()
    {

    }
  
    private void MoveR2()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //controls.Player.Move.performed += ctx => l3Float = ctx.ReadValue<float>();

        controls = new PlayerControls();
        controls.Enable();

        controls.Player.Move.performed += ctx => l3 = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => l3 = Vector2.zero;

        controls.Player.Aim.performed += ctx => r3 = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => r3 = Vector2.zero;


        controls.Player.Dash.canceled += ctx => Dash();

    }
    private bool IsTouchingGround()
    {

        return Physics2D.OverlapCircle(groundCheck.position, groundTouchDistance, groundLayerMask);

    }
    private void Update()
    {   rb.position += Vector2.up * 2E-3f;
        if(rb.linearVelocityX/l3.x < maxHorizontalSpeed){ rb.linearVelocityX += (Mathf.RoundToInt(l3.x) * moveSpeed); }
        // If we aren't moving or moving in the wrong direction
        if ((Mathf.RoundToInt(l3.x) == 0) || (rb.linearVelocityX / l3.x) < 0) 
        { 
            rb.linearVelocityX -=  Mathf.Sign(rb.linearVelocityX) * dampingRate * Time.deltaTime ; 
        }
        
      

        if(IsTouchingGround())
        {
            currentJumpCooldown -= Time.deltaTime;
            currentDashAmount = maxDashCharges;
            canJump = true;
            dashDisplay.SetCount(currentDashAmount);
            if (currentJumpCooldown <= 0 && canJump)
            {
                if (Mathf.RoundToInt(l3.y) > 0)
                {
                    rb.linearVelocityY = jumpPower;
                    currentJumpCooldown = maxJumpCooldown;
                    canJump = false;
                }
            } }

       
        // Show aiming
        float angle =  180/3.14f * Mathf.Atan2(r3.y, r3.x);
        Debug.Log($"angle : {angle}");
        
        //bat.GetComponentInChildren<SpriteRenderer>().flipY = (Mathf.Abs(angle )>= 90);


        bat.transform.SetLocalPositionAndRotation(r3.normalized * 0.25f, Quaternion.Euler(0, 0, angle));
        arrow.transform.SetLocalPositionAndRotation(r3.normalized * 0.25f, Quaternion.Euler(0, 0, angle - 90));
        

    }

    internal void RestoreDash()
    {
      if (currentDashAmount < maxDashCharges )
        {
            ++currentDashAmount;
            dashDisplay.SetCount(currentDashAmount);
            
        }
    }
}
