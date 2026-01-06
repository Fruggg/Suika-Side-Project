using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using static AnimationCurveExtensions;



public class basicPlayerController : MonoBehaviour
{
    [Header("Ganeral Movement")]
    [SerializeField] float jumpPower;
    [SerializeField] float groundTouchDistance = 2;
    [SerializeField] float moveSpeed;
    [SerializeField] float dampingRate;
    [SerializeField] float maxHorizontalSpeed;
    [SerializeField] float maxJumpCooldown = 0.25f;
    [Header("Dash")]
    [SerializeField] AnimationCurve dashCurve;
    [SerializeField] int maxDashCharges = 3;
    [SerializeField] float dashPower;
    [SerializeField] float dashDuration;
    [SerializeField] float dashDistance = -1;
    [SerializeField] int dashSlices = 35;
    [Header("Other Objects")]
    [SerializeField] HeartDisplayManager dashDisplay;
    [SerializeField] Transform groundCheck;
    [SerializeField] GameObject bat;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform arrow;
    [SerializeField] LayerMask groundLayerMask;

    TimeStopper timeMan;
    PlayerControls controls;
    private float currentJumpCooldown;
    private int currentDashAmount;
    string dashCoName = nameof(DashCoroutine);
    private bool canJump = true;


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
        // Integrate velocity ; 1/2 velocity ^2 times t is distance
        
        

        float cachedGravity = rb.gravityScale;
        float cachedVel = 0.25f * Mathf.Min(15f, rb.linearVelocity.magnitude);
        for (int i = 0; i < dashSlices; i++)
        {
            rb.gravityScale = 0;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Debug.Log($"{((float)i / dashSlices)} ");
            float velMag = (1 / Time.timeScale) * dashPower * dashCurve.Evaluate(((float)i / dashSlices));
            rb.linearVelocity = velMag * direction;
            yield return new WaitForSecondsRealtime(dashDuration / (float)dashSlices);
        }
        rb.gravityScale = cachedGravity;
        rb.linearVelocity = Vector2.zero;
        
        yield return null;
    }
    private void Dash()
    {
        bool canDash = (currentDashAmount != 0);
        if (canDash)
        {
            Vector2 normAim = r3.normalized;
            
            // Bullet Time
            timeMan.RequestTimeScale((float)((0.35f)), dashDuration + 0.1f);

            StopCoroutine(dashCoName);
            StartCoroutine(dashCoName, normAim);
            
            Debug.Log("Dashed!");
            --currentDashAmount;
            dashDisplay.SetCount(currentDashAmount);
        }
    }
    private bool IsTouchingGround()
    {

        return Physics2D.OverlapCircle(groundCheck.position, groundTouchDistance, groundLayerMask);

    }
    private void Update()
    {   
        rb.position += Vector2.up * 2E-3f;
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
        //Debug.Log($"angle : {angle}");
        
        //bat.GetComponentInChildren<SpriteRenderer>().flipY = (Mathf.Abs(angle )>= 90);


        bat.transform.SetLocalPositionAndRotation(r3.normalized * 0.25f, Quaternion.Euler(0, 0, angle));
        arrow.transform.SetLocalPositionAndRotation(r3.normalized * dashDistance * 0.9f, Quaternion.Euler(0, 0, angle - 90));
        

    }

    internal void RestoreDash()
    {
        if (currentDashAmount < maxDashCharges)
        {
            ++currentDashAmount;
            dashDisplay.SetCount(currentDashAmount);

        }
    }
    private void Start()
    {
        timeMan = FindAnyObjectByType<TimeStopper>();
    }
    void Awake()
    {
        #region Controls
        controls = new PlayerControls();
        controls.Enable();

        controls.Player.Move.performed += ctx => l3 = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => l3 = Vector2.zero;

        controls.Player.Aim.performed += ctx => r3 = ctx.ReadValue<Vector2>();
        controls.Player.Aim.canceled += ctx => r3 = Vector2.zero;


        controls.Player.Dash.canceled += ctx => Dash();
        #endregion Controls

       
    }
    private void OnValidate()
    {
        // daShDuration * to turn the bound into a proper integral (Same cuerve but squished by an amount (assuming thet dashduration < 1))
        dashDistance =  dashDuration * dashPower*( AnimationCurveExtensions.Integrate(dashCurve, sliceCount:35));
        arrow.transform.SetLocalPositionAndRotation(Vector2.up * dashDistance * 0.9f, Quaternion.identity);

    }
}
