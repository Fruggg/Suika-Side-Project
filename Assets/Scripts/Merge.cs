using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    private Transform Transform;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = ballSprites[(int)ball];
        Transform = GetComponent<Transform>();
        transform.localScale = Vector3.one;
    }

    public void UpgradeBall()
    {
        ball++;
        float scale = 1.5f * (int)ball;
        transform.localScale = scale * new Vector3(1,1,0);
        sr.sprite = ballSprites[(int)ball];
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            var ball1 = this.ball;
            var ball2 = collision.gameObject.GetComponent<Merge>().ball;
            //Debug.Log(ball1 + " is touching " + ball2);
            if (ball2 == ball1)
            { 
                UpgradeBall();
                Destroy(collision.gameObject);
            }
        }
    }
}
