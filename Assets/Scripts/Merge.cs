using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Merge : MonoBehaviour
{
    
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
    private Transform Transform;
    
    private SpriteRenderer sr;
    public Rigidbody2D rb;

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = ballSprites[(int)ball];
        Transform = GetComponent<Transform>();
        transform.localScale = Vector3.one;
    }


    public void Update()
    {
        var ballIndex = (int)ball;
        float scale = ballIndex != 0 ? 1.5f * ballIndex : 1;
        transform.localScale = scale * new Vector3(1, 1, 1);
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
        BallType type = (BallType)(stage);
        this.ball = type;
        float scale = 1.5f * stage;
        transform.localScale = scale * new Vector3(1, 1, 1);
        //todo unComment when we actually have sprites
        //sr.sprite = ballSprites[stage];
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {

            var ball1 = this.ball;
            var otherMerge = collision.gameObject.GetComponent<Merge>();
            var ball2 = otherMerge.ball;
            //Debug.Log(ball1 + " is touching " + ball2);
            if (ball2 == ball1)
            {
                //UpgradeBall();

                SetBallStage((int)ball + 1);
                
                // Also increment the score: 
                OnMerge?.Invoke((int)ball);
                
                // Conservation of momentum
                // v = (mv + mv)/m
                float newMass = rb.mass + otherMerge.rb.mass;
                rb.linearVelocity = (1/newMass) * rb.linearVelocity * rb.mass + otherMerge.rb.linearVelocity * otherMerge.rb.mass;
                rb.mass = newMass;
                
                Destroy(collision.gameObject);
            }
        }
    }
}
