using System.Collections;
using UnityEngine;

public class LeekHitbox : MonoBehaviour
{
    [SerializeField] float strikeForce = 200f;
    [SerializeField] float bathitboxDuration;
    [SerializeField] BoxCollider2D bathitbox;
    [SerializeField] Animator batAnimation;
    [SerializeField] float maxBat = 1.5f;
    [SerializeField] float homerunReq;
    [SerializeField] GameObject HomeRunSplash;
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

    private void CallStopTime()
    {
        FindFirstObjectByType<TimeStopper>().StopTime(0.15f);
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
            Vector2 direction = new Vector2(transform.up.y, -transform.up.x);
            float chargeMultiplier = Mathf.Min(currentBatCharge, maxBat);
            float speedMult = 0.005f *  player.linearVelocity.sqrMagnitude;
            float force = chargeMultiplier * strikeForce * (1 + speedMult);
            bool homeRun = force >= homerunReq;
            if (homeRun)
            {
                //sfx.play(homerun)
                GameObject splash = HomeRunSplash;
                var instantiatedSplash = Instantiate(splash, collision.attachedRigidbody.position, Quaternion.identity);
                CallStopTime();
            }
            collision.gameObject.GetComponentInChildren<Rigidbody2D>().AddForce(direction * force);
            playerController.RestoreDash();
        }
    }
    private void Update()
    {

        if (batCharging) { currentBatCharge += Time.deltaTime; }

        // Bat flash
        if (currentBatCharge >= maxBat - 0.01f && currentBatCharge <= maxBat + 0.01f) { flashAnimator.SetTrigger("Flash"); }

    }
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();

        controls.Player.ChargeBat.performed += ctx => { SetBatCharging(); };
        controls.Player.ChargeBat.canceled += ctx => { batCharging = false; SwingBat(); };
    }
}
