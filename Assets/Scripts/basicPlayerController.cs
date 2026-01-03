using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class basicPlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float dampingRate;
    [SerializeField] float maxHorizontalSpeed;

    [SerializeField] float maxJumpCooldown = 0.25f;
    [SerializeField] float dashPower;
    [SerializeField] float jumpPower;
    [SerializeField] string groundtag;
    [SerializeField] GameObject bat;
    [SerializeField] Rigidbody2D rb;
    PlayerControls controls;
    [SerializeField] Transform arrow;

    private float currentJumpCooldown;

    /**
       * Controls are crazy simple:
       * The left stick will move in any direction, jumping if help sufficiently upward
       * The right stick will aim the bat
       * Both sticks are used, and the triggers (L2, R2) will respectively dash in the direction of the left stick and 
       * charge the swing of the bat; the bat will swing on release.
       * 
      **/

    // Control variables
    [SerializeField] float groundTouchDistance = 2;
    Vector2 r3 = Vector2.zero;
    Vector2 l3 = Vector2.zero;
    float l3Float = 0;

    [SerializeField] int maxDashCharges = 3;

    private void Dash()

    {
        bool canDash = true;
        if (canDash)
        {
            Vector2 normAim = r3.normalized;
            rb.AddForce(normAim * dashPower);
            Debug.Log("Dashed!");
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
        return Physics2D.Raycast(transform.position, Vector2.down, groundTouchDistance);
    }
    private void Update()
    {
        // Movement
        rb.position += Vector2.up * 2E-3f;
        if(rb.linearVelocityX/l3.x < maxHorizontalSpeed){ rb.linearVelocityX += (Mathf.RoundToInt(l3.x) * moveSpeed); }
        // If we aren't moving or moving in the wrong direction
        if ((Mathf.RoundToInt(l3.x) == 0) || (rb.linearVelocityX / l3.x) < 0) 
        { 
            rb.linearVelocityX -=  Mathf.Sign(rb.linearVelocityX) * dampingRate * Time.deltaTime ; 
        }
        rb.linearVelocityY -= 9.81f * Time.deltaTime;
        currentJumpCooldown -= Time.deltaTime;

        if(IsTouchingGround() && currentJumpCooldown <= 0)
        {
            Debug.Log("touching ground");
            if (Mathf.RoundToInt(l3.y) > 0) 
            { 
                rb.linearVelocityY = jumpPower;
                currentJumpCooldown = maxJumpCooldown;
            }
        }

       
        // Show aiming
        float angle =  180/3.14f * Mathf.Atan2(r3.y, r3.x);
        Debug.Log($"angle : {angle}");
        
        //bat.GetComponentInChildren<SpriteRenderer>().flipY = (Mathf.Abs(angle )>= 90);


        bat.transform.SetLocalPositionAndRotation(r3.normalized * 0.25f, Quaternion.Euler(0, 0, angle));
        

    }
}
