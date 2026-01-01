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
        sr.sprite = ballSprites[stage];
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
                //UpgradeBall();
                SetBallStage((int)ball + 1);

                Destroy(collision.gameObject);
            }
        }
    }
}
