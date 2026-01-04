using System.Collections;
using UnityEngine;

public class LeekHitbox : MonoBehaviour
{
    [SerializeField] float strikeForce = 200f;
    [SerializeField] float bathitboxDuration;
    [SerializeField] BoxCollider2D bathitbox;
    [SerializeField] Animator batAnimation;


    bool batCharging = false;
    [SerializeField] private float currentBatCharge;
    PlayerControls controls;
    [SerializeField] Rigidbody2D player;
    [SerializeField] basicPlayerController playerController;

    [SerializeField] Animator flashAnimator;

    private IEnumerator BatHitboxManager()
    {
        yield return new WaitForSeconds(0.05f);
        bathitbox.enabled = true;
        yield return new WaitForSeconds(bathitboxDuration);
        bathitbox.enabled = false;
        currentBatCharge = 0;
        yield return null;
    }
    private void SetBatCharging()
    {
        batCharging = true;
        batAnimation.SetBool("Charging", true);
    }


    private void SwingBat()
    {
        batAnimation.SetBool("Charging", false);
        StartCoroutine(nameof(BatHitboxManager));
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Leekin my shit!!! balls");
            Vector2 direction = new Vector2(transform.up.y, -transform.up.x);
            float chargeMultiplier = Mathf.Min(currentBatCharge, 1);
            float speedMult = 0.01f *  player.linearVelocity.sqrMagnitude; 
            collision.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(chargeMultiplier * direction * strikeForce * ( 1 + speedMult));
            playerController.RestoreDash();
        }
    }
    private void Update()
    {

        if (batCharging) { currentBatCharge += Time.deltaTime; }

        // Bat flash
        if (currentBatCharge >= 0.99f && currentBatCharge <= 1.01f) { flashAnimator.SetTrigger("Flash"); }

    }
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();

        controls.Player.ChargeBat.performed += ctx => { SetBatCharging(); };
        controls.Player.ChargeBat.canceled += ctx => { batCharging = false; SwingBat(); };
    }
}
