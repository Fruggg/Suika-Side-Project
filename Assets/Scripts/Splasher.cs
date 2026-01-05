using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class ColorPalette
{
    public List<Color> palette;
    
}

public class Splasher : MonoBehaviour
{

    [SerializeField] GameObject[] splashGameObjects;
    [SerializeField] List<ColorPalette> colorSplotches;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float[] squareCollisionTiers;
    [SerializeField] UnityEvent OnSplash;
    int SplashMagnitude()
    {
        for (int i = squareCollisionTiers.Length - 1; i >= 0; i--)
        {
            //It's going fast enough to qualify
            if(rb.linearVelocity.sqrMagnitude >= squareCollisionTiers[i])
            {
                return i;
            }
        }
        return 0;
    }
   
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Nothing when collide with player
        int splashMag = SplashMagnitude();
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ejector")) return;
        foreach (var contact in other.contacts)
        {
            if (!(splashMag >= 1)) return; 
            //Vector2 directionOfNormal = contact.point - contact.normal;

            //float angle = 180 + 180 / 3.14f * Mathf.Atan2(directionOfNormal.y, directionOfNormal.x);
            
            GameObject splash = splashGameObjects[UnityEngine.Random.Range(0, splashGameObjects.Length)];
            var instantiatedSplash = Instantiate(splash, contact.point, Quaternion.identity);

            //this was genuinely random at a point, but we later decided to give meaning to colors

            //int randomPalette = UnityEngine.Random.Range(0, colorSplotches.Count);
            // Only one color since we don't need many colors yet
            Color color = colorSplotches[0].palette[0];
            
            // A little hack to see if they're the same based on mass
            if (contact.collider.attachedRigidbody != null)
            {
                if(contact.collider.attachedRigidbody.mass == rb.mass)
                {
                    
                    color = colorSplotches[1].palette[0];
                }
            }
            instantiatedSplash.GetComponent<SpriteRenderer>().color = color;
            ;
           
        }
    }
}
