using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Merge : MonoBehaviour
{

   
    [SerializeField] float speedReq;
    [SerializeField] float defaultScale = 1.5f;
    [SerializeField] int killStage;
    [SerializeField] GameObject deathExplosion;
    private float timerTillDeath;
    [SerializeField] private float deathTime = 10f;
    public enum BallType
    {
        Wheatley,
        Slender,
        Jack,
        Dice,
        Megamind,
        Spamton,
        Nagito,
        Bill,
        Ingo,
        Reigen,
        Sans,
    }
    public BallType ball;
    public List<Sprite> ballSprites;
    // use this event for the scoring UI logic
    public UnityEvent<int> OnMerge = new UnityEvent<int>();
    public UnityEvent<int> OnDeath = new UnityEvent<int>();
    private Transform Transform;
    
    [SerializeField] private SpriteRenderer sr;
    public Rigidbody2D rb;

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = ballSprites[(int)ball];
        Transform = GetComponent<Transform>();
        transform.localScale = Vector3.one * defaultScale;
    }


    public void Update()
    {
        var ballIndex = (int)ball;
        float scale = ballIndex != 0 ? defaultScale * ballIndex : defaultScale;
        transform.localScale = scale * new Vector3(1, 1, 1);
        timerTillDeath += Time.deltaTime;

        if (timerTillDeath >= deathTime)
        {
            if (ballIndex < killStage)
            {
                Destroy(gameObject);
                timerTillDeath = 0f;
                OnDeath?.Invoke((int)ball);
            }
            else
            {
                GameObject splash = deathExplosion;
                var instantiatedSplash = Instantiate(splash, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
    
    //public void UpgradeBall()
    //{
    //    ball++;
    //    float scale = 1.5f * (int)ball;
    //    transform.localScale = scale * new Vector3(1,1,0);
    //    sr.sprite = ballSprites[(int)ball];
    //}


    public void SetBallStage(int stage)
    {
        timerTillDeath = 0f;
        deathTime = (stage * 5) + 5;
        rb.mass = Mathf.Pow (2, stage);
        if(stage == killStage)
        {
            GameObject splash = deathExplosion;
            var instantiatedSplash = Instantiate(splash, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        BallType type = (BallType)(stage);
        this.ball = type;
        float scale = defaultScale * stage;
        transform.localScale = scale * Vector3.one;
        //todo unComment when we actually have sprites
        sr.sprite = ballSprites[stage];
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {

            var ball1 = this.ball;
            var otherMerge = collision.gameObject.GetComponent<Merge>();
            var ball2 = otherMerge.ball;
            //Debug.Log(ball1 + " is touching " + ball2);

            // Additional Requirement to see if the balls are fast enough to merge

            bool fastEnough = (rb.linearVelocity - otherMerge.rb.linearVelocity).sqrMagnitude >= speedReq;

            if (ball2 == ball1 && fastEnough)
            {
                //UpgradeBall();


                float newMass = rb.mass + otherMerge.rb.mass;
                rb.linearVelocity = (1 / newMass) * rb.linearVelocity * rb.mass + otherMerge.rb.linearVelocity * otherMerge.rb.mass;
                rb.mass = newMass;
                SetBallStage((int)ball + 1);
                
                // Also increment the score: 
                OnMerge?.Invoke((int)ball);
                
                // Conservation of momentum
                // v = (mv + mv)/m
                
                Destroy(collision.gameObject);
            }
        }
    }
}
